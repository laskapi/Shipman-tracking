using shipman.Server.Application.Dtos.Addresses;

namespace shipman.Server.Application.Dtos.Contacts;

public record CreateContactDto(
    string Name,
    string Email,
    string Phone,
    CreateAddressDto PrimaryAddress
);
