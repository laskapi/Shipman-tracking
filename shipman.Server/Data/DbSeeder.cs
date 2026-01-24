using shipman.Server.Data;
using shipman.Server.Domain.Entities;
using shipman.Server.Domain.Enums;

namespace shipman.Server.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        if (db.Shipments.Any())
            return;

        var shipments = new List<Shipment>
        {
            new Shipment
            {
                Id = Guid.NewGuid(),
                TrackingNumber = "PL000123456",
                Sender = "Amazon Fulfillment Center Berlin",
                Receiver = "Katarzyna Zielińska",
                Origin = "Berlin, Germany",
                Destination = "Wrocław, Poland",
                Weight = 3.2m,
                ServiceType = "Express",
                Status = ShipmentStatus.Processing,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow.AddHours(-2),
                EstimatedDelivery = DateTime.UtcNow.AddDays(1),
                Events = new List<ShipmentEvent>
                {
                    new ShipmentEvent { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow.AddDays(-1), Description = "Shipment created" },
                    new ShipmentEvent { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow.AddHours(-20), Description = "Picked up from sender" }
                }
            },
            new Shipment
            {
                Id = Guid.NewGuid(),
                TrackingNumber = "PL000987654",
                Sender = "DHL London Hub",
                Receiver = "Piotr Nowak",
                Origin = "London, UK",
                Destination = "Poznań, Poland",
                Weight = 1.8m,
                ServiceType = "Standard",
                Status = ShipmentStatus.InTransit,
                CreatedAt = DateTime.UtcNow.AddDays(-3),
                UpdatedAt = DateTime.UtcNow.AddHours(-5),
                EstimatedDelivery = DateTime.UtcNow.AddDays(2),
                Events = new List<ShipmentEvent>
                {
                    new ShipmentEvent { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow.AddDays(-3), Description = "Shipment created" },
                    new ShipmentEvent { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow.AddDays(-2), Description = "Departed from London hub" },
                    new ShipmentEvent { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow.AddDays(-1), Description = "Arrived in Poland" }
                }
            },
            new Shipment
            {
                Id = Guid.NewGuid(),
                TrackingNumber = "PL000555222",
                Sender = "Zalando Warehouse Paris",
                Receiver = "Anna Kowalska",
                Origin = "Paris, France",
                Destination = "Kraków, Poland",
                Weight = 0.9m,
                ServiceType = "Standard",
                Status = ShipmentStatus.Delivered,
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                UpdatedAt = DateTime.UtcNow.AddDays(-1),
                EstimatedDelivery = DateTime.UtcNow.AddDays(-1),
                Events = new List<ShipmentEvent>
                {
                    new ShipmentEvent { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow.AddDays(-10), Description = "Shipment created" },
                    new ShipmentEvent { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow.AddDays(-9), Description = "Picked up from sender" },
                    new ShipmentEvent { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow.AddDays(-7), Description = "Arrived at sorting facility" },
                    new ShipmentEvent { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow.AddDays(-1), Description = "Delivered to receiver" }
                }
            },
            new Shipment
            {
                Id = Guid.NewGuid(),
                TrackingNumber = "PL000333111",
                Sender = "MediaMarkt Poznań",
                Receiver = "Marek Lewandowski",
                Origin = "Poznań, Poland",
                Destination = "Gdańsk, Poland",
                Weight = 6.5m,
                ServiceType = "Economy",
                Status = ShipmentStatus.Processing,
                CreatedAt = DateTime.UtcNow.AddHours(-12),
                UpdatedAt = DateTime.UtcNow.AddHours(-3),
                EstimatedDelivery = DateTime.UtcNow.AddDays(3),
                Events = new List<ShipmentEvent>
                {
                    new ShipmentEvent { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow.AddHours(-12), Description = "Shipment created" }
                }
            },
            new Shipment
            {
                Id = Guid.NewGuid(),
                TrackingNumber = "PL000777888",
                Sender = "IKEA Gdańsk",
                Receiver = "Julia Wójcik",
                Origin = "Gdańsk, Poland",
                Destination = "Warsaw, Poland",
                Weight = 12.4m,
                ServiceType = "Freight",
                Status = ShipmentStatus.InTransit,
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                UpdatedAt = DateTime.UtcNow.AddHours(-6),
                EstimatedDelivery = DateTime.UtcNow.AddDays(1),
                Events = new List<ShipmentEvent>
                {
                    new ShipmentEvent { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow.AddDays(-2), Description = "Shipment created" },
                    new ShipmentEvent { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow.AddDays(-1), Description = "Left Gdańsk distribution center" }
                }
            },
            new Shipment
            {
                Id = Guid.NewGuid(),
                TrackingNumber = "PL000444999",
                Sender = "Apple Store Munich",
                Receiver = "Tomasz Krawczyk",
                Origin = "Munich, Germany",
                Destination = "Łódź, Poland",
                Weight = 1.2m,
                ServiceType = "Express",
                Status = ShipmentStatus.Delivered,
                CreatedAt = DateTime.UtcNow.AddDays(-7),
                UpdatedAt = DateTime.UtcNow.AddDays(-2),
                EstimatedDelivery = DateTime.UtcNow.AddDays(-2),
                Events = new List<ShipmentEvent>
                {
                    new ShipmentEvent { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow.AddDays(-7), Description = "Shipment created" },
                    new ShipmentEvent { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow.AddDays(-6), Description = "Departed Munich" },
                    new ShipmentEvent { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow.AddDays(-2), Description = "Delivered to receiver" }
                }
            },
            new Shipment
            {
                Id = Guid.NewGuid(),
                TrackingNumber = "PL000222444",
                Sender = "Allegro Seller Kraków",
                Receiver = "Ewa Jabłońska",
                Origin = "Kraków, Poland",
                Destination = "Lublin, Poland",
                Weight = 0.5m,
                ServiceType = "Standard",
                Status = ShipmentStatus.Processing,
                CreatedAt = DateTime.UtcNow.AddHours(-5),
                UpdatedAt = DateTime.UtcNow.AddHours(-1),
                EstimatedDelivery = DateTime.UtcNow.AddDays(2),
                Events = new List<ShipmentEvent>
                {
                    new ShipmentEvent { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow.AddHours(-5), Description = "Shipment created" }
                }
            },
            new Shipment
            {
                Id = Guid.NewGuid(),
                TrackingNumber = "PL000666111",
                Sender = "Decathlon Wrocław",
                Receiver = "Łukasz Pawlak",
                Origin = "Wrocław, Poland",
                Destination = "Szczecin, Poland",
                Weight = 4.3m,
                ServiceType = "Standard",
                Status = ShipmentStatus.InTransit,
                CreatedAt = DateTime.UtcNow.AddDays(-4),
                UpdatedAt = DateTime.UtcNow.AddHours(-8),
                EstimatedDelivery = DateTime.UtcNow.AddDays(1),
                Events = new List<ShipmentEvent>
                {
                    new ShipmentEvent { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow.AddDays(-4), Description = "Shipment created" },
                    new ShipmentEvent { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow.AddDays(-3), Description = "Departed Wrocław facility" }
                }
            },
            new Shipment
            {
                Id = Guid.NewGuid(),
                TrackingNumber = "PL000999333",
                Sender = "H&M Stockholm",
                Receiver = "Natalia Szymańska",
                Origin = "Stockholm, Sweden",
                Destination = "Gdynia, Poland",
                Weight = 0.7m,
                ServiceType = "Standard",
                Status = ShipmentStatus.Delivered,
                CreatedAt = DateTime.UtcNow.AddDays(-14),
                UpdatedAt = DateTime.UtcNow.AddDays(-3),
                EstimatedDelivery = DateTime.UtcNow.AddDays(-3),
                Events = new List<ShipmentEvent>
                {
                    new ShipmentEvent { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow.AddDays(-14), Description = "Shipment created" },
                    new ShipmentEvent { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow.AddDays(-13), Description = "Departed Stockholm" },
                    new ShipmentEvent { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow.AddDays(-3), Description = "Delivered to receiver" }
                }
            },
            new Shipment
            {
                Id = Guid.NewGuid(),
                TrackingNumber = "PL000111222",
                Sender = "Castorama Zielona Góra",
                Receiver = "Adrian Malinowski",
                Origin = "Zielona Góra, Poland",
                Destination = "Opole, Poland",
                Weight = 9.8m,
                ServiceType = "Freight",
                Status = ShipmentStatus.Processing,
                CreatedAt = DateTime.UtcNow.AddHours(-2),
                UpdatedAt = DateTime.UtcNow.AddMinutes(-30),
                EstimatedDelivery = DateTime.UtcNow.AddDays(4),
                Events = new List<ShipmentEvent>
                {
                    new ShipmentEvent { Id = Guid.NewGuid(), Timestamp = DateTime.UtcNow.AddHours(-2), Description = "Shipment created" }
                }
            }
        };

        db.Shipments.AddRange(shipments);
        db.SaveChanges();
    }
}
