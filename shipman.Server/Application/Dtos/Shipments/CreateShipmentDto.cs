using shipman.Server.Domain.Enums;

public class CreateShipmentDto
{
    public ContactDto Sender { get; set; } = default!;
    public ContactDto Receiver { get; set; } = default!;
    public decimal Weight { get; set; }
    public ServiceType ServiceType { get; set; } = ServiceType.Standard;
}

public class ContactDto
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Address { get; set; } = default!;
}
