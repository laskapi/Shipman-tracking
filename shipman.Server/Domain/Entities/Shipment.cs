using shipman.Server.Domain.Enums;
using shipman.Server.Infrastructure.Extensions;
namespace shipman.Server.Domain.Entities;

public class Shipment
{
    public Guid Id { get; set; }
    public string TrackingNumber { get; set; } = default!;
    public string Sender { get; set; } = default!;
    public string Receiver { get; set; } = default!;

    public string Origin { get; set; } = default!;
    public string Destination { get; set; } = default!;
    public decimal Weight { get; set; }
    public ServiceType ServiceType { get; set; } = ServiceType.Standard;
    public ShipmentStatus Status { get; set; } = ShipmentStatus.Processing;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? EstimatedDelivery { get; set; }

    public List<ShipmentEvent> Events { get; set; } = new();

    public void CalculateEstimatedDelivery()
    {
        EstimatedDelivery = ServiceType switch
        {
            ServiceType.Express => DateTime.UtcNow.AddDays(1),
            ServiceType.Freight => DateTime.UtcNow.AddDays(5),
            _ => DateTime.UtcNow.AddDays(3) // Standard
        };
    }

    public void AddEvent(ShipmentEvent evt)
    {
        // Delivered shipments are final.
        if (Status == ShipmentStatus.Delivered)
            throw new InvalidOperationException("Cannot add events to a delivered shipment.");

        // Cancelled shipments cannot transition further.
        if (Status == ShipmentStatus.Cancelled)
            throw new InvalidOperationException("Cannot add events to a cancelled shipment.");

        switch (evt.EventType)
        {
            case ShipmentEventType.PickedUp:
                // Pickup can only occur once.
                if (Events.Any(e => e.EventType == ShipmentEventType.PickedUp))
                    throw new InvalidOperationException("Shipment has already been picked up.");
                break;

            case ShipmentEventType.InTransit:
                // Shipment must be picked up before it can be in transit.
                if (!Events.Any(e => e.EventType == ShipmentEventType.PickedUp))
                    throw new InvalidOperationException("Shipment must be picked up before going in transit.");
                break;

            case ShipmentEventType.ArrivedAtFacility:
            case ShipmentEventType.DepartedFacility:
                // Facility events require the shipment to be in transit.
                if (!Events.Any(e => e.EventType == ShipmentEventType.InTransit))
                    throw new InvalidOperationException("Shipment must be in transit before facility events.");
                break;

            case ShipmentEventType.OutForDelivery:
                // Out for delivery requires arrival at facility.
                if (!Events.Any(e => e.EventType == ShipmentEventType.ArrivedAtFacility))
                    throw new InvalidOperationException("Shipment must arrive at a facility before going out for delivery.");
                break;

            case ShipmentEventType.Delivered:
                // Delivery requires out-for-delivery.
                if (!Events.Any(e => e.EventType == ShipmentEventType.OutForDelivery))
                    throw new InvalidOperationException("Shipment must be out for delivery before it can be delivered.");

                // Delivery can only occur once.
                if (Events.Any(e => e.EventType == ShipmentEventType.Delivered))
                    throw new InvalidOperationException("Shipment has already been delivered.");
                break;

            case ShipmentEventType.Cancelled:
                // Cannot cancel after delivery.
                if (Events.Any(e => e.EventType == ShipmentEventType.Delivered))
                    throw new InvalidOperationException("Cannot cancel a delivered shipment.");

                // Cannot cancel twice.
                if (Events.Any(e => e.EventType == ShipmentEventType.Cancelled))
                    throw new InvalidOperationException("Shipment has already been cancelled.");
                break;
        }

        Events.Add(evt);
        Status = evt.EventType.ToStatus();
        UpdatedAt = evt.Timestamp;
    }
}

