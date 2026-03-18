using shipman.Server.Application.Dtos.Shipments;

namespace shipman.Tests.Integration.Factories;

public class ShipmentDtoFactory
{
    private readonly Guid _senderId;
    private readonly Guid _receiverId;
    private readonly Guid _destinationAddressId;

    public ShipmentDtoFactory(Guid senderId, Guid receiverId, Guid destinationAddressId)
    {
        _senderId = senderId;
        _receiverId = receiverId;
        _destinationAddressId = destinationAddressId;
    }

    public ShipmentCreateDto Create(
        decimal weight = 2.5m,
        string serviceType = "Standard",
        AddressDto? destinationAddress = null)
    {
        return new ShipmentCreateDto(
            SenderId: _senderId,
            ReceiverId: _receiverId,
            DestinationAddressId: _destinationAddressId,
            DestinationAddress: destinationAddress,
            Weight: weight,
            ServiceType: serviceType
        );
    }

    public UpdateShipmentDto Update(
        Guid? destinationAddressId = null,
        AddressDto? destinationAddress = null,
        string? serviceType = null)
    {
        return new UpdateShipmentDto(
            DestinationAddressId: destinationAddressId,
            DestinationAddress: destinationAddress,
            ServiceType: serviceType
        );
    }

    public AddressDto Address(
        string street = "Via Roma",
        string houseNumber = "1",
        string? apartmentNumber = null,
        string city = "Rome",
        string postalCode = "00100",
        string country = "Italy",
        double latitude = 41.9028,
        double longitude = 12.4964)
    {
        return new AddressDto(
            Street: street,
            HouseNumber: houseNumber,
            ApartmentNumber: apartmentNumber,
            City: city,
            PostalCode: postalCode,
            Country: country,
            Latitude: latitude,
            Longitude: longitude
        );
    }
}
