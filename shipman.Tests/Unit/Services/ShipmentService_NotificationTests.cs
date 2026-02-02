using Moq;
using shipman.Server.Domain.Entities;
using shipman.Server.Domain.Enums;
using shipman.Tests.TestUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace shipman.Tests.Unit.Services
{
    public class ShipmentService_NotificationTests
    {
        [Fact]
        public async Task CreateShipment_SendsCreatedNotification()
        {
            await using var db = InMemoryDbContextFactory.Create();

            var mock = NotificationMockFactory.CreateMock();
            var service = ServiceDataFactory.CreateService(db, mock.Object);

            var dto = DtoFactory.CreateShipment();

            var result = await service.CreateShipmentAsync(dto);

            mock.Verify(x => x.ShipmentCreatedAsync(It.IsAny<Shipment>()), Times.Once);
        }

        [Fact]
        public async Task AddEventAsync_DeliveredEvent_SendsDeliveredNotification()
        {
            await using var db = InMemoryDbContextFactory.Create();

            var shipment = ServiceDataFactory.CreateShipment();
            db.Shipments.Add(shipment);
            await db.SaveChangesAsync();

            var mock = NotificationMockFactory.CreateMock();
            var service = ServiceDataFactory.CreateService(db, mock.Object);

            await EventChainHelper.AddFullDeliveryChainAsync(service, shipment.Id);
            await service.AddEventAsync(shipment.Id, DtoFactory.CreateEventDto(ShipmentEventType.Delivered));

            mock.Verify(x => x.ShipmentDeliveredAsync(It.IsAny<Shipment>()), Times.Once);
        }


        [Fact]
        public async Task AddEventAsync_CancelledEvent_SendsCancelledNotification()
        {
            await using var db = InMemoryDbContextFactory.Create();

            var shipment = ServiceDataFactory.CreateShipment();
            db.Shipments.Add(shipment);
            await db.SaveChangesAsync();

            var mock = NotificationMockFactory.CreateMock();
            var service = ServiceDataFactory.CreateService(db, mock.Object);

            var dto = DtoFactory.CreateEventDto(ShipmentEventType.Cancelled);

            await service.AddEventAsync(shipment.Id, dto);

            mock.Verify(x => x.ShipmentCancelledAsync(It.IsAny<Shipment>()), Times.Once);
        }



    }
}
