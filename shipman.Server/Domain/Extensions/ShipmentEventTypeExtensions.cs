using shipman.Server.Domain.Enums;

namespace shipman.Server.Domain.Extensions;

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

    public static string ToDescription(this ShipmentEventType type)
    {
        return type switch
        {
            ShipmentEventType.Created =>
                "Shipment was created",

            ShipmentEventType.PickedUp =>
                "Shipment was picked up",

            ShipmentEventType.InTransit =>
                "Shipment is in transit",

            ShipmentEventType.ArrivedAtFacility =>
                "Shipment arrived at a facility",

            ShipmentEventType.DepartedFacility =>
                "Shipment departed a facility",

            ShipmentEventType.OutForDelivery =>
                "Shipment is out for delivery",

            ShipmentEventType.Delivered =>
                "Shipment was delivered",

            ShipmentEventType.Cancelled =>
                "Shipment was cancelled",

            _ => "Shipment event occurred"
        };
    }
}
