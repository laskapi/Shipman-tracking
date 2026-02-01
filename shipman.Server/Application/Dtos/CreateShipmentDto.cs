using shipman.Server.Domain.Enums;
public class CreateShipmentDto
{
    public string Sender { get; set; } = default!;
    public string ReceiverName { get; set; } = default!;
    public string ReceiverEmail { get; set; } = default!;
    public string ReceiverPhone { get; set; } = default!;
    public string Origin { get; set; } = default!;
    public string Destination { get; set; } = default!;
    public decimal Weight { get; set; }
    public ServiceType ServiceType { get; set; } = ServiceType.Standard;
}
