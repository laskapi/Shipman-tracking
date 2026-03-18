using shipman.Server.Application.Services.Shipments;
using shipman.Server.Domain.Enums;
using shipman.Tests.Unit.Dtos;

namespace shipman.Tests.Unit.Services;

public static class EventChainHelper
{
    /// <summary>
    /// Adds the full valid delivery chain:
    /// Prepared → HandedOver → Delivered
    /// </summary>
    public static async Task AddFullDeliveryChainAsync(
        ShipmentService service,
        Guid shipmentId)
    {
        await service.AddEventAsync(shipmentId, DtoFactory.CreateEvent(ShipmentEventType.Prepared));
        await service.AddEventAsync(shipmentId, DtoFactory.CreateEvent(ShipmentEventType.HandedOver));
        await service.AddEventAsync(shipmentId, DtoFactory.CreateEvent(ShipmentEventType.Delivered));
    }
}
