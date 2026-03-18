namespace shipman.Server.Application.Dtos.Shipments;

public record ShipmentCreateDto(
    Guid SenderId,
    Guid ReceiverId,
    Guid? DestinationAddressId,
    AddressDto? DestinationAddress,
    decimal Weight,
    string ServiceType
);
