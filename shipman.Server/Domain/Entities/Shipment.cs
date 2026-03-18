using shipman.Server.Domain.Enums;
using shipman.Server.Domain.Extensions;
using shipman.Server.Domain.Rules;

namespace shipman.Server.Domain.Entities;

public class Shipment
{
    public Guid Id { get; set; }

    public required string TrackingNumber { get; set; } = default!;

    public Guid SenderId { get; set; }
    public required Contact Sender { get; set; } = default!;

    public Guid ReceiverId { get; set; }
    public required Contact Receiver { get; set; } = default!;

    public Guid DestinationAddressId { get; set; }
    public required Address DestinationAddress { get; set; } = default!;

    public decimal Weight { get; set; }
    public ServiceType ServiceType { get; set; } = ServiceType.Standard;
    public ShipmentStatus Status { get; set; } = ShipmentStatus.Created;

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
            _ => DateTime.UtcNow.AddDays(3)
        };
    }

    public void AddEvent(ShipmentEvent evt)
    {
        EnsureShipmentIsModifiable();
        ValidateEventSequence(evt);

        Events.Add(evt);
        Status = evt.EventType.ToStatus();
        UpdatedAt = evt.Timestamp;
    }

    private void EnsureShipmentIsModifiable()
    {
        if (Status == ShipmentStatus.Delivered)
            throw new InvalidOperationException("Delivered shipments cannot be modified.");

        if (Status == ShipmentStatus.Cancelled)
            throw new InvalidOperationException("Cancelled shipments cannot be modified.");
    }

    private void ValidateEventSequence(ShipmentEvent evt)
    {
        var last = Events.LastOrDefault()?.EventType ?? ShipmentEventType.Created;

        if (!ShipmentEventFlow.AllowedTransitions[last].Contains(evt.EventType))
            throw new InvalidOperationException("Invalid event transition.");
    }
}
