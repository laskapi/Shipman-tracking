using shipman.Server.Application.Dtos.Shipments;

namespace shipman.Server.Application.Dtos.Contacts;

public class ContactDetailsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;

    public AddressDto PrimaryAddress { get; set; } = default!;
    public List<AddressDto> DestinationAddresses { get; set; } = new();
}
