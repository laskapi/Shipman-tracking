namespace shipman.Server.Application.Dtos.Addresses;

public record CreateAddressDto(
    string Street,
    string HouseNumber,
    string? ApartmentNumber,
    string City,
    string PostalCode,
    string Country
);
