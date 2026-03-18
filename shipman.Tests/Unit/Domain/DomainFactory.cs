using shipman.Server.Domain.Entities;
using shipman.Server.Domain.Enums;

namespace shipman.Tests.Unit.Domain;

public static class DomainFactory
{
    public static Address Address(
        string street = "Test Street",
        string house = "10A",
        string? apt = null,
        string city = "Testville",
        string postal = "00-000",
        string country = "Testland",
        double lat = 50.0,
        double lng = 20.0)
    {
        return new Address
        {
            Id = Guid.NewGuid(),
            Street = street,
            HouseNumber = house,
            ApartmentNumber = apt,
            City = city,
            PostalCode = postal,
            Country = country,
            Latitude = lat,
            Longitude = lng
        };
    }

    public static Contact Contact(string name = "Test User")
    {
        var addr = Address();

        return new Contact
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = $"{name.Replace(" ", "").ToLower()}@example.com",
            Phone = "+48 600 000 000",
            PrimaryAddressId = addr.Id,
            PrimaryAddress = addr
        };
    }

    public static Shipment Shipment(
        Guid? id = null,
        ShipmentStatus status = ShipmentStatus.Created,
        IEnumerable<ShipmentEvent>? events = null)
    {
        var sender = Contact("Sender");
        var receiver = Contact("Receiver");
        var destination = Address("Destination Street", "3", null, "Destination City");

        var shipment = new Shipment
        {
            Id = id ?? Guid.NewGuid(),
            TrackingNumber = "TESTTRACK1234",
            SenderId = sender.Id,
            Sender = sender,
            ReceiverId = receiver.Id,
            Receiver = receiver,
            DestinationAddressId = destination.Id,
            DestinationAddress = destination,
            Weight = 1.0m,
            ServiceType = ServiceType.Standard,
            Status = status,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        shipment.CalculateEstimatedDelivery();

        if (events != null)
        {
            foreach (var e in events)
                shipment.AddEvent(e);
        }

        return shipment;
    }

    public static ShipmentEvent Event(
        ShipmentEventType type,
        Guid? shipmentId = null)
    {
        return new ShipmentEvent
        {
            Id = Guid.NewGuid(),
            ShipmentId = shipmentId ?? Guid.NewGuid(),
            Timestamp = DateTime.UtcNow,
            EventType = type,
            Location = "Test Location",
            Description = type.ToString()
        };
    }
}
