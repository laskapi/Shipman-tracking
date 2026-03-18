using shipman.Server.Domain.Enums;

namespace shipman.Server.Domain.Entities;

public class ShipmentEvent
{
    public Guid Id { get; set; }

    public Guid ShipmentId { get; set; }
    public Shipment Shipment { get; set; } = default!;

    public DateTime Timestamp { get; set; }
    public ShipmentEventType EventType { get; set; }

    public string? Location { get; set; }
    public string? Description { get; set; }
}
