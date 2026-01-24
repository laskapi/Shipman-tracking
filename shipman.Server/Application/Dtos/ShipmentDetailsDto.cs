using shipman.Server.Domain.Enums;

public class ShipmentDetailsDto
{
    public Guid Id { get; set; }
    public string TrackingNumber { get; set; } = default!;
    public string Sender { get; set; } = default!;
    public string Receiver { get; set; } = default!;
    public string Origin { get; set; } = default!;
    public string Destination { get; set; } = default!;
    public decimal Weight { get; set; }
    public string ServiceType { get; set; } = default!;
    public ShipmentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? EstimatedDelivery { get; set; }
    public List<ShipmentEventDto> Events { get; set; } = new();
}

public class ShipmentEventDto
{
    public DateTime Timestamp { get; set; }
    public string EventType { get; set; } = default!;
    public string Location { get; set; } = default!;
    public string Description { get; set; } = default!;
}
