using shipman.Server.Domain.Entities;
using shipman.Server.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace shipman.Tests.Unit.Domain;

public class ShipmentTests
{
    private Shipment CreateShipment()
    {
        return new Shipment
        {
            Id = Guid.NewGuid(),
            Origin = "A",
            Destination = "B",
            Sender = "Sender",
            Receiver = "Receiver",
            Weight = 1,
            ServiceType = ServiceType.Standard
        };
    }
    [Fact]
    public void CannotDeliverBeforePickup()
    {
        var shipment = CreateShipment();

        var deliveredEvent = new ShipmentEvent
        {
            Id = Guid.NewGuid(),
            EventType = ShipmentEventType.Delivered,
            Timestamp = DateTime.UtcNow
        };

        Assert.Throws<InvalidOperationException>(() =>
            shipment.AddEvent(deliveredEvent)
        );
    }
    [Fact]
    public void CannotPickupTwice()
    {
        var shipment = CreateShipment();

        shipment.AddEvent(new ShipmentEvent
        {
            Id = Guid.NewGuid(),
            EventType = ShipmentEventType.PickedUp,
            Timestamp = DateTime.UtcNow
        });

        var secondPickup = new ShipmentEvent
        {
            Id = Guid.NewGuid(),
            EventType = ShipmentEventType.PickedUp,
            Timestamp = DateTime.UtcNow
        };

        Assert.Throws<InvalidOperationException>(() =>
            shipment.AddEvent(secondPickup)
        );
    }
    [Fact]
    public void CannotCancelAfterDelivered()
    {
        var shipment = CreateShipment();

        shipment.AddEvent(new ShipmentEvent { Id = Guid.NewGuid(), EventType = ShipmentEventType.PickedUp, Timestamp = DateTime.UtcNow });
        shipment.AddEvent(new ShipmentEvent { Id = Guid.NewGuid(), EventType = ShipmentEventType.InTransit, Timestamp = DateTime.UtcNow });
        shipment.AddEvent(new ShipmentEvent { Id = Guid.NewGuid(), EventType = ShipmentEventType.ArrivedAtFacility, Timestamp = DateTime.UtcNow });
        shipment.AddEvent(new ShipmentEvent { Id = Guid.NewGuid(), EventType = ShipmentEventType.OutForDelivery, Timestamp = DateTime.UtcNow });
        shipment.AddEvent(new ShipmentEvent { Id = Guid.NewGuid(), EventType = ShipmentEventType.Delivered, Timestamp = DateTime.UtcNow });

        var cancelEvent = new ShipmentEvent
        {
            Id = Guid.NewGuid(),
            EventType = ShipmentEventType.Cancelled,
            Timestamp = DateTime.UtcNow
        };

        Assert.Throws<InvalidOperationException>(() =>
            shipment.AddEvent(cancelEvent)
        );
    }

    [Fact]
    public void CannotAddEventsAfterCancellation()
    {
        var shipment = CreateShipment();
        shipment.AddEvent(new ShipmentEvent
        {
            Id = Guid.NewGuid(),
            EventType = ShipmentEventType.Cancelled,
            Timestamp = DateTime.UtcNow
        });
        var inTransitEvent = new ShipmentEvent
        {
            Id = Guid.NewGuid(),
            EventType = ShipmentEventType.InTransit,
            Timestamp = DateTime.UtcNow
        };
        Assert.Throws<InvalidOperationException>(() =>
        shipment.AddEvent(inTransitEvent));

    }
    [Fact]
    public void StatusUpdatesCorrectlyBasedOnEventType()
    {
        var shipment = CreateShipment();

        shipment.AddEvent(new ShipmentEvent
        {
            Id = Guid.NewGuid(),
            EventType = ShipmentEventType.PickedUp,
            Timestamp = DateTime.UtcNow
        });

        Assert.Equal(ShipmentStatus.Shipped, shipment.Status);
    }
    [Fact]
    public void ValidEventSequence_CompletesWithoutErrors()
    {
        var shipment = CreateShipment();

        var events = new[]
        {
        ShipmentEventType.PickedUp,
        ShipmentEventType.InTransit,
        ShipmentEventType.ArrivedAtFacility,
        ShipmentEventType.OutForDelivery,
        ShipmentEventType.Delivered
    };

        foreach (var type in events)
        {
            shipment.AddEvent(new ShipmentEvent
            {
                Id = Guid.NewGuid(),
                EventType = type,
                Timestamp = DateTime.UtcNow
            });
        }

        Assert.Equal(ShipmentStatus.Delivered, shipment.Status);
    }
    

}
