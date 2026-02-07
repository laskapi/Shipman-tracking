namespace shipman.Server.Application.Dtos.Shipments;

using shipman.Server.Domain.Enums;

public class UpdateShipmentDto
{
    public string? Destination { get; set; }
    public decimal? Weight { get; set; }
    public ServiceType? ServiceType { get; set; }
}
