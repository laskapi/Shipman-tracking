namespace shipman.Server.Domain.Enums;
public enum ShipmentEventType
{
    Created,
    PickedUp,
    InTransit,
    ArrivedAtFacility,
    DepartedFacility,
    OutForDelivery,
    Delivered,
    Cancelled
}
