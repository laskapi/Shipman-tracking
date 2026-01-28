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
        Events.Add(evt);
        Status = evt.EventType.ToStatus();
        UpdatedAt = evt.Timestamp;
    }
}

