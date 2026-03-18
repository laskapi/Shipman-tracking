namespace shipman.Server.Domain.Entities;

public class Contact
{
    public Guid Id { get; set; }

    public required string Name { get; set; } = default!;
    public required string Email { get; set; } = default!;
    public required string Phone { get; set; } = default!;

    public Guid PrimaryAddressId { get; set; }
    public required Address PrimaryAddress { get; set; } = default!;

    public ICollection<ContactDestinationAddress> DestinationAddresses { get; set; }
        = new List<ContactDestinationAddress>();
}
