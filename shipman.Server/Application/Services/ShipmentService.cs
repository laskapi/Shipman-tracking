using Microsoft.EntityFrameworkCore;
using shipman.Server.Application.Dtos;
using shipman.Server.Application.Dtos.Shipments;
using shipman.Server.Application.Exceptions;
using shipman.Server.Application.Interfaces;
using shipman.Server.Application.Services;
using shipman.Server.Data;
using shipman.Server.Domain.Entities;
using shipman.Server.Domain.Entities.ValueObjects;
using shipman.Server.Domain.Enums;
using shipman.Server.Domain.Extensions;
using shipman.Server.Infrastructure.Extensions;
using System.Reflection;

public class ShipmentService : IShipmentService
{
    private readonly ILogger<ShipmentService> _logger;
    private readonly IAppDbContext _db;
    private readonly INotificationService _notifications;
    private readonly GeocodingService _geocoding;

    public ShipmentService(
        ILogger<ShipmentService> logger,
        IAppDbContext db,
        INotificationService notifications,
        GeocodingService geocoding)
    {
        _logger = logger;
        _db = db;
        _notifications = notifications;
        _geocoding = geocoding;
    }

    public async Task<Shipment> CreateShipmentAsync(CreateShipmentDto dto)
    {
        _logger.LogInformation("Creating new shipment for receiver {Receiver}", dto.Receiver.Name);

        double originLat, originLng;
        double destLat, destLng;

        try
        {
            (originLat, originLng) = await _geocoding.GeocodeAsync(dto.Sender.Address);
        }
        catch (Exception)
        {
            throw new AppValidationException(new Dictionary<string, string[]>
            {
                ["Sender.Address"] = new[] { "Address not found" }
            });
        }

        try
        {
            (destLat, destLng) = await _geocoding.GeocodeAsync(dto.Receiver.Address);
        }
        catch (Exception)
        {
            throw new AppValidationException(new Dictionary<string, string[]>
            {
                ["Receiver.Address"] = new[] { "Address not found" }
            });
        }

        var sender = new Contact(
            dto.Sender.Name,
            dto.Sender.Email,
            dto.Sender.Phone,
            dto.Sender.Address
        );

        var receiver = new Contact(
            dto.Receiver.Name,
            dto.Receiver.Email,
            dto.Receiver.Phone,
            dto.Receiver.Address
        );

        var shipment = new Shipment
        {
            Id = Guid.NewGuid(),
            TrackingNumber = Guid.NewGuid().ToString("N")[..12].ToUpper(),

            Sender = sender,
            Receiver = receiver,

            OriginCoordinates = new Coordinates(originLat, originLng),
            DestinationCoordinates = new Coordinates(destLat, destLng),

            Weight = dto.Weight,
            ServiceType = dto.ServiceType
        };

        shipment.CalculateEstimatedDelivery();

        try
        {
            shipment.AddEvent(new ShipmentEvent
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                EventType = ShipmentEventType.Created,
                Location = dto.Sender.Address,
                Description = "Shipment created"
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Failed to add Created event to shipment {ShipmentId}", shipment.Id);
            throw new AppDomainException("Failed to create shipment event");
        }

        _db.Shipments.Add(shipment);
        await _db.SaveChangesAsync();

        _logger.LogInformation("Shipment {ShipmentId} created successfully", shipment.Id);

        await _notifications.ShipmentCreatedAsync(shipment);
        return shipment;
    }

    public async Task<PagedResultDto<Shipment>> GetAllAsync(
        int page,
        int pageSize,
        ShipmentFilterDto filter,
        string sortBy,
        string direction)
    {
        _logger.LogInformation("Fetching shipments page {Page} with filter {Filter}", page, filter);

        var baseQuery = _db.Shipments.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.TrackingNumber))
            baseQuery = baseQuery.Where(s => s.TrackingNumber.Contains(filter.TrackingNumber));

