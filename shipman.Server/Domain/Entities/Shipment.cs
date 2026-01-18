using shipman.Server.Domain.Enums;

namespace shipman.Server.Domain.Entities;

public class Shipment
{
    public Guid Id { get; set; }
    public string TrackingNumber { get; set; } = default!;
    public string Sender { get; set; } = default!;
    public string Receiver { get; set; } = default!;
    public ShipmentStatus Status { get; set; } = ShipmentStatus.Processing;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
