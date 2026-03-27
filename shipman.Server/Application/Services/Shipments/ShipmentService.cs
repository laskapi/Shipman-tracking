using Microsoft.EntityFrameworkCore;
using shipman.Server.Application.Dtos;
using shipman.Server.Application.Dtos.Shipments;
using shipman.Server.Application.Exceptions;
using shipman.Server.Application.Interfaces;
using shipman.Server.Data;
using shipman.Server.Domain.Entities;
using shipman.Server.Domain.Enums;
using shipman.Server.Domain.Extensions;

namespace shipman.Server.Application.Services.Shipments;

public class ShipmentService : IShipmentService
{
    private readonly ILogger<ShipmentService> _logger;
    private readonly IAppDbContext _db;
    private readonly INotificationService _notifications;
    private readonly ShipmentFactory _factory;
    private readonly ShipmentUpdater _updater;

    public ShipmentService(
        ILogger<ShipmentService> logger,
        IAppDbContext db,
        INotificationService notifications,
        ShipmentFactory factory,
        ShipmentUpdater updater)
    {
        _logger = logger;
        _db = db;
        _notifications = notifications;
        _factory = factory;
        _updater = updater;
    }

    public async Task<Shipment> CreateShipmentAsync(ShipmentCreateDto dto)
    {
        var shipment = await _factory.CreateAsync(dto);
        _db.Shipments.Add(shipment);
        await _db.SaveChangesAsync();
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
        var query = _db.Shipments
            .Include(s => s.Sender)
            .Include(s => s.Receiver)
            .Include(s => s.DestinationAddress)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            query = query.Where(s =>
                s.TrackingNumber.Contains(filter.Search) ||
                s.Sender.Name.Contains(filter.Search) ||
                s.Receiver.Name.Contains(filter.Search));
        }

        if (!string.IsNullOrWhiteSpace(filter.Status) &&
            Enum.TryParse<ShipmentStatus>(filter.Status, true, out var status))
        {
            query = query.Where(s => s.Status == status);
        }

        query = sortBy.ToLower() switch
        {
            "createdat" => direction == "asc"
                ? query.OrderBy(s => s.CreatedAt)
                : query.OrderByDescending(s => s.CreatedAt),

            "updatedat" => direction == "asc"
                ? query.OrderBy(s => s.UpdatedAt)
                : query.OrderByDescending(s => s.UpdatedAt),

            _ => query.OrderByDescending(s => s.UpdatedAt)
        };

        var totalCount = await query.CountAsync();
        var items = await query
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
            .Include(s => s.Sender).ThenInclude(c => c.PrimaryAddress)
            .Include(s => s.Receiver).ThenInclude(c => c.PrimaryAddress)
            .Include(s => s.DestinationAddress)
            .Include(s => s.Events)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (shipment == null)
            throw new AppNotFoundException("Shipment not found");

        return shipment;
    }
    public async Task<Shipment> AddEventAsync(Guid id, ShipmentEventCreateDto dto)
    {
        var shipment = await _db.Shipments
            .Include(s => s.Events)
            .Include(s => s.DestinationAddress)
            .Include(s => s.Sender).ThenInclude(c => c.PrimaryAddress)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (shipment == null)
            throw new AppNotFoundException("Shipment not found");

        if (!Enum.TryParse<ShipmentEventType>(dto.EventType, true, out var eventType))
        {
            throw new AppValidationException(new Dictionary<string, string[]>
            {
                ["EventType"] = new[] { "Invalid event type" }
            });
        }

        var evt = new ShipmentEvent
        {
            Id = Guid.NewGuid(),
            ShipmentId = shipment.Id,
            Timestamp = DateTime.UtcNow,
            EventType = eventType,
            Location = dto.Location ?? shipment.DestinationAddress.City,
            Description = dto.Description ?? eventType.ToDescription()
        };

        try
        {
            shipment.AddEvent(evt);
        }
        catch
        {
            throw new AppDomainException("Invalid event sequence");
        }

        _db.ShipmentEvents.Add(evt);
        await _db.SaveChangesAsync();

        if (eventType == ShipmentEventType.Delivered)
            await _notifications.ShipmentDeliveredAsync(shipment);

        if (eventType == ShipmentEventType.Cancelled)
            await _notifications.ShipmentCancelledAsync(shipment);

        return shipment;
    }

    public async Task<Shipment> UpdateShipmentAsync(Guid id, ShipmentUpdateDto dto)
    {
        var shipment = await _db.Shipments
            .Include(s => s.DestinationAddress)
            .Include(s => s.Receiver)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (shipment == null)
            throw new AppNotFoundException("Shipment not found");

        if (shipment.Status is ShipmentStatus.Delivered or ShipmentStatus.Cancelled)
            throw new AppDomainException("Cannot update a completed shipment");

        await _updater.UpdateAsync(shipment, dto);
        await _db.SaveChangesAsync();

        return shipment;
    }

    public async Task DeleteShipmentAsync(Guid id)
    {
        var shipment = await _db.Shipments.FirstOrDefaultAsync(s => s.Id == id);

        if (shipment == null)
            throw new AppNotFoundException("Shipment not found");

        _db.Shipments.Remove(shipment);
        await _db.SaveChangesAsync();
    }
}
