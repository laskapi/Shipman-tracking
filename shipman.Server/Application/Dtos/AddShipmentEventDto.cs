using shipman.Server.Domain.Enums;
public class AddShipmentEventDto
{
    public ShipmentEventType EventType { get; set; }
    public string? Location { get; set; }
    public string? Description { get; set; }
}
