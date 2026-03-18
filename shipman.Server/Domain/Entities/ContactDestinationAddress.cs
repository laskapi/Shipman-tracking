namespace shipman.Server.Domain.Entities;

public class ContactDestinationAddress
{
    public Guid ContactId { get; set; }
    public Contact Contact { get; set; } = default!;

    public Guid AddressId { get; set; }
    public Address Address { get; set; } = default!;
}
