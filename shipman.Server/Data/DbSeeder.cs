using shipman.Server.Data;
using shipman.Server.Domain.Entities;
using shipman.Server.Domain.Enums;

public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        if (db.Shipments.Any())
            return;

        var shipments = new List<Shipment>();

        // Shipment 1 — Standard
        var s1 = new Shipment
        {
            Id = Guid.NewGuid(),
            TrackingNumber = "PL000111222",
            Sender = "Castorama Zielona Góra",
            Receiver = "Adrian Malinowski",
            Origin = "Zielona Góra, Poland",
            Destination = "Opole, Poland",
            Weight = 9.8m,
            ServiceType = ServiceType.Standard
        };

        s1.CalculateEstimatedDelivery();

        s1.AddEvent(new ShipmentEvent
        {
            Id = Guid.NewGuid(),
            Timestamp = DateTime.UtcNow.AddHours(-5),
            EventType = ShipmentEventType.Created,
            Location = s1.Origin,
            Description = "Shipment created"
        });

        s1.AddEvent(new ShipmentEvent
        {
            Id = Guid.NewGuid(),
            Timestamp = DateTime.UtcNow.AddHours(-3),
            EventType = ShipmentEventType.PickedUp,
            Location = s1.Origin,
            Description = "Shipment picked up by courier"
        });

        s1.AddEvent(new ShipmentEvent
        {
            Id = Guid.NewGuid(),
            Timestamp = DateTime.UtcNow.AddHours(-1),
            EventType = ShipmentEventType.InTransit,
            Location = "Wrocław Sorting Center",
            Description = "Arrived at sorting facility"
        });

        shipments.Add(s1);


        // Shipment 2 — Express
        var s2 = new Shipment
        {
            Id = Guid.NewGuid(),
            TrackingNumber = "PL000333444",
            Sender = "MediaMarkt Warszawa",
            Receiver = "Kamil Nowak",
            Origin = "Warszawa, Poland",
            Destination = "Poznań, Poland",
            Weight = 2.3m,
            ServiceType = ServiceType.Express
        };

        s2.CalculateEstimatedDelivery();

        s2.AddEvent(new ShipmentEvent
        {
            Id = Guid.NewGuid(),
            Timestamp = DateTime.UtcNow.AddHours(-2),
            EventType = ShipmentEventType.Created,
            Location = s2.Origin,
            Description = "Shipment created"
        });

        s2.AddEvent(new ShipmentEvent
        {
            Id = Guid.NewGuid(),
            Timestamp = DateTime.UtcNow.AddHours(-1),
            EventType = ShipmentEventType.PickedUp,
            Location = s2.Origin,
            Description = "Shipment picked up by courier"
        });

        shipments.Add(s2);


        // Shipment 3 — Freight
        var s3 = new Shipment
        {
            Id = Guid.NewGuid(),
            TrackingNumber = "PL000555666",
            Sender = "IKEA Gdańsk",
            Receiver = "Magda Kowalska",
            Origin = "Gdańsk, Poland",
            Destination = "Kraków, Poland",
            Weight = 45.0m,
            ServiceType = ServiceType.Freight
        };

        s3.CalculateEstimatedDelivery();

        s3.AddEvent(new ShipmentEvent
        {
            Id = Guid.NewGuid(),
            Timestamp = DateTime.UtcNow.AddHours(-10),
            EventType = ShipmentEventType.Created,
            Location = s3.Origin,
            Description = "Shipment created"
        });

        s3.AddEvent(new ShipmentEvent
        {
            Id = Guid.NewGuid(),
            Timestamp = DateTime.UtcNow.AddHours(-8),
            EventType = ShipmentEventType.PickedUp,
            Location = s3.Origin,
            Description = "Shipment picked up by freight carrier"
        });

        s3.AddEvent(new ShipmentEvent
        {
            Id = Guid.NewGuid(),
            Timestamp = DateTime.UtcNow.AddHours(-4),
            EventType = ShipmentEventType.InTransit,
            Location = "Łódź Logistics Hub",
            Description = "Arrived at freight hub"
        });

        shipments.Add(s3);


        db.Shipments.AddRange(shipments);
        db.SaveChanges();
    }
}
