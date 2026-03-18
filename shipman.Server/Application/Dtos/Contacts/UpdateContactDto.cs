using shipman.Server.Application.Dtos.Shipments;

namespace shipman.Server.Application.Dtos.Contacts;

public record UpdateContactDto(
    string? Name,
    string? Email,
    string? Phone,
    AddressDto? PrimaryAddress
);
