using Microsoft.EntityFrameworkCore;
using shipman.Server.Data;
using shipman.Server.Domain.Entities;
using shipman.Server.Domain.Enums;
using System.Threading;

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
            ServiceType = request.ServiceType,
            Status = ShipmentStatus.Processing,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Events = new List<ShipmentEvent>
        {
            new ShipmentEvent
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                EventType = "Created",
                Location = request.Origin,
                Description = "Shipment created"
            }
        }
        };

        _db.Shipments.Add(shipment);
        await _db.SaveChangesAsync();

        return shipment;
    }

    public Task<List<Shipment>> GetAllAsync() =>
    _db.Shipments.ToListAsync();


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
            EventType = dto.Status.ToString(),        
            Location = shipment.Origin,               
            Description = $"Status changed to {dto.Status}"
        });

        await _db.SaveChangesAsync();
        return shipment;
    }
}
