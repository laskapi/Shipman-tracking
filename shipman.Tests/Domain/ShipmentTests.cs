using shipman.Server.Domain.Entities;
using shipman.Server.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace shipman.Tests.Domain;

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

}
