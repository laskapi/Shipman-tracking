using Microsoft.EntityFrameworkCore;
using shipman.Server.Application.Dtos;
using shipman.Server.Application.Interfaces;
using shipman.Server.Data;
using shipman.Server.Domain.Entities;
using shipman.Server.Domain.Enums;
using shipman.Server.Infrastructure.Extensions;
using System.Reflection;
using System.Threading;

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
        shipment.AddEvent(new ShipmentEvent
        {
            Id = Guid.NewGuid(),
            Timestamp = DateTime.UtcNow,
            EventType = ShipmentEventType.Created,
            Location = request.Origin,
            Description = "Shipment created"
        });

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
        // Read‑only query: tracking disabled for performance
        var query = _db.Shipments
            .AsNoTracking()
            .AsQueryable();

        // Apply filters (tracking number search + status)
        if (!string.IsNullOrWhiteSpace(filter.TrackingNumber))
            query = query.Where(s => s.TrackingNumber.Contains(filter.TrackingNumber));

        if (filter.Status.HasValue)
            query = query.Where(s => s.Status == filter.Status.Value);

        // Dynamic sorting: reflection used to support arbitrary sortable fields
        bool asc = direction.Equals("asc", StringComparison.OrdinalIgnoreCase);

        var property = typeof(Shipment)
            .GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        if (property != null)
        {
            query = query.OrderByProperty(property.Name, asc);
        }
        else
        {
            // Fallback: UpdatedAt is the most relevant field for shipment dashboards
            query = asc
                ? query.OrderBy(s => s.UpdatedAt)
                : query.OrderByDescending(s => s.UpdatedAt);
        }

        // Total count after filtering (required for pagination metadata)
        var totalCount = await query.CountAsync();

        // Pagination applied last to ensure correct page slices
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
            .Include(s => s.Events)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
    public async Task<Shipment?> UpdateStatusAsync(Guid id, UpdateShipmentStatusDto dto)
    {
        var shipment = await _db.Shipments
            .Include(s => s.Events)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (shipment == null)
            return null;

        // Update status
        shipment.Status = dto.Status;
        shipment.UpdatedAt = DateTime.UtcNow;

        // Add event
        shipment.Events.Add(new ShipmentEvent
        {
            Id = Guid.NewGuid(),
            ShipmentId = shipment.Id,
            Timestamp = DateTime.UtcNow,
//Todo            EventType = dto.status.ToString(),
            Location = shipment.Origin,
            Description = $"Status changed to {dto.Status}"
        });

        await _db.SaveChangesAsync();
        return shipment;
    }
}
