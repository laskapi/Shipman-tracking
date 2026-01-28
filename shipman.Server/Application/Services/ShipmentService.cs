using Microsoft.EntityFrameworkCore;
using shipman.Server.Application.Dtos;
using shipman.Server.Application.Interfaces;
using shipman.Server.Data;
using shipman.Server.Domain.Entities;
using shipman.Server.Domain.Enums;
using shipman.Server.Infrastructure.Extensions;
using System.Reflection;

namespace shipman.Server.Application.Services;

public class ShipmentService : IShipmentService
{
    private readonly AppDbContext _db;
    public ShipmentService(AppDbContext db)
    {
        _db = db;
    }
    public async Task<Shipment> CreateShipmentAsync(CreateShipmentDto request)
    {
        var shipment = new Shipment
        {
            Id = Guid.NewGuid(),
            TrackingNumber = Guid.NewGuid().ToString("N")[..12].ToUpper(),
            Sender = request.Sender,
            Receiver = request.Receiver,
            Origin = request.Origin,
            Destination = request.Destination,
            Weight = request.Weight,
            ServiceType = request.ServiceType
        };

        shipment.CalculateEstimatedDelivery();

        try
        {
            shipment.AddEvent(new ShipmentEvent
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                EventType = ShipmentEventType.Created,
                Location = request.Origin,
                Description = "Shipment created"
            });
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException($"Failed to create shipment: {ex.Message}");
        }

        _db.Shipments.Add(shipment);
        await _db.SaveChangesAsync();

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
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.TrackingNumber))
            query = query.Where(s => s.TrackingNumber.Contains(filter.TrackingNumber));

        if (filter.Status.HasValue)
            query = query.Where(s => s.Status == filter.Status.Value);

        bool asc = direction.Equals("asc", StringComparison.OrdinalIgnoreCase);

        var property = typeof(Shipment)
            .GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        if (property != null)
        {
            query = query.OrderByProperty(property.Name, asc);
        }
        else
        {
            query = asc
                ? query.OrderBy(s => s.UpdatedAt)
                : query.OrderByDescending(s => s.UpdatedAt);
        }

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

    public async Task<Shipment?> GetByIdAsync(Guid id)
    {
        return await _db.Shipments
            .AsNoTracking()
            .Include(s => s.Events)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
    public async Task<Shipment?> AddEventAsync(Guid id, AddShipmentEventDto dto)
    {
        var shipment = await _db.Shipments
            .Include(s => s.Events)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (shipment == null)
            return null;

        var evt = new ShipmentEvent
        {
            Id = Guid.NewGuid(),
            ShipmentId = shipment.Id,
            Timestamp = DateTime.UtcNow,
            EventType = dto.EventType,
            Location = dto.Location ?? shipment.Origin,
            Description = dto.Description ?? dto.EventType.ToString()
        };

        try
        {
            shipment.AddEvent(evt);
        }
        catch (InvalidOperationException ex)
        {
            // Let the controller translate this into a 400 Bad Request
            throw new InvalidOperationException(ex.Message);
        }

        await _db.SaveChangesAsync();
        return shipment;
    }

    public async Task<Shipment?> CancelShipmentAsync(Guid id)
    {
        var shipment = await _db.Shipments
            .Include(s => s.Events)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (shipment == null)
            return null;

        var evt = new ShipmentEvent
        {
            Id = Guid.NewGuid(),
            ShipmentId = shipment.Id,
            Timestamp = DateTime.UtcNow,
            EventType = ShipmentEventType.Cancelled,
            Location = shipment.Origin,
            Description = "Shipment cancelled"
        };

        try
        {
            shipment.AddEvent(evt);
        }
        catch (InvalidOperationException ex)
        {
            throw new InvalidOperationException(ex.Message);
        }

        await _db.SaveChangesAsync();
        return shipment;
    }


}
