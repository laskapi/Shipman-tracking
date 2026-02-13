using Microsoft.Extensions.Logging.Abstractions;
using shipman.Server.Application.Interfaces;
using shipman.Server.Application.Services;
using shipman.Server.Data;
using shipman.Server.Domain.Entities;
using shipman.Server.Domain.Enums;
using shipman.Tests.Unit.Fakes;
using System.Reflection;

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
            Receiver = new Contact("Receiver Name", "receiver@example.com", "+48 600 000 000"),
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

    public static ShipmentService CreateService(
        IAppDbContext db,
        INotificationService? notifications = null)
    {
        var logger = NullLogger<ShipmentService>.Instance;
        notifications ??= new FakeNotificationService();

        return new ShipmentService(logger, db, notifications);
    }




}
