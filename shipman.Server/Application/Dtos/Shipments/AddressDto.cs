namespace shipman.Server.Application.Dtos.Shipments;

public record AddressDto(
    Guid Id,
    string Street,
    string HouseNumber,
    string? ApartmentNumber,
    string City,
    string PostalCode,
    string Country,
    double Latitude,
    double Longitude
);
