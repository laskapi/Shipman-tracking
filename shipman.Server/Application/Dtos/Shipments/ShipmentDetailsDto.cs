using shipman.Server.Application.Dtos.Events;
using shipman.Server.Application.Dtos.Shipments;

public class ShipmentDetailsDto
{
    public Guid Id { get; set; }
    public string TrackingNumber { get; set; } = default!;

    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public Guid DestinationAddressId { get; set; }

    public ContactDto Sender { get; set; } = default!;
    public ContactDto Receiver { get; set; } = default!;
    public AddressDto DestinationAddress { get; set; } = default!;

    public decimal Weight { get; set; }
    public string ServiceType { get; set; } = default!;
    public string Status { get; set; } = default!;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? EstimatedDelivery { get; set; }

    public List<ShipmentEventDto> Events { get; set; } = new();
}
