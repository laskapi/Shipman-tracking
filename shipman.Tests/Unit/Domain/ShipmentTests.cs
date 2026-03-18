using shipman.Server.Domain.Enums;

namespace shipman.Tests.Unit.Domain;

public class ShipmentTests
{
    [Fact]
    public void CannotDeliverBeforeHandedOver()
    {
        var shipment = DomainFactory.Shipment();

        var deliveredEvent = DomainFactory.Event(ShipmentEventType.Delivered, shipment.Id);

        Assert.Throws<InvalidOperationException>(() =>
            shipment.AddEvent(deliveredEvent)
        );
    }

    [Fact]
    public void CannotHandOverTwice()
    {
        var shipment = DomainFactory.Shipment();

        shipment.AddEvent(DomainFactory.Event(ShipmentEventType.Prepared, shipment.Id));
        shipment.AddEvent(DomainFactory.Event(ShipmentEventType.HandedOver, shipment.Id));

        var secondHandOver = DomainFactory.Event(ShipmentEventType.HandedOver, shipment.Id);

        Assert.Throws<InvalidOperationException>(() =>
            shipment.AddEvent(secondHandOver)
        );
    }

    [Fact]
    public void CannotCancelAfterDelivered()
    {
        var shipment = DomainFactory.Shipment();

        shipment.AddEvent(DomainFactory.Event(ShipmentEventType.Prepared, shipment.Id));
        shipment.AddEvent(DomainFactory.Event(ShipmentEventType.HandedOver, shipment.Id));
        shipment.AddEvent(DomainFactory.Event(ShipmentEventType.Delivered, shipment.Id));

        var cancelEvent = DomainFactory.Event(ShipmentEventType.Cancelled, shipment.Id);

        Assert.Throws<InvalidOperationException>(() =>
            shipment.AddEvent(cancelEvent)
        );
    }

    [Fact]
    public void CannotAddEventsAfterCancellation()
    {
        var shipment = DomainFactory.Shipment();

        shipment.AddEvent(DomainFactory.Event(ShipmentEventType.Cancelled, shipment.Id));

        var handedOverEvent = DomainFactory.Event(ShipmentEventType.HandedOver, shipment.Id);

        Assert.Throws<InvalidOperationException>(() =>
            shipment.AddEvent(handedOverEvent)
        );
    }

    [Fact]
    public void StatusUpdatesCorrectlyBasedOnEventType()
    {
        var shipment = DomainFactory.Shipment();

        shipment.AddEvent(DomainFactory.Event(ShipmentEventType.Prepared, shipment.Id));
        shipment.AddEvent(DomainFactory.Event(ShipmentEventType.HandedOver, shipment.Id));

        Assert.Equal(ShipmentStatus.HandedOver, shipment.Status);
    }

    [Fact]
    public void ValidEventSequence_CompletesWithoutErrors()
    {
        var shipment = DomainFactory.Shipment();

        var sequence = new[]
        {
            ShipmentEventType.Prepared,
            ShipmentEventType.HandedOver,
            ShipmentEventType.Delivered
        };

        foreach (var type in sequence)
            shipment.AddEvent(DomainFactory.Event(type, shipment.Id));

        Assert.Equal(ShipmentStatus.Delivered, shipment.Status);
    }
}
