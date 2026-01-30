using shipman.Server.Application.Services;
using shipman.Server.Data;
using shipman.Server.Domain.Entities;
using shipman.Server.Domain.Enums;

namespace shipman.Tests.Unit.Services;

public static class ServiceDataFactory
{
    public static Shipment CreateShipment(
        Guid? id = null,
        ShipmentStatus status = ShipmentStatus.Processing,
        List<ShipmentEvent>? events = null)
    {
        return new Shipment
        {
            Id = id ?? Guid.NewGuid(),
            TrackingNumber = "TN123",
            Sender = "Sender",
            Receiver = "Receiver",
            Origin = "A",
            Destination = "B",
            Weight = 1,
            ServiceType = ServiceType.Standard,
            Status = status,
            Events = events ?? new List<ShipmentEvent>()
        };
    }

    public static ShipmentEvent CreateEvent(
        ShipmentEventType type,
        Guid shipmentId)
    {
        return new ShipmentEvent
        {
            Id = Guid.NewGuid(),
            ShipmentId = shipmentId,
            Timestamp = DateTime.UtcNow,
            EventType = type,
            Location = "A",
            Description = type.ToString()
        };
    }

    public static ShipmentService CreateService(IAppDbContext db)
    {
        return new ShipmentService(db);
    }

    public static AddShipmentEventDto CreateEventDto(
    ShipmentEventType type,
    string? location = "A",
    string? description = null)
    {
        return new AddShipmentEventDto
        {
            EventType = type,
            Location = location,
            Description = description ?? type.ToString()
        };
    }


}
