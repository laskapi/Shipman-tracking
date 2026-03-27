using shipman.Server.Application.Dtos.Shipments;
using shipman.Server.Domain.Enums;

namespace shipman.Tests.Unit.Dtos;

public static class DtoFactory
{
    public static ShipmentCreateDto CreateShipment(
        Guid? senderId = null,
        Guid? receiverId = null,
        Guid? destinationAddressId = null,
        decimal weight = 2.5m,
        ServiceType serviceType = ServiceType.Standard)
    {
        return new ShipmentCreateDto(
            SenderId: senderId ?? Guid.NewGuid(),
            ReceiverId: receiverId ?? Guid.NewGuid(),
            DestinationAddressId: destinationAddressId,
            Weight: weight,
            ServiceType: serviceType.ToString()
        );
    }

    public static ShipmentEventCreateDto CreateEvent(
        ShipmentEventType type,
        string? location = "Test Location",
        string? description = null)
    {
        return new ShipmentEventCreateDto
        {
            EventType = type.ToString(),
            Location = location,
            Description = description ?? type.ToString()
        };
    }
}
