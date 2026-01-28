using shipman.Server.Domain.Entities;
using shipman.Server.Domain.Enums;

public class ShipmentEvent
{
    public Guid Id { get; set; }
    public Guid ShipmentId { get; set; }
    public Shipment Shipment { get; set; } = default!;
    public DateTime Timestamp { get; set; }
    public ShipmentEventType EventType { get; set; } = default!;   // structured
    public string Location { get; set; } = default!;    // human-readable location
    public string Description { get; set; } = default!; // human-readable detail
}
