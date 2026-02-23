using Microsoft.EntityFrameworkCore;
using shipman.Server.Application.Dtos;
using shipman.Server.Application.Dtos.Shipments;
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
        _logger.LogInformation("Creating new shipment for receiver {Receiver}", dto.ReceiverName);

        var (originLat, originLng) = await _geocoding.GeocodeAsync(dto.Origin);
        var (destLat, destLng) = await _geocoding.GeocodeAsync(dto.Destination);

        var shipment = new Shipment
        {
            Id = Guid.NewGuid(),
            TrackingNumber = Guid.NewGuid().ToString("N")[..12].ToUpper(),
            Sender = new Contact(
                dto.SenderName,
                dto.SenderEmail,
            dto.SenderPhone
            ),
            Receiver = new Contact(
                dto.ReceiverName,
                dto.ReceiverEmail,
            dto.ReceiverPhone
            ),
            Origin = dto.Origin,
            OriginCoordinates = new Coordinates(originLat, originLng),

            Destination = dto.Destination,
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
                Location = dto.Origin,
                Description = "Shipment created"
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Failed to add Created event to shipment {ShipmentId}", shipment.Id);
            throw;
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


    public async Task<Shipment?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Fetching shipment {ShipmentId}", id);

        return await _db.Shipments
            .AsNoTracking()
            .Include(s => s.Events)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Shipment?> AddEventAsync(Guid id, AddShipmentEventDto dto)
    {
        _logger.LogInformation("Adding event {EventType} to shipment {ShipmentId}", dto.EventType, id);

        var shipment = await _db.Shipments
            .Include(s => s.Events)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (shipment == null)
        {
            _logger.LogWarning("Shipment {ShipmentId} not found when adding event {EventType}", id, dto.EventType);
            return null;
        }

        var evt = new ShipmentEvent
        {
            Id = Guid.NewGuid(),
            ShipmentId = shipment.Id,
            Timestamp = DateTime.UtcNow,
            EventType = dto.EventType,
            Location = shipment.Origin,
            Description = dto.EventType.ToDescription()
        };

        try
        {
            shipment.AddEvent(evt);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Invalid event sequence for shipment {ShipmentId}", id);
            throw;
        }

        _db.ShipmentEvents.Add(evt);
        await _db.SaveChangesAsync();

        _logger.LogInformation("Event {EventType} added to shipment {ShipmentId}", dto.EventType, id);

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


    public async Task<Shipment?> UpdateShipmentAsync(Guid id, UpdateShipmentDto dto)
    {
        _logger.LogInformation("Updating shipment {ShipmentId}", id);

        var shipment = await _db.Shipments
            .Include(s => s.Events)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (shipment == null)
        {
            _logger.LogWarning("Shipment {ShipmentId} not found for update", id);
            return null;
        }

        if (shipment.Status is ShipmentStatus.Delivered or ShipmentStatus.Cancelled)
        {
            _logger.LogWarning("Attempt to update completed shipment {ShipmentId}", id);
            throw new InvalidOperationException("Cannot update a completed shipment.");
        }

        if (dto.Destination != null)
            shipment.Destination = dto.Destination;

        if (dto.Weight.HasValue)
            shipment.Weight = dto.Weight.Value;

        if (dto.ServiceType.HasValue)
            shipment.ServiceType = dto.ServiceType.Value;

        await _db.SaveChangesAsync();

        _logger.LogInformation("Shipment {ShipmentId} updated successfully", id);

        return shipment;
    }

    public async Task<bool> DeleteShipmentAsync(Guid id)
    {
        _logger.LogInformation("Deleting shipment {ShipmentId}", id);

        var shipment = await _db.Shipments
            .Include(s => s.Events)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (shipment == null)
        {
            _logger.LogWarning("Shipment {ShipmentId} not found for deletion", id);
            return false;
        }

        _db.Shipments.Remove(shipment);
        await _db.SaveChangesAsync();

        _logger.LogInformation("Shipment {ShipmentId} deleted", id);

        return true;
    }
}
