
namespace shipman.Server.Application.Dtos.Shipments;

public class ShipmentListItemDto
{
    public Guid Id { get; set; }
    public string TrackingNumber { get; set; } = default!;
    public string Sender { get; set; } = default!;
    public string Receiver { get; set; } = default!;
    public string Destination { get; set; } = default!;
    public string Status { get; set; } = default!;
    public DateTime UpdatedAt { get; set; }
}
