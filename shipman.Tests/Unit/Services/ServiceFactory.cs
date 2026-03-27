using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using shipman.Server.Application.Interfaces;
using shipman.Server.Application.Services.Addresses;
using shipman.Server.Application.Services.Shipments;
using shipman.Server.Data;
using shipman.Server.Domain.Entities;
using shipman.Tests.Unit.Domain;
using shipman.Tests.Unit.Fakes;

namespace shipman.Tests.Unit.Services;

public static class ServiceFactory
{
    // -------------------------------------------------------
    // Create ShipmentService with optional mocked notifications
    // -------------------------------------------------------
    public static ShipmentService Create(
        IAppDbContext db,
        INotificationService? notifications = null)
    {
        var logger = NullLogger<ShipmentService>.Instance;

        notifications ??= new FakeNotificationService();

        var geocoding = new FakeGeocodingService();
        var shipmentQueries = new ShipmentQueries((AppDbContext)db);
        var addressService = new AddressService(db, geocoding, shipmentQueries);
        var factory = new ShipmentFactory(db);
        var updater = new ShipmentUpdater(db);

        return new ShipmentService(logger, db, notifications, factory, updater);
    }

    // -------------------------------------------------------
    // Create a mock notification service (Moq)
    // -------------------------------------------------------
    public static Mock<INotificationService> MockNotifications()
    {
        var mock = new Mock<INotificationService>();

        mock.Setup(x => x.ShipmentCreatedAsync(It.IsAny<Shipment>()))
            .Returns(Task.CompletedTask);

        mock.Setup(x => x.ShipmentDeliveredAsync(It.IsAny<Shipment>()))
            .Returns(Task.CompletedTask);

        mock.Setup(x => x.ShipmentCancelledAsync(It.IsAny<Shipment>()))
            .Returns(Task.CompletedTask);

        return mock;
    }

    // -------------------------------------------------------
    // Seed contacts into the in-memory database
    // -------------------------------------------------------
    public static async Task<(Contact sender, Contact receiver)> SeedContactsAsync(IAppDbContext db)
    {
        var sender = DomainFactory.Contact("Sender Test");
        var receiver = DomainFactory.Contact("Receiver Test");

        db.Contacts.Add(sender);
        db.Contacts.Add(receiver);

        await db.SaveChangesAsync();

        return (sender, receiver);
    }

    // -------------------------------------------------------
    // Seed a shipment into the database
    // -------------------------------------------------------
    public static async Task<Shipment> SeedShipmentAsync(IAppDbContext db)
    {
        var shipment = DomainFactory.Shipment();
        db.Shipments.Add(shipment);
        await db.SaveChangesAsync();
        return shipment;
    }
}
