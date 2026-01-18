using shipman.Server.Data;
using shipman.Server.Domain.Entities;
using shipman.Server.Domain.Enums;

namespace shipman.Server.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        // If there is already data, do nothing
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
                Status = ShipmentStatus.Processing,
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            },
            new Shipment
            {
                Id = Guid.NewGuid(),
                TrackingNumber = "PL000987654",
                Sender = "DHL London Hub",
                Receiver = "Piotr Nowak",
                Status = ShipmentStatus.InTransit,
                CreatedAt = DateTime.UtcNow.AddDays(-3)
            },
            new Shipment
            {
                Id = Guid.NewGuid(),
                TrackingNumber = "PL000555222",
                Sender = "Zalando Warehouse Paris",
                Receiver = "Anna Kowalska",
                Status = ShipmentStatus.Delivered,
                CreatedAt = DateTime.UtcNow.AddDays(-10)
            },
            new Shipment
            {
                Id = Guid.NewGuid(),
                TrackingNumber = "PL000333111",
                Sender = "MediaMarkt Poznań",
                Receiver = "Marek Lewandowski",
                Status = ShipmentStatus.Processing,
                CreatedAt = DateTime.UtcNow.AddHours(-12)
            },
            new Shipment
            {
                Id = Guid.NewGuid(),
                TrackingNumber = "PL000777888",
                Sender = "IKEA Gdańsk",
                Receiver = "Julia Wójcik",
                Status = ShipmentStatus.InTransit,
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new Shipment
            {
                Id = Guid.NewGuid(),
                TrackingNumber = "PL000444999",
                Sender = "Apple Store Munich",
                Receiver = "Tomasz Krawczyk",
                Status = ShipmentStatus.Delivered,
                CreatedAt = DateTime.UtcNow.AddDays(-7)
            },
            new Shipment
            {
                Id = Guid.NewGuid(),
                TrackingNumber = "PL000222444",
                Sender = "Allegro Seller Kraków",
                Receiver = "Ewa Jabłońska",
                Status = ShipmentStatus.Processing,
                CreatedAt = DateTime.UtcNow.AddHours(-5)
            },
            new Shipment
            {
                Id = Guid.NewGuid(),
                TrackingNumber = "PL000666111",
                Sender = "Decathlon Wrocław",
                Receiver = "Łukasz Pawlak",
                Status = ShipmentStatus.InTransit,
                CreatedAt = DateTime.UtcNow.AddDays(-4)
            },
            new Shipment
            {
                Id = Guid.NewGuid(),
                TrackingNumber = "PL000999333",
                Sender = "H&M Stockholm",
                Receiver = "Natalia Szymańska",
                Status = ShipmentStatus.Delivered,
                CreatedAt = DateTime.UtcNow.AddDays(-14)
            },
            new Shipment
            {
                Id = Guid.NewGuid(),
                TrackingNumber = "PL000111222",
                Sender = "Castorama Zielona Góra",
                Receiver = "Adrian Malinowski",
                Status = ShipmentStatus.Processing,
                CreatedAt = DateTime.UtcNow.AddHours(-2)
            }
        };

        db.Shipments.AddRange(shipments);
        db.SaveChanges();
    }
}
