using shipman.Server.Domain.Enums;

public class ShipmentFilterDto
{
    public string? TrackingNumber { get; set; }
    public ShipmentStatus? Status { get; set; }
}
