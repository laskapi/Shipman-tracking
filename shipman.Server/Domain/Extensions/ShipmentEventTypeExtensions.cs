using shipman.Server.Domain.Enums;

namespace shipman.Server.Domain.Extensions;

public static class ShipmentEventTypeExtensions
{
    public static ShipmentStatus ToStatus(this ShipmentEventType type) =>
        type switch
        {
            ShipmentEventType.Created => ShipmentStatus.Created,
            ShipmentEventType.Prepared => ShipmentStatus.Prepared,
            ShipmentEventType.HandedOver => ShipmentStatus.HandedOver,
            ShipmentEventType.Delivered => ShipmentStatus.Delivered,
            ShipmentEventType.Delayed => ShipmentStatus.Delayed,
            ShipmentEventType.Cancelled => ShipmentStatus.Cancelled,
            _ => ShipmentStatus.Created
        };

    public static string ToDescription(this ShipmentEventType type) =>
        type switch
        {
            ShipmentEventType.Created => "Shipment was created",
            ShipmentEventType.Prepared => "Shipment was prepared",
            ShipmentEventType.HandedOver => "Shipment was handed over",
            ShipmentEventType.Delivered => "Shipment was delivered",
            ShipmentEventType.Delayed => "Shipment was delayed",
            ShipmentEventType.Cancelled => "Shipment was cancelled",
            _ => "Shipment event occurred"
        };
}
