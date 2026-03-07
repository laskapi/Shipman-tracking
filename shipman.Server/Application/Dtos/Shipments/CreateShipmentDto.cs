using shipman.Server.Application.Dtos.Shipments;
using shipman.Server.Domain.Enums;

public class CreateShipmentDto
{
    public ContactDto Sender { get; set; } = default!;
    public ContactDto Receiver { get; set; } = default!;
    public decimal Weight { get; set; }
    public ServiceType ServiceType { get; set; } = ServiceType.Standard;
}