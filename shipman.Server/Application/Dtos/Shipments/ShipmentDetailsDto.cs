using shipman.Server.Application.Dtos.Shipments;
using shipman.Server.Domain.Enums;

public class ShipmentDetailsDto
{
    public Guid Id { get; set; }
    public string TrackingNumber { get; set; } = default!;
    
    public ContactDto Sender { get; set; } = default!;
    public ContactDto Receiver { get; set; } = default!;

    public string Origin { get; set; } = default!;
    public CoordinatesDto OriginCoordinates { get; set; } = default!;
    public string Destination { get; set; } = default!;
    public CoordinatesDto DestinationCoordinates { get; set; } = default!;
    
    public decimal Weight { get; set; }
    public ServiceType ServiceType { get; set; }
    public ShipmentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? EstimatedDelivery { get; set; }
    public List<ShipmentEventDto> Events { get; set; } = new();
}
