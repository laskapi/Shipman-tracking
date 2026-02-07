using shipman.Server.Domain.Enums;

namespace shipman.Server.Application.Dtos.Shipments;

public class ShipmentListItemDto
{
    public Guid Id { get; set; }
    public string TrackingNumber { get; set; } = default!;
    public string Sender { get; set; } = default!;
    public string ReceiverName { get; set; } = default!;
    public string Origin { get; set; } = default!;
    public string Destination { get; set; } = default!;
    public ShipmentStatus Status { get; set; }
    public DateTime UpdatedAt { get; set; }
}
