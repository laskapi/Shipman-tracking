using Microsoft.EntityFrameworkCore;
using shipman.Server.Application.Exceptions;
using shipman.Server.Domain.Enums;
using shipman.Tests.Unit.Domain;
using shipman.Tests.Unit.Dtos;

namespace shipman.Tests.Unit.Services;

public class ShipmentServiceTests
{
    [Fact]
    public async Task AddEventAsync_AddsEvent_AndSavesChanges()
    {
        await using var db = InMemoryDbContextFactory.Create();

        var shipment = DomainFactory.Shipment();
        db.Shipments.Add(shipment);
        await db.SaveChangesAsync();

        var service = ServiceFactory.Create(db);

        var dto = DtoFactory.CreateEvent(ShipmentEventType.Prepared);

        var result = await service.AddEventAsync(shipment.Id, dto);

        Assert.NotNull(result);
        Assert.Single(result.Events);
        Assert.Equal(ShipmentEventType.Prepared, result.Events.First().EventType);
    }

    [Fact]
    public async Task AddEventAsync_CancelEvent_UpdatesStatusToCancelled()
    {
        await using var db = InMemoryDbContextFactory.Create();

        var events = new[]
        {
            DomainFactory.Event(ShipmentEventType.Prepared),
            DomainFactory.Event(ShipmentEventType.HandedOver),
            DomainFactory.Event(ShipmentEventType.Delayed)
        };

        var shipment = DomainFactory.Shipment(events: events);
        db.Shipments.Add(shipment);
        await db.SaveChangesAsync();

        var service = ServiceFactory.Create(db);

        var dto = DtoFactory.CreateEvent(
            ShipmentEventType.Cancelled,
            description: "Cancelled by sender"
        );

        var result = await service.AddEventAsync(shipment.Id, dto);

        Assert.NotNull(result);
        Assert.Equal(ShipmentStatus.Cancelled, result.Status);
        Assert.Contains(result.Events, e => e.EventType == ShipmentEventType.Cancelled);
    }

    [Fact]
    public async Task AddEventAsync_Throws_WhenShipmentDoesNotExist()
    {
        await using var db = InMemoryDbContextFactory.Create();
        var service = ServiceFactory.Create(db);

        var dto = DtoFactory.CreateEvent(ShipmentEventType.HandedOver);

        await Assert.ThrowsAsync<AppNotFoundException>(() =>
            service.AddEventAsync(Guid.NewGuid(), dto)
        );
    }

    [Fact]
    public async Task DeleteShipmentAsync_RemovesShipment()
    {
        await using var db = InMemoryDbContextFactory.Create();

        var shipment = DomainFactory.Shipment();
        db.Shipments.Add(shipment);
        await db.SaveChangesAsync();

        var service = ServiceFactory.Create(db);

        await service.DeleteShipmentAsync(shipment.Id);

        Assert.False(await db.Shipments.AnyAsync(s => s.Id == shipment.Id));
    }

    [Fact]
    public async Task DeleteShipmentAsync_Throws_WhenShipmentDoesNotExist()
    {
        await using var db = InMemoryDbContextFactory.Create();
        var service = ServiceFactory.Create(db);

        await Assert.ThrowsAsync<AppNotFoundException>(() =>
            service.DeleteShipmentAsync(Guid.NewGuid())
        );
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsShipmentWithEvents()
    {
        await using var db = InMemoryDbContextFactory.Create();

        var events = new[]
        {
            DomainFactory.Event(ShipmentEventType.Prepared)
        };

        var shipment = DomainFactory.Shipment(events: events);
        db.Shipments.Add(shipment);
        await db.SaveChangesAsync();

        var service = ServiceFactory.Create(db);
        var result = await service.GetByIdAsync(shipment.Id);

        Assert.NotNull(result);
        Assert.Equal(shipment.Id, result.Id);
        Assert.Single(result.Events);
        Assert.Equal(ShipmentEventType.Prepared, result.Events.First().EventType);
    }

    [Fact]
    public async Task CreateShipmentAsync_CreatesShipment_WithCreatedEvent()
    {
        await using var db = InMemoryDbContextFactory.Create();

        var (sender, receiver) = await ServiceFactory.SeedContactsAsync(db);

        var service = ServiceFactory.Create(db);

        var dto = DtoFactory.CreateShipment(sender.Id, receiver.Id);

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
