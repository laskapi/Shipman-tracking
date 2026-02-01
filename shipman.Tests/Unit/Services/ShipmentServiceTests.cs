using Microsoft.EntityFrameworkCore;
using shipman.Server.Domain.Enums;
using shipman.Tests.TestUtils;

namespace shipman.Tests.Unit.Services;

public class ShipmentServiceTests
{
    [Fact]
    public async Task AddEventAsync_AddsEvent_AndSavesChanges()
    {
        await using var db = InMemoryDbContextFactory.Create();

        var shipmentId = Guid.NewGuid();
        var shipment = ServiceDataFactory.CreateShipment(shipmentId);

        db.Shipments.Add(shipment);
        await db.SaveChangesAsync();

        var service = ServiceDataFactory.CreateService(db);
        var dto = DtoFactory.CreateEventDto(ShipmentEventType.PickedUp);

        var result = await service.AddEventAsync(shipmentId, dto);

        Assert.NotNull(result);
        Assert.Single(result!.Events);
        Assert.Equal(ShipmentEventType.PickedUp, result.Events.First().EventType);
    }

    [Fact]
    public async Task AddEventAsync_CancelEvent_UpdatesStatusToCancelled()
    {
        await using var db = InMemoryDbContextFactory.Create();

        var shipmentId = Guid.NewGuid();
        var events = new List<ShipmentEvent>
        {
            ServiceDataFactory.CreateEvent(ShipmentEventType.PickedUp, shipmentId)
        };

        var shipment = ServiceDataFactory.CreateShipment(shipmentId, events: events);

        db.Shipments.Add(shipment);
        await db.SaveChangesAsync();

        var service = ServiceDataFactory.CreateService(db);
        var dto = DtoFactory.CreateEventDto(
            ShipmentEventType.Cancelled,
            description: "Cancelled by sender"
        );

        var result = await service.AddEventAsync(shipmentId, dto);

        Assert.NotNull(result);
        Assert.Equal(ShipmentStatus.Cancelled, result!.Status);
        Assert.Contains(result.Events, e => e.EventType == ShipmentEventType.Cancelled);
    }

    [Fact]
    public async Task AddEventAsync_ReturnsNull_WhenShipmentDoesNotExist()
    {
        await using var db = InMemoryDbContextFactory.Create();
        var service = ServiceDataFactory.CreateService(db);

        var dto = DtoFactory.CreateEventDto(ShipmentEventType.PickedUp);

        var result = await service.AddEventAsync(Guid.NewGuid(), dto);

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteShipmentAsync_RemovesShipment_AndReturnsTrue()
    {
        await using var db = InMemoryDbContextFactory.Create();

        var shipmentId = Guid.NewGuid();
        var shipment = ServiceDataFactory.CreateShipment(shipmentId);

        db.Shipments.Add(shipment);
        await db.SaveChangesAsync();

        var service = ServiceDataFactory.CreateService(db);

        var result = await service.DeleteShipmentAsync(shipmentId);

        Assert.True(result);
        Assert.False(await db.Shipments.AnyAsync(s => s.Id == shipmentId));
    }

    [Fact]
    public async Task DeleteShipmentAsync_ReturnsFalse_WhenShipmentDoesNotExist()
    {
        await using var db = InMemoryDbContextFactory.Create();
        var service = ServiceDataFactory.CreateService(db);

        var result = await service.DeleteShipmentAsync(Guid.NewGuid());

        Assert.False(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsShipmentWithEvents()
    {
        await using var db = InMemoryDbContextFactory.Create();

        var shipmentId = Guid.NewGuid();
        var events = new List<ShipmentEvent>
        {
            ServiceDataFactory.CreateEvent(ShipmentEventType.PickedUp, shipmentId)
        };

        var shipment = ServiceDataFactory.CreateShipment(shipmentId, events: events);

        db.Shipments.Add(shipment);
        await db.SaveChangesAsync();

        var service = ServiceDataFactory.CreateService(db);
        var result = await service.GetByIdAsync(shipmentId);

        Assert.NotNull(result);
        Assert.Equal(shipmentId, result!.Id);
        Assert.Single(result.Events);
        Assert.Equal(ShipmentEventType.PickedUp, result.Events.First().EventType);
    }

    [Fact]
    public async Task CreateShipmentAsync_CreatesShipment_WithCreatedEvent()
    {
        await using var db = InMemoryDbContextFactory.Create();
        var service = ServiceDataFactory.CreateService(db);

        var dto = DtoFactory.CreateShipment();


        var result = await service.CreateShipmentAsync(dto);

        Assert.NotNull(result);
        Assert.False(string.IsNullOrWhiteSpace(result.TrackingNumber));
        Assert.Equal(12, result.TrackingNumber.Length);
        Assert.NotNull(result.EstimatedDelivery);
        Assert.Single(result.Events);

        var createdEvent = result.Events.First();
        Assert.Equal(ShipmentEventType.Created, createdEvent.EventType);
        Assert.Equal("Shipment created", createdEvent.Description);

        Assert.True(await db.Shipments.AnyAsync(s => s.Id == result.Id));
    }
}
