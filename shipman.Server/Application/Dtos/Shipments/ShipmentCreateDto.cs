namespace shipman.Server.Application.Dtos.Shipments;

public record ShipmentCreateDto(
    Guid SenderId,
    Guid ReceiverId,
    Guid? DestinationAddressId,
    decimal Weight,
    string ServiceType
);