        if (filter.Status.HasValue)
            baseQuery = baseQuery.Where(s => s.Status == filter.Status.Value);

        var totalCount = await baseQuery.CountAsync();

        bool asc = direction.Equals("asc", StringComparison.OrdinalIgnoreCase);

        var property = typeof(Shipment)
            .GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        IQueryable<Shipment> sortedQuery;

        if (property != null)
        {
            sortedQuery = baseQuery.OrderByProperty(property.Name, asc);
        }
        else
        {
            _logger.LogWarning("Invalid sort property {SortBy}, falling back to UpdatedAt", sortBy);

            sortedQuery = asc
                ? baseQuery.OrderBy(s => s.UpdatedAt)
                : baseQuery.OrderByDescending(s => s.UpdatedAt);
        }

        var items = await sortedQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<Shipment>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }

    public async Task<Shipment> GetByIdAsync(Guid id)
    {
        var shipment = await _db.Shipments
            .AsNoTracking()
            .Include(s => s.Events)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (shipment == null)
            throw new AppNotFoundException("Shipment not found");

        return shipment;
    }


    public async Task<Shipment> AddEventAsync(Guid id, AddShipmentEventDto dto)
    {
        _logger.LogInformation("Adding event {EventType} to shipment {ShipmentId}", dto.EventType, id);

        var shipment = await _db.Shipments
            .Include(s => s.Events)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (shipment == null)
            throw new AppNotFoundException("Shipment not found");

        var evt = new ShipmentEvent
        {
            Id = Guid.NewGuid(),
            ShipmentId = shipment.Id,
            Timestamp = DateTime.UtcNow,
            EventType = dto.EventType,
            Location = shipment.Sender.Address,
            Description = dto.EventType.ToDescription()
        };

        try
        {
            shipment.AddEvent(evt);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Invalid event sequence for shipment {ShipmentId}", id);
            throw new AppDomainException("Invalid event sequence");
        }

        _db.ShipmentEvents.Add(evt);
        await _db.SaveChangesAsync();

        switch (dto.EventType)
        {
            case ShipmentEventType.Delivered:
                await _notifications.ShipmentDeliveredAsync(shipment);
                break;

            case ShipmentEventType.Cancelled:
                await _notifications.ShipmentCancelledAsync(shipment);
                break;
        }

        return shipment;
    }

    public async Task<Shipment> UpdateShipmentAsync(Guid id, UpdateShipmentDto dto)
    {
        _logger.LogInformation("Updating shipment {ShipmentId}", id);

        var shipment = await _db.Shipments
            .Include(s => s.Events)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (shipment == null)
            throw new AppNotFoundException("Shipment not found");

        if (shipment.Status is ShipmentStatus.Delivered or ShipmentStatus.Cancelled)
            throw new AppDomainException("Cannot update a completed shipment");

        if (!string.IsNullOrWhiteSpace(dto.Destination))
        {
            try
            {
                var (lat, lng) = await _geocoding.GeocodeAsync(dto.Destination);
                shipment.Receiver = shipment.Receiver with { Address = dto.Destination };
                shipment.DestinationCoordinates = new Coordinates(lat, lng);
            }
            catch (Exception)
            {
                throw new AppValidationException(new Dictionary<string, string[]>
                {
                    ["Receiver.Address"] = new[] { "Address not found" }
                });
            }
        }

        if (dto.Weight.HasValue)
            shipment.Weight = dto.Weight.Value;

        if (dto.ServiceType.HasValue)
            shipment.ServiceType = dto.ServiceType.Value;

        await _db.SaveChangesAsync();

        return shipment;
    }

    public async Task DeleteShipmentAsync(Guid id)
    {
        _logger.LogInformation("Deleting shipment {ShipmentId}", id);

        var shipment = await _db.Shipments
            .Include(s => s.Events)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (shipment == null)
            throw new AppNotFoundException("Shipment not found");

        _db.Shipments.Remove(shipment);
        await _db.SaveChangesAsync();

        return;
    }
}
