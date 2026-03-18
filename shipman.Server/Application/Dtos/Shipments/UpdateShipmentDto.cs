namespace shipman.Server.Application.Dtos.Shipments;

public record UpdateShipmentDto(
    Guid? DestinationAddressId,
    AddressDto? DestinationAddress,
    string? ServiceType
);
