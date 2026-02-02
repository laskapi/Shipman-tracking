namespace shipman.Tests.Unit.Fakes;
using shipman.Server.Application.Interfaces;
using shipman.Server.Domain.Entities;

public class FakeNotificationService : INotificationService
{
    public Task ShipmentCancelledAsync(Shipment shipment) => Task.CompletedTask;
    public Task ShipmentCreatedAsync(Shipment shipment) => Task.CompletedTask;
    public Task ShipmentDeliveredAsync(Shipment shipment) => Task.CompletedTask;
}
