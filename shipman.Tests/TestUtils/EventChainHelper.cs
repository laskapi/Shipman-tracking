using shipman.Server.Application.Dtos;
using shipman.Server.Domain.Enums;
using shipman.Server.Application.Services;

namespace shipman.Tests.TestUtils;

public static class EventChainHelper
{
    public static async Task AddFullDeliveryChainAsync(
        ShipmentService service,
        Guid shipmentId)
    {
        await service.AddEventAsync(shipmentId, Create(ShipmentEventType.PickedUp));
        await service.AddEventAsync(shipmentId, Create(ShipmentEventType.InTransit));
        await service.AddEventAsync(shipmentId, Create(ShipmentEventType.ArrivedAtFacility));
        await service.AddEventAsync(shipmentId, Create(ShipmentEventType.DepartedFacility));
        await service.AddEventAsync(shipmentId, Create(ShipmentEventType.OutForDelivery));
    }

    private static AddShipmentEventDto Create(ShipmentEventType type)
        => new AddShipmentEventDto
        {
            EventType = type,
            Location = null,
            Description = type.ToString()
        };
}
