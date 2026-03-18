namespace shipman.Server.Application.Dtos.Events;

public class ShipmentEventDto
{
    public DateTime Timestamp { get; set; }
    public string EventType { get; set; } = default!;
    public string? Location { get; set; }
    public string? Description { get; set; }
}
