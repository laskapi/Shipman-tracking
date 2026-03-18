using Moq;
using shipman.Server.Domain.Enums;
using shipman.Tests.Unit.Dtos;

namespace shipman.Tests.Unit.Services;

public class ShipmentService_NotificationTests
{
    [Fact]
    public async Task CreateShipment_SendsCreatedNotification()
    {
        await using var db = InMemoryDbContextFactory.Create();

        var (sender, receiver) = await ServiceFactory.SeedContactsAsync(db);

        var mock = ServiceFactory.MockNotifications();
        var service = ServiceFactory.Create(db, mock.Object);

        var dto = DtoFactory.CreateShipment(sender.Id, receiver.Id);

        var result = await service.CreateShipmentAsync(dto);

        mock.Verify(x => x.ShipmentCreatedAsync(It.IsAny<shipman.Server.Domain.Entities.Shipment>()), Times.Once);
    }

    [Fact]
    public async Task AddEventAsync_DeliveredEvent_SendsDeliveredNotification()
    {
        await using var db = InMemoryDbContextFactory.Create();

        var shipment = await ServiceFactory.SeedShipmentAsync(db);

        var mock = ServiceFactory.MockNotifications();
        var service = ServiceFactory.Create(db, mock.Object);

        await EventChainHelper.AddFullDeliveryChainAsync(service, shipment.Id);

        mock.Verify(x => x.ShipmentDeliveredAsync(It.IsAny<shipman.Server.Domain.Entities.Shipment>()), Times.Once);
    }

    [Fact]
    public async Task AddEventAsync_CancelledEvent_SendsCancelledNotification()
    {
        await using var db = InMemoryDbContextFactory.Create();

        var shipment = await ServiceFactory.SeedShipmentAsync(db);

        var mock = ServiceFactory.MockNotifications();
        var service = ServiceFactory.Create(db, mock.Object);

        var dto = DtoFactory.CreateEvent(ShipmentEventType.Cancelled);

        await service.AddEventAsync(shipment.Id, dto);

        mock.Verify(x => x.ShipmentCancelledAsync(It.IsAny<shipman.Server.Domain.Entities.Shipment>()), Times.Once);
    }
}
