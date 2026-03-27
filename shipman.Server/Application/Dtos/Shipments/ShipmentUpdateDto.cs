namespace shipman.Server.Application.Dtos.Shipments;

public record ShipmentUpdateDto(
    Guid? DestinationAddressId,
    string? ServiceType
);
