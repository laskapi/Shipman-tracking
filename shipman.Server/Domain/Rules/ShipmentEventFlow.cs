using shipman.Server.Domain.Enums;

namespace shipman.Server.Domain.Rules;

public static class ShipmentEventFlow
{
    public static readonly Dictionary<ShipmentEventType, ShipmentEventType[]> AllowedTransitions =
        new()
        {
            { ShipmentEventType.Created, new[] { ShipmentEventType.Prepared, ShipmentEventType.Cancelled } },
            { ShipmentEventType.Prepared, new[] { ShipmentEventType.HandedOver, ShipmentEventType.Cancelled, ShipmentEventType.Delayed } },
            { ShipmentEventType.HandedOver, new[] { ShipmentEventType.Delivered, ShipmentEventType.Delayed } },
            { ShipmentEventType.Delivered, Array.Empty<ShipmentEventType>() },
            { ShipmentEventType.Delayed, new[] { ShipmentEventType.Prepared, ShipmentEventType.HandedOver, ShipmentEventType.Delivered, ShipmentEventType.Cancelled } },
            { ShipmentEventType.Cancelled, Array.Empty<ShipmentEventType>() }
        };
}
