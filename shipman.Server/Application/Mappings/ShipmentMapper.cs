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

            Sender = new ContactDto
            {
                Name = shipment.Sender.Name,
                Email = shipment.Sender.Email,
                Phone = shipment.Sender.Phone,
                Address = shipment.Sender.Address
            },

            Receiver = new ContactDto
            {
                Name = shipment.Receiver.Name,
                Email = shipment.Receiver.Email,
                Phone = shipment.Receiver.Phone,
                Address = shipment.Receiver.Address
            },

            Origin = shipment.Sender.Address,
            OriginCoordinates = new CoordinatesDto
            {
                Lat = shipment.OriginCoordinates.Lat,
                Lng = shipment.OriginCoordinates.Lng
            },

            Destination = shipment.Receiver.Address,
            DestinationCoordinates = new CoordinatesDto
            {
                Lat = shipment.DestinationCoordinates.Lat,
                Lng = shipment.DestinationCoordinates.Lng
            },

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

            Sender = shipment.Sender.Name,
            ReceiverName = shipment.Receiver.Name,

            Origin = shipment.Sender.Address,
            Destination = shipment.Receiver.Address,

            Status = shipment.Status,
            UpdatedAt = shipment.UpdatedAt
        };
    }

}
