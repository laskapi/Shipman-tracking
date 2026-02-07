using shipman.Server.Application.Dtos.Shipments;
using shipman.Server.Domain.Entities;

namespace shipman.Server.Application.Mappings;

public static class ShipmentMapper
{
    public static ShipmentDetailsDto ToDetailsDto(this Shipment shipment)
    {
        return new ShipmentDetailsDto
        {
            Id = shipment.Id,
            TrackingNumber = shipment.TrackingNumber,
            Sender = shipment.Sender,
            ReceiverName = shipment.Receiver.Name,
            ReceiverEmail = shipment.Receiver.Email,
            ReceiverPhone = shipment.Receiver.Phone,
            Origin = shipment.Origin,
            Destination = shipment.Destination,
            Weight = shipment.Weight,
            ServiceType = shipment.ServiceType,
            Status = shipment.Status,
            CreatedAt = shipment.CreatedAt,
            UpdatedAt = shipment.UpdatedAt,
            EstimatedDelivery = shipment.EstimatedDelivery,
            
            Events = shipment.Events
                .OrderByDescending(e => e.Timestamp)
                .Select(e => new ShipmentEventDto
                {
                    Timestamp = e.Timestamp,
                    EventType = e.EventType,
                    Location = e.Location,
                    Description = e.Description
                })
                .ToList()
        };
    }
    public static ShipmentListItemDto ToListItemDto(this Shipment shipment)
    {
        return new ShipmentListItemDto
        {
            Id = shipment.Id,
            TrackingNumber = shipment.TrackingNumber,
            Sender = shipment.Sender,
            ReceiverName = shipment.Receiver.Name,
            Origin = shipment.Origin,
            Destination = shipment.Destination,
            Status = shipment.Status,
            UpdatedAt = shipment.UpdatedAt
        };
    }
}
