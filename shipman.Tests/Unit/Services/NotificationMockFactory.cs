using Moq;
using shipman.Server.Application.Interfaces;
using shipman.Server.Domain.Entities;

namespace shipman.Tests.Unit.Services;

public static class NotificationMockFactory
{
    public static Mock<INotificationService> CreateMock()
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
   
}
