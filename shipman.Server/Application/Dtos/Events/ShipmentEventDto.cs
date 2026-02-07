using shipman.Server.Domain.Enums;

public class ShipmentEventDto
{
    public DateTime Timestamp { get; set; }
    public ShipmentEventType EventType { get; set; }
    public string Location { get; set; } = default!;
    public string Description { get; set; } = default!;
}
