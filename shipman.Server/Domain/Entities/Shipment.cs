using shipman.Server.Domain.Enums;
public class Shipment
{
    public Guid Id { get; set; }
    public string TrackingNumber { get; set; } = default!;
    public string Sender { get; set; } = default!;
    public string Receiver { get; set; } = default!;

    public string Origin { get; set; } = default!;
    public string Destination { get; set; } = default!;
    public decimal Weight { get; set; }
    public string ServiceType { get; set; } = "Standard";

    public ShipmentStatus Status { get; set; } = ShipmentStatus.Processing;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? EstimatedDelivery { get; set; }

    public List<ShipmentEvent> Events { get; set; } = new();
}

