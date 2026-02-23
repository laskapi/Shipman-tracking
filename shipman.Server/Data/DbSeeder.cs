using shipman.Server.Data;
using shipman.Server.Domain.Entities;
using shipman.Server.Domain.Enums;
using shipman.Server.Domain.Entities.ValueObjects;

public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        if (db.Shipments.Any())
            return;

        var shipments = new List<Shipment>();

        Shipment CreateShipment(
            string tracking,
            Contact sender,
            Contact receiver,
            string origin,
            Coordinates originCoords,
            string destination,
            Coordinates destinationCoords,
            decimal weight,
            ServiceType serviceType,
            List<(int hoursAgo, ShipmentEventType type, string location, string desc)> events)
        {
            var s = new Shipment
            {
                Id = Guid.NewGuid(),
                TrackingNumber = tracking,

                Sender = sender,
                Receiver = receiver,

                Origin = origin,
                OriginCoordinates = originCoords,

                Destination = destination,
                DestinationCoordinates = destinationCoords,

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
            // 1 — Zielona Góra → Opole
            CreateShipment(
                "PL000111222",
                new Contact("Castorama Zielona Góra", "store1@example.com", "+48 600 000 001"),
                new Contact("Adrian Malinowski", "adrian@example.com", "+48 600 111 222"),
                "Zielona Góra, Poland",
                new Coordinates(51.9356, 15.5062),
                "Opole, Poland",
                new Coordinates(50.6751, 17.9213),
                9.8m,
                ServiceType.Standard,
                new()
                {
                    (5, ShipmentEventType.Created, "Zielona Góra, Poland", "Shipment created"),
                    (3, ShipmentEventType.PickedUp, "Zielona Góra, Poland", "Shipment picked up"),
                    (1, ShipmentEventType.InTransit, "Wrocław Sorting Center", "Arrived at sorting facility")
                }),

            // 2 — Warszawa → Poznań
            CreateShipment(
                "PL000333444",
                new Contact("MediaMarkt Warszawa", "store2@example.com", "+48 600 000 002"),
                new Contact("Kamil Nowak", "kamil@example.com", "+48 600 333 444"),
                "Warszawa, Poland",
                new Coordinates(52.2297, 21.0122),
                "Poznań, Poland",
                new Coordinates(52.4064, 16.9252),
                2.3m,
                ServiceType.Express,
                new()
                {
                    (2, ShipmentEventType.Created, "Warszawa, Poland", "Shipment created"),
                    (1, ShipmentEventType.PickedUp, "Warszawa, Poland", "Shipment picked up")
                }),

            // 3 — Gdańsk → Kraków
            CreateShipment(
                "PL000555666",
                new Contact("IKEA Gdańsk", "store3@example.com", "+48 600 000 003"),
                new Contact("Magda Kowalska", "magda@example.com", "+48 600 555 666"),
                "Gdańsk, Poland",
                new Coordinates(54.3520, 18.6466),
                "Kraków, Poland",
                new Coordinates(50.0647, 19.9450),
                45.0m,
                ServiceType.Freight,
                new()
                {
                    (10, ShipmentEventType.Created, "Gdańsk, Poland", "Shipment created"),
                    (8, ShipmentEventType.PickedUp, "Gdańsk, Poland", "Shipment picked up"),
                    (4, ShipmentEventType.InTransit, "Łódź Logistics Hub", "Arrived at freight hub")
                }),

            // 4 — Szczecin → Wrocław
            CreateShipment(
                "PL000777888",
                new Contact("RTV Euro AGD Szczecin", "store4@example.com", "+48 600 000 004"),
                new Contact("Paweł Zieliński", "pawel@example.com", "+48 600 777 888"),
                "Szczecin, Poland",
                new Coordinates(53.4285, 14.5528),
                "Wrocław, Poland",
                new Coordinates(51.1079, 17.0385),
                4.2m,
                ServiceType.Standard,
                new()
                {
                    (6, ShipmentEventType.Created, "Szczecin, Poland", "Shipment created"),
                    (4, ShipmentEventType.PickedUp, "Szczecin, Poland", "Shipment picked up")
                }),

            // 5 — Kraków → Gdynia
            CreateShipment(
                "PL000999000",
                new Contact("Decathlon Kraków", "store5@example.com", "+48 600 000 005"),
                new Contact("Karolina Bąk", "karolina@example.com", "+48 600 999 000"),
                "Kraków, Poland",
                new Coordinates(50.0647, 19.9450),
                "Gdynia, Poland",
                new Coordinates(54.5189, 18.5305),
                6.1m,
                ServiceType.Express,
                new()
                {
                    (3, ShipmentEventType.Created, "Kraków, Poland", "Shipment created"),
                    (2, ShipmentEventType.PickedUp, "Kraków, Poland", "Shipment picked up"),
                    (1, ShipmentEventType.InTransit, "Warszawa Hub", "Arrived at hub")
                }),

            // 6 — Poznań → Łódź
            CreateShipment(
                "PL001111222",
                new Contact("Leroy Merlin Poznań", "store6@example.com", "+48 600 000 006"),
                new Contact("Marek Wójcik", "marek@example.com", "+48 601 111 222"),
                "Poznań, Poland",
                new Coordinates(52.4064, 16.9252),
                "Łódź, Poland",
                new Coordinates(51.7592, 19.4550),
                12.5m,
                ServiceType.Standard,
                new()
                {
                    (7, ShipmentEventType.Created, "Poznań, Poland", "Shipment created")
                }),

            // 7 — Warszawa → Lublin
            CreateShipment(
                "PL001333444",
                new Contact("Empik Warszawa", "store7@example.com", "+48 600 000 007"),
                new Contact("Julia Król", "julia@example.com", "+48 601 333 444"),
                "Warszawa, Poland",
                new Coordinates(52.2297, 21.0122),
                "Lublin, Poland",
                new Coordinates(51.2465, 22.5684),
                1.1m,
                ServiceType.Standard,
                new()
                {
                    (5, ShipmentEventType.Created, "Warszawa, Poland", "Shipment created"),
                    (3, ShipmentEventType.PickedUp, "Warszawa, Poland", "Shipment picked up")
                }),

            // 8 — Wrocław → Katowice
            CreateShipment(
                "PL001555666",
                new Contact("Komputronik Wrocław", "store8@example.com", "+48 600 000 008"),
                new Contact("Tomasz Lewandowski", "tomasz@example.com", "+48 601 555 666"),
                "Wrocław, Poland",
                new Coordinates(51.1079, 17.0385),
                "Katowice, Poland",
                new Coordinates(50.2649, 19.0238),
                3.4m,
                ServiceType.Express,
                new()
                {
                    (4, ShipmentEventType.Created, "Wrocław, Poland", "Shipment created")
                }),

            // 9 — Gdańsk → Szczecin
            CreateShipment(
                "PL001777888",
                new Contact("Auchan Gdańsk", "store9@example.com", "+48 600 000 009"),
                new Contact("Anna Szymańska", "anna@example.com", "+48 601 777 888"),
                "Gdańsk, Poland",
                new Coordinates(54.3520, 18.6466),
                "Szczecin, Poland",
                new Coordinates(53.4285, 14.5528),
                8.0m,
                ServiceType.Standard,
                new()
                {
                    (12, ShipmentEventType.Created, "Gdańsk, Poland", "Shipment created"),
                    (10, ShipmentEventType.PickedUp, "Gdańsk, Poland", "Shipment picked up"),
                    (6, ShipmentEventType.InTransit, "Bydgoszcz Hub", "Arrived at hub")
                }),

            // 10 — Łódź → Warszawa
            CreateShipment(
                "PL002000111",
                new Contact("H&M Łódź", "store10@example.com", "+48 600 000 010"),
                new Contact("Oliwia Pawlak", "oliwia@example.com", "+48 602 000 111"),
                "Łódź, Poland",
                new Coordinates(51.7592, 19.4550),
                "Warszawa, Poland",
                new Coordinates(52.2297, 21.0122),
                0.9m,
                ServiceType.Standard,
                new()
                {
                    (2, ShipmentEventType.Created, "Łódź, Poland", "Shipment created")
                }),

            // 11 — Kraków → Rzeszów
            CreateShipment(
                "PL002222333",
                new Contact("Zara Kraków", "store11@example.com", "+48 600 000 011"),
                new Contact("Filip Mazur", "filip@example.com", "+48 602 222 333"),
                "Kraków, Poland",
                new Coordinates(50.0647, 19.9450),
                "Rzeszów, Poland",
                new Coordinates(50.0413, 21.9990),
                1.7m,
                ServiceType.Express,
                new()
                {
                    (3, ShipmentEventType.Created, "Kraków, Poland", "Shipment created"),
                    (1, ShipmentEventType.PickedUp, "Kraków, Poland", "Shipment picked up")
                }),

            // 12 — Opole → Wrocław
            CreateShipment(
                "PL002444555",
                new Contact("Biedronka Opole", "store12@example.com", "+48 600 000 012"),
                new Contact("Natalia Dudek", "natalia@example.com", "+48 602 444 555"),
                "Opole, Poland",
                new Coordinates(50.6751, 17.9213),
                "Wrocław, Poland",
                new Coordinates(51.1079, 17.0385),
                5.2m,
                ServiceType.Standard,
                new()
                {
                    (4, ShipmentEventType.Created, "Opole, Poland", "Shipment created")
                }),

            // 13 — Szczecin → Gorzów Wielkopolski
            CreateShipment(
                "PL002666777",
                new Contact("Lidl Szczecin", "store13@example.com", "+48 600 000 013"),
                new Contact("Jakub Adamczyk", "jakub@example.com", "+48 602 666 777"),
                "Szczecin, Poland",
                new Coordinates(53.4285, 14.5528),
                "Gorzów Wielkopolski, Poland",
                new Coordinates(52.7368, 15.2288),
                2.0m,
                ServiceType.Standard,
                new()
                {
                    (6, ShipmentEventType.Created, "Szczecin, Poland", "Shipment created"),
                    (4, ShipmentEventType.PickedUp, "Szczecin, Poland", "Shipment picked up")
                }),

            // 14 — Warszawa → Białystok
            CreateShipment(
                "PL002888999",
                new Contact("Pepco Warszawa", "store14@example.com", "+48 600 000 014"),
                new Contact("Marta Piotrowska", "marta@example.com", "+48 602 888 999"),
                "Warszawa, Poland",
                new Coordinates(52.2297, 21.0122),
                "Białystok, Poland",
                new Coordinates(53.1325, 23.1688),
                1.3m,
                ServiceType.Standard,
                new()
                {
                    (3, ShipmentEventType.Created, "Warszawa, Poland", "Shipment created")
                }),

            // 15 — Katowice → Kielce
            CreateShipment(
                "PL003000111",
                new Contact("OBI Katowice", "store15@example.com", "+48 600 000 015"),
                new Contact("Damian Lis", "damian@example.com", "+48 603 000 111"),
                "Katowice, Poland",
                new Coordinates(50.2649, 19.0238),
                "Kielce, Poland",
                new Coordinates(50.8661, 20.6286),
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
