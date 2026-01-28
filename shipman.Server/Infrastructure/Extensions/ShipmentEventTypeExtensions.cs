using shipman.Server.Domain.Enums;

namespace shipman.Server.Infrastructure.Extensions;

public static class ShipmentEventTypeExtensions
{
    public static ShipmentStatus ToStatus(this ShipmentEventType eventType)
    {
        return eventType switch
        {
            ShipmentEventType.Created => ShipmentStatus.Processing,
            ShipmentEventType.PickedUp => ShipmentStatus.Shipped,
            ShipmentEventType.InTransit => ShipmentStatus.InTransit,
            ShipmentEventType.ArrivedAtFacility => ShipmentStatus.InTransit,
            ShipmentEventType.DepartedFacility => ShipmentStatus.InTransit,
            ShipmentEventType.OutForDelivery => ShipmentStatus.InTransit,
            ShipmentEventType.Delivered => ShipmentStatus.Delivered,
            ShipmentEventType.Cancelled => ShipmentStatus.Cancelled,
            _ => ShipmentStatus.Processing
        };
    }
}
