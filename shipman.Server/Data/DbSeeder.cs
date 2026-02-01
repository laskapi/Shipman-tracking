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

        Shipment CreateShipment(
            string tracking,
            string sender,
            string receiverName,
            string receiverEmail,
            string receiverPhone,
            string origin,
            string destination,
            decimal weight,
            ServiceType serviceType,
            List<(int hoursAgo, ShipmentEventType type, string location, string desc)> events)
        {
            var s = new Shipment
            {
                Id = Guid.NewGuid(),
                TrackingNumber = tracking,
                Sender = sender,
                Receiver = new Receiver(receiverName, receiverEmail, receiverPhone),
                Origin = origin,
                Destination = destination,
                Weight = weight,
                ServiceType = serviceType
            };

            s.CalculateEstimatedDelivery();

            foreach (var e in events)
            {
                s.AddEvent(new ShipmentEvent
                {
                    Id = Guid.NewGuid(),
                    Timestamp = DateTime.UtcNow.AddHours(-e.hoursAgo),
                    EventType = e.type,
                    Location = e.location,
                    Description = e.desc
                });
            }

            return s;
        }

        shipments.AddRange(new[]
        {
            // 1
            CreateShipment(
                "PL000111222",
                "Castorama Zielona Góra",
                "Adrian Malinowski", "adrian@example.com", "+48 600 111 222",
                "Zielona Góra, Poland",
                "Opole, Poland",
                9.8m,
                ServiceType.Standard,
                new()
                {
                    (5, ShipmentEventType.Created, "Zielona Góra, Poland", "Shipment created"),
                    (3, ShipmentEventType.PickedUp, "Zielona Góra, Poland", "Shipment picked up"),
                    (1, ShipmentEventType.InTransit, "Wrocław Sorting Center", "Arrived at sorting facility")
                }),

            // 2
            CreateShipment(
                "PL000333444",
                "MediaMarkt Warszawa",
                "Kamil Nowak", "kamil@example.com", "+48 600 333 444",
                "Warszawa, Poland",
                "Poznań, Poland",
                2.3m,
                ServiceType.Express,
                new()
                {
                    (2, ShipmentEventType.Created, "Warszawa, Poland", "Shipment created"),
                    (1, ShipmentEventType.PickedUp, "Warszawa, Poland", "Shipment picked up")
                }),

            // 3
            CreateShipment(
                "PL000555666",
                "IKEA Gdańsk",
                "Magda Kowalska", "magda@example.com", "+48 600 555 666",
                "Gdańsk, Poland",
                "Kraków, Poland",
                45.0m,
                ServiceType.Freight,
                new()
                {
                    (10, ShipmentEventType.Created, "Gdańsk, Poland", "Shipment created"),
                    (8, ShipmentEventType.PickedUp, "Gdańsk, Poland", "Shipment picked up"),
                    (4, ShipmentEventType.InTransit, "Łódź Logistics Hub", "Arrived at freight hub")
                }),

            // 4
            CreateShipment(
                "PL000777888",
                "RTV Euro AGD Szczecin",
                "Paweł Zieliński", "pawel@example.com", "+48 600 777 888",
                "Szczecin, Poland",
                "Wrocław, Poland",
                4.2m,
                ServiceType.Standard,
                new()
                {
                    (6, ShipmentEventType.Created, "Szczecin, Poland", "Shipment created"),
                    (4, ShipmentEventType.PickedUp, "Szczecin, Poland", "Shipment picked up")
                }),

            // 5
            CreateShipment(
                "PL000999000",
                "Decathlon Kraków",
                "Karolina Bąk", "karolina@example.com", "+48 600 999 000",
                "Kraków, Poland",
                "Gdynia, Poland",
                6.1m,
                ServiceType.Express,
                new()
                {
                    (3, ShipmentEventType.Created, "Kraków, Poland", "Shipment created"),
                    (2, ShipmentEventType.PickedUp, "Kraków, Poland", "Shipment picked up"),
                    (1, ShipmentEventType.InTransit, "Warszawa Hub", "Arrived at hub")
                }),

            // 6
            CreateShipment(
                "PL001111222",
                "Leroy Merlin Poznań",
                "Marek Wójcik", "marek@example.com", "+48 601 111 222",
                "Poznań, Poland",
                "Łódź, Poland",
                12.5m,
                ServiceType.Standard,
                new()
                {
                    (7, ShipmentEventType.Created, "Poznań, Poland", "Shipment created")
                }),

            // 7
            CreateShipment(
                "PL001333444",
                "Empik Warszawa",
                "Julia Król", "julia@example.com", "+48 601 333 444",
                "Warszawa, Poland",
                "Lublin, Poland",
                1.1m,
                ServiceType.Standard,
                new()
                {
                    (5, ShipmentEventType.Created, "Warszawa, Poland", "Shipment created"),
                    (3, ShipmentEventType.PickedUp, "Warszawa, Poland", "Shipment picked up")
                }),

            // 8
            CreateShipment(
                "PL001555666",
                "Komputronik Wrocław",
                "Tomasz Lewandowski", "tomasz@example.com", "+48 601 555 666",
                "Wrocław, Poland",
                "Katowice, Poland",
                3.4m,
                ServiceType.Express,
                new()
                {
                    (4, ShipmentEventType.Created, "Wrocław, Poland", "Shipment created")
                }),

            // 9
            CreateShipment(
                "PL001777888",
                "Auchan Gdańsk",
                "Anna Szymańska", "anna@example.com", "+48 601 777 888",
                "Gdańsk, Poland",
                "Szczecin, Poland",
                8.0m,
                ServiceType.Standard,
                new()
                {
                    (12, ShipmentEventType.Created, "Gdańsk, Poland", "Shipment created"),
                    (10, ShipmentEventType.PickedUp, "Gdańsk, Poland", "Shipment picked up"),
                    (6, ShipmentEventType.InTransit, "Bydgoszcz Hub", "Arrived at hub")
                }),

            // 10
            CreateShipment(
                "PL002000111",
                "H&M Łódź",
                "Oliwia Pawlak", "oliwia@example.com", "+48 602 000 111",
                "Łódź, Poland",
                "Warszawa, Poland",
                0.9m,
                ServiceType.Standard,
                new()
                {
                    (2, ShipmentEventType.Created, "Łódź, Poland", "Shipment created")
                }),

            // 11
            CreateShipment(
                "PL002222333",
                "Zara Kraków",
                "Filip Mazur", "filip@example.com", "+48 602 222 333",
                "Kraków, Poland",
                "Rzeszów, Poland",
                1.7m,
                ServiceType.Express,
                new()
                {
                    (3, ShipmentEventType.Created, "Kraków, Poland", "Shipment created"),
                    (1, ShipmentEventType.PickedUp, "Kraków, Poland", "Shipment picked up")
                }),

            // 12
            CreateShipment(
                "PL002444555",
                "Biedronka Opole",
                "Natalia Dudek", "natalia@example.com", "+48 602 444 555",
                "Opole, Poland",
                "Wrocław, Poland",
                5.2m,
                ServiceType.Standard,
                new()
                {
                    (4, ShipmentEventType.Created, "Opole, Poland", "Shipment created")
                }),

            // 13
            CreateShipment(
                "PL002666777",
                "Lidl Szczecin",
                "Jakub Adamczyk", "jakub@example.com", "+48 602 666 777",
                "Szczecin, Poland",
                "Gorzów Wielkopolski, Poland",
                2.0m,
                ServiceType.Standard,
                new()
                {
                    (6, ShipmentEventType.Created, "Szczecin, Poland", "Shipment created"),
                    (4, ShipmentEventType.PickedUp, "Szczecin, Poland", "Shipment picked up")
                }),

            // 14
            CreateShipment(
                "PL002888999",
                "Pepco Warszawa",
                "Marta Piotrowska", "marta@example.com", "+48 602 888 999",
                "Warszawa, Poland",
                "Białystok, Poland",
                1.3m,
                ServiceType.Standard,
                new()
                {
                    (3, ShipmentEventType.Created, "Warszawa, Poland", "Shipment created")
                }),

            // 15
            CreateShipment(
                "PL003000111",
                "OBI Katowice",
                "Damian Lis", "damian@example.com", "+48 603 000 111",
                "Katowice, Poland",
                "Kielce, Poland",
                7.9m,
                ServiceType.Freight,
                new()
                {
                    (14, ShipmentEventType.Created, "Katowice, Poland", "Shipment created"),
                    (12, ShipmentEventType.PickedUp, "Katowice, Poland", "Shipment picked up"),
                    (8, ShipmentEventType.InTransit, "Częstochowa Hub", "Arrived at hub")
                })
        });

        db.Shipments.AddRange(shipments);
        db.SaveChanges();
    }
}
