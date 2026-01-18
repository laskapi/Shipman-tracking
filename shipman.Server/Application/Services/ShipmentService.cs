using Microsoft.EntityFrameworkCore;
using shipman.Server.Data;
using shipman.Server.Domain.Entities;
using shipman.Server.Domain.Enums;

public class ShipmentService : IShipmentService
{
    private readonly AppDbContext _db;

    public ShipmentService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Shipment> CreateAsync(CreateShipmentDto dto)
    {
        var shipment = new Shipment
        {
            Id = Guid.NewGuid(),
            TrackingNumber = Guid.NewGuid().ToString().Substring(0, 8),
            Sender = dto.Sender,
            Receiver = dto.Receiver,
            Status = ShipmentStatus.Processing,
            CreatedAt = DateTime.UtcNow
        };

        _db.Shipments.Add(shipment);
        await _db.SaveChangesAsync();

        return shipment;
    }

    public Task<List<Shipment>> GetAllAsync() =>
        _db.Shipments.ToListAsync();

    public Task<Shipment?> GetByIdAsync(Guid id) =>
        _db.Shipments.FirstOrDefaultAsync(s => s.Id == id);

    public async Task<Shipment> UpdateStatusAsync(Guid id, ShipmentStatus status)
    {
        var shipment = await _db.Shipments.FindAsync(id)
            ?? throw new Exception("Shipment not found");

        shipment.Status = status;
        await _db.SaveChangesAsync();

        return shipment;
    }
}
