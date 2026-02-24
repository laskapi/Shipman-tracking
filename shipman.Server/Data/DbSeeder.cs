using shipman.Server.Data;
using shipman.Server.Domain.Entities;
using shipman.Server.Domain.Entities.ValueObjects;
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
            Contact sender,
            Contact receiver,
            Coordinates originCoords,
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

                OriginCoordinates = originCoords,
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
                new Contact("Castorama Zielona Góra", "store1@example.com", "+48 600 000 001", "Zielona Góra, Poland"),
                new Contact("Adrian Malinowski", "adrian@example.com", "+48 600 111 222", "Opole, Poland"),
                new Coordinates(51.9356, 15.5062),
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
                new Contact("MediaMarkt Warszawa", "store2@example.com", "+48 600 000 002", "Warszawa, Poland"),
                new Contact("Kamil Nowak", "kamil@example.com", "+48 600 333 444", "Poznań, Poland"),
                new Coordinates(52.2297, 21.0122),
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
                new Contact("IKEA Gdańsk", "store3@example.com", "+48 600 000 003", "Gdańsk, Poland"),
                new Contact("Magda Kowalska", "magda@example.com", "+48 600 555 666", "Kraków, Poland"),
                new Coordinates(54.3520, 18.6466),
                new Coordinates(50.0647, 19.9450),
                45.0m,
                ServiceType.Freight,
                new()
                {
                    (10, ShipmentEventType.Created, "Gdańsk, Poland", "Shipment created"),
                    (8, ShipmentEventType.PickedUp, "Gdańsk, Poland", "Shipment picked up"),
                    (4, ShipmentEventType.InTransit, "Łódź Logistics Hub", "Arrived at freight hub")
                }),

            // 4 — Wrocław → Szczecin
            CreateShipment(
                "PL000777888",
                new Contact("DPD Wrocław", "dpd@example.com", "+48 600 777 888", "Wrocław, Poland"),
                new Contact("Marek Zieliński", "marek@example.com", "+48 600 888 777", "Szczecin, Poland"),
                new Coordinates(51.1079, 17.0385),
                new Coordinates(53.4285, 14.5528),
                12.4m,
                ServiceType.Standard,
                new()
                {
                    (6, ShipmentEventType.Created, "Wrocław, Poland", "Shipment created")
                }),

            // 5 — Katowice → Rzeszów
            CreateShipment(
                "PL000999000",
                new Contact("RTV Katowice", "rtv@example.com", "+48 600 999 000", "Katowice, Poland"),
                new Contact("Julia Pawlak", "julia@example.com", "+48 600 000 999", "Rzeszów, Poland"),
                new Coordinates(50.2649, 19.0238),
                new Coordinates(50.0413, 21.9990),
                4.1m,
                ServiceType.Standard,
                new()
                {
                    (3, ShipmentEventType.Created, "Katowice, Poland", "Shipment created")
                }),

            // 6 — Łódź → Lublin
            CreateShipment(
                "PL001111222",
                new Contact("Leroy Merlin Łódź", "lm@example.com", "+48 601 111 222", "Łódź, Poland"),
                new Contact("Piotr Lewandowski", "piotr@example.com", "+48 601 222 111", "Lublin, Poland"),
                new Coordinates(51.7592, 19.4550),
                new Coordinates(51.2465, 22.5684),
                7.2m,
                ServiceType.Standard,
                new()
                {
                    (4, ShipmentEventType.Created, "Łódź, Poland", "Shipment created")
                }),

            // 7 — Białystok → Gdynia
            CreateShipment(
                "PL001333444",
                new Contact("Auchan Białystok", "auchan@example.com", "+48 601 333 444", "Białystok, Poland"),
                new Contact("Ola Kwiatkowska", "ola@example.com", "+48 601 444 333", "Gdynia, Poland"),
                new Coordinates(53.1325, 23.1688),
                new Coordinates(54.5189, 18.5305),
                3.5m,
                ServiceType.Express,
                new()
                {
                    (1, ShipmentEventType.Created, "Białystok, Poland", "Shipment created")
                }),

            // 8 — Toruń → Kielce
            CreateShipment(
                "PL001555666",
                new Contact("Toruń Logistics", "torun@example.com", "+48 601 555 666", "Toruń, Poland"),
                new Contact("Karolina Wójcik", "karolina@example.com", "+48 601 666 555", "Kielce, Poland"),
                new Coordinates(53.0138, 18.5984),
                new Coordinates(50.8661, 20.6286),
                6.0m,
                ServiceType.Standard,
                new()
                {
                    (2, ShipmentEventType.Created, "Toruń, Poland", "Shipment created")
                }),

            // 9 — Olsztyn → Bielsko-Biała
            CreateShipment(
                "PL001777888",
                new Contact("Olsztyn Store", "olsztyn@example.com", "+48 601 777 888", "Olsztyn, Poland"),
                new Contact("Tomasz Król", "tomasz@example.com", "+48 601 888 777", "Bielsko-Biała, Poland"),
                new Coordinates(53.7784, 20.4801),
                new Coordinates(49.8224, 19.0444),
                8.9m,
                ServiceType.Standard,
                new()
                {
                    (5, ShipmentEventType.Created, "Olsztyn, Poland", "Shipment created")
                }),

            // 10 — Radom → Gorzów Wielkopolski
            CreateShipment(
                "PL001999000",
                new Contact("Radom Depot", "radom@example.com", "+48 601 999 000", "Radom, Poland"),
                new Contact("Ewa Szymańska", "ewa@example.com", "+48 601 000 999", "Gorzów Wielkopolski, Poland"),
                new Coordinates(51.4027, 21.1471),
                new Coordinates(52.7368, 15.2288),
                5.7m,
                ServiceType.Standard,
                new()
                {
                    (4, ShipmentEventType.Created, "Radom, Poland", "Shipment created")
                }),

            // 11 — Sopot → Tarnów
            CreateShipment(
                "PL002111222",
                new Contact("Sopot Boutique", "sopot@example.com", "+48 602 111 222", "Sopot, Poland"),
                new Contact("Michał Baran", "michal@example.com", "+48 602 222 111", "Tarnów, Poland"),
                new Coordinates(54.4416, 18.5601),
                new Coordinates(50.0121, 20.9858),
                2.9m,
                ServiceType.Express,
                new()
                {
                    (1, ShipmentEventType.Created, "Sopot, Poland", "Shipment created")
                }),

            // 12 — Koszalin → Zamość
            CreateShipment(
                "PL002333444",
                new Contact("Koszalin Market", "koszalin@example.com", "+48 602 333 444", "Koszalin, Poland"),
                new Contact("Natalia Lis", "natalia@example.com", "+48 602 444 333", "Zamość, Poland"),
                new Coordinates(54.1943, 16.1722),
                new Coordinates(50.7231, 23.2510),
                11.0m,
                ServiceType.Freight,
                new()
                {
                    (8, ShipmentEventType.Created, "Koszalin, Poland", "Shipment created")
                }),

            // 13 — Płock → Legnica
            CreateShipment(
                "PL002555666",
                new Contact("Płock Warehouse", "plock@example.com", "+48 602 555 666", "Płock, Poland"),
                new Contact("Sebastian Gajda", "sebastian@example.com", "+48 602 666 555", "Legnica, Poland"),
                new Coordinates(52.5463, 19.7065),
                new Coordinates(51.2070, 16.1550),
                7.4m,
                ServiceType.Standard,
                new()
                {
                    (3, ShipmentEventType.Created, "Płock, Poland", "Shipment created")
                }),

            // 14 — Elbląg → Nowy Sącz
            CreateShipment(
                "PL002777888",
                new Contact("Elbląg Center", "elblag@example.com", "+48 602 777 888", "Elbląg, Poland"),
                new Contact("Dominika Urban", "dominika@example.com", "+48 602 888 777", "Nowy Sącz, Poland"),
                new Coordinates(54.1522, 19.4088),
                new Coordinates(49.6210, 20.6970),
                3.2m,
                ServiceType.Standard,
                new()
                {
                    (2, ShipmentEventType.Created, "Elbląg, Poland", "Shipment created")
                }),

            // 15 — Kalisz → Suwałki
            CreateShipment(
                "PL002999000",
                new Contact("Kalisz Depot", "kalisz@example.com", "+48 602 999 000", "Kalisz, Poland"),
                new Contact("Patryk Mazur", "patryk@example.com", "+48 602 000 999", "Suwałki, Poland"),
                new Coordinates(51.7611, 18.0910),
                new Coordinates(54.1118, 22.9300),
                10.5m,
                ServiceType.Standard,
                new()
                {
                    (6, ShipmentEventType.Created, "Kalisz, Poland", "Shipment created")
                })
        });

        db.Shipments.AddRange(shipments);
        db.SaveChanges();
    }
}
