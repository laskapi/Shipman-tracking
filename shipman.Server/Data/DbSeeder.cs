using shipman.Server.Data;
using shipman.Server.Domain.Entities;
using shipman.Server.Domain.Enums;

public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        if (db.Shipments.Any())
            return;

        var addresses = new List<Address>();
        var contacts = new List<Contact>();
        var shipments = new List<Shipment>();

        // -----------------------------
        // Helpers
        // -----------------------------

        Address CreateAddress(
            string street,
            string house,
            string? apt,
            string city,
            string postal,
            string country,
            double lat,
            double lng)
        {
            var a = new Address
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

            addresses.Add(a);
            return a;
        }

        Contact CreateContact(
            string name,
            string email,
            string phone,
            Address primary)
        {
            var c = new Contact
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                Phone = phone,
                PrimaryAddressId = primary.Id,
                PrimaryAddress = primary
            };

            contacts.Add(c);
            return c;
        }

        Shipment CreateShipment(
            string tracking,
            Contact sender,
            Contact receiver,
            Address destination,
            decimal weight,
            ServiceType serviceType,
            List<(int hoursAgo, ShipmentEventType type, string location, string desc)> events)
        {
            var s = new Shipment
            {
                Id = Guid.NewGuid(),
                TrackingNumber = tracking,
                SenderId = sender.Id,
                Sender = sender,
                ReceiverId = receiver.Id,
                Receiver = receiver,
                DestinationAddressId = destination.Id,
                DestinationAddress = destination,
                Weight = weight,
                ServiceType = serviceType,
                Status = ShipmentStatus.Created,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            s.CalculateEstimatedDelivery();

            foreach (var e in events.OrderByDescending(e => e.hoursAgo))
            {
                s.AddEvent(new ShipmentEvent
                {
                    Id = Guid.NewGuid(),
                    ShipmentId = s.Id,
                    Timestamp = DateTime.UtcNow.AddHours(-e.hoursAgo),
                    EventType = e.type,
                    Location = e.location,
                    Description = e.desc
                });
            }

            shipments.Add(s);
            return s;
        }

        // -----------------------------
        // Create Contacts + Addresses
        // -----------------------------

        var addrNorthPoint = CreateAddress("Maple Street", "12A", null, "Riverton", "10-101", "Nordovia", 51.2345, 19.8765);
        var addrAlex = CreateAddress("Oakridge Avenue", "55", "4", "Clearwater", "20-202", "Nordovia", 52.1122, 18.5544);

        var northPoint = CreateContact("NorthPoint Supplies", "contact@northpoint.com", "+48 500 100 200", addrNorthPoint);
        var alexCarter = CreateContact("Alex Carter", "alex.carter@example.com", "+48 500 200 300", addrAlex);

        var addrBlueBox = CreateAddress("ul. Brzozowa", "8", "2B", "Brzozówka", "33-120", "Poland", 50.9876, 20.1234);
        var addrMaya = CreateAddress("Riverbend Road", "14", null, "Stonehaven", "40-404", "Nordovia", 53.2211, 17.9988);

        var blueBox = CreateContact("BlueBox Electronics", "store@bluebox.com", "+48 500 300 400", addrBlueBox);
        var mayaJensen = CreateContact("Maya Jensen", "maya.jensen@example.com", "+48 500 400 500", addrMaya);

        var addrUrbanHome = CreateAddress("ul. Słoneczna", "14", null, "Jasnogród", "55-550", "Poland", 51.5566, 19.3344);
        var addrLiam = CreateAddress("Brookside Lane", "7", "1A", "Brookfield", "60-606", "Nordovia", 52.7788, 18.1122);

        var urbanHome = CreateContact("UrbanHome Depot", "info@urbanhome.com", "+48 500 500 600", addrUrbanHome);
        var liamNovak = CreateContact("Liam Novak", "liam.novak@example.com", "+48 500 600 700", addrLiam);

        var addrQuickShip = CreateAddress("Logistics Way", "3", null, "Westvale", "70-707", "Nordovia", 50.8899, 19.4455);
        var addrSofia = CreateAddress("ul. Kwiatowa", "22", "5", "Nowa Dolina", "80-808", "Poland", 51.6677, 20.5566);

        var quickShip = CreateContact("QuickShip Logistics", "support@quickship.com", "+48 500 700 800", addrQuickShip);
        var sofiaReyes = CreateContact("Sofia Reyes", "sofia.reyes@example.com", "+48 500 800 900", addrSofia);

        // -----------------------------
        // Create Shipments
        // -----------------------------

        CreateShipment(
            "FX100001",
            northPoint,
            alexCarter,
            addrAlex,
            4.5m,
            ServiceType.Standard,
            new()
            {
                (10, ShipmentEventType.Prepared, "Riverton", "Package prepared for dispatch"),
                (6, ShipmentEventType.HandedOver, "Riverton", "Shipment handed over to courier"),
                (1, ShipmentEventType.Delivered, "Clearwater", "Delivered to recipient")
            });

        CreateShipment(
            "FX100002",
            blueBox,
            mayaJensen,
            addrMaya,
            1.2m,
            ServiceType.Express,
            new()
            {
                (6, ShipmentEventType.Prepared, "Brzozówka", "Packed and ready"),
                (3, ShipmentEventType.HandedOver, "Brzozówka", "Courier picked up the shipment")
            });

        CreateShipment(
            "FX100003",
            urbanHome,
            liamNovak,
            addrLiam,
            12.0m,
            ServiceType.Freight,
            new()
            {
                (20, ShipmentEventType.Prepared, "Jasnogród", "Loaded onto freight truck"),
                (10, ShipmentEventType.Delayed, "Stonehaven", "Unexpected delay at checkpoint")
            });

        CreateShipment(
            "FX100004",
            quickShip,
            sofiaReyes,
            addrSofia,
            3.3m,
            ServiceType.Standard,
            new()
            {
                (4, ShipmentEventType.Prepared, "Westvale", "Package prepared"),
                (2, ShipmentEventType.HandedOver, "Westvale", "Shipment handed over to courier"),
                (1, ShipmentEventType.Delivered, "Nowa Dolina", "Delivered successfully")
            });

        CreateShipment(
            "FX100005",
            northPoint,
            mayaJensen,
            addrMaya,
            2.0m,
            ServiceType.Standard,
            new()
            {
                (5, ShipmentEventType.Prepared, "Riverton", "Prepared for dispatch"),
                (3, ShipmentEventType.Cancelled, "Riverton", "Shipment cancelled by merchant")
            });

        CreateShipment(
            "FX100006",
            blueBox,
            liamNovak,
            addrLiam,
            7.8m,
            ServiceType.Express,
            new()
            {
                (7, ShipmentEventType.Prepared, "Brzozówka", "Packed and ready"),
                (4, ShipmentEventType.HandedOver, "Brzozówka", "Courier picked up the shipment")
            });

        CreateShipment(
            "FX100007",
            urbanHome,
            sofiaReyes,
            addrSofia,
            5.4m,
            ServiceType.Standard,
            new()
            {
                (8, ShipmentEventType.Prepared, "Jasnogród", "Prepared for dispatch"),
                (3, ShipmentEventType.Delayed, "Nowa Dolina", "Weather delay reported")
            });

        CreateShipment(
            "FX100008",
            quickShip,
            alexCarter,
            addrAlex,
            9.1m,
            ServiceType.Freight,
            new()
            {
                (12, ShipmentEventType.Prepared, "Westvale", "Loaded onto freight truck")
            });

        // -----------------------------
        // Save to DB
        // -----------------------------

        db.Addresses.AddRange(addresses);
        db.Contacts.AddRange(contacts);
        db.Shipments.AddRange(shipments);
        db.SaveChanges();
    }
}
