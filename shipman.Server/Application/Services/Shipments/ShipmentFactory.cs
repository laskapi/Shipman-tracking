using Microsoft.EntityFrameworkCore;
using shipman.Server.Application.Dtos.Shipments;
using shipman.Server.Application.Exceptions;
using shipman.Server.Data;
using shipman.Server.Domain.Entities;
using shipman.Server.Domain.Enums;

namespace shipman.Server.Application.Services.Shipments;

public class ShipmentFactory
{
    private readonly IAppDbContext _db;

    public ShipmentFactory(IAppDbContext db)
    {
        _db = db;
    }

    public async Task<Shipment> CreateAsync(ShipmentCreateDto dto)
    {
        // Load sender and receiver
        var sender = await _db.Contacts
            .Include(c => c.PrimaryAddress)
            .FirstOrDefaultAsync(c => c.Id == dto.SenderId);

        if (sender == null)
            throw new AppValidationException(new Dictionary<string, string[]>
            {
                ["SenderId"] = new[] { "Sender not found" }
            });

        var receiver = await _db.Contacts
            .Include(c => c.PrimaryAddress)
            .Include(c => c.DestinationAddresses)
                .ThenInclude(x => x.Address)
            .FirstOrDefaultAsync(c => c.Id == dto.ReceiverId);

        if (receiver == null)
            throw new AppValidationException(new Dictionary<string, string[]>
            {
                ["ReceiverId"] = new[] { "Receiver not found" }
            });

        // Resolve destination address
        Address destinationAddress;

        if (dto.DestinationAddressId is not null)
        {
            destinationAddress = await _db.Addresses
                .FirstOrDefaultAsync(a => a.Id == dto.DestinationAddressId.Value)
                ?? throw new AppValidationException(new Dictionary<string, string[]>
                {
                    ["DestinationAddressId"] = new[] { "Destination address not found" }
                });

        }
        else
        {
            // Default: receiver primary address
            destinationAddress = receiver.PrimaryAddress;
        }

        // Parse service type
        if (!Enum.TryParse<ServiceType>(dto.ServiceType, true, out var serviceType))
            throw new AppValidationException(new Dictionary<string, string[]>
            {
                ["ServiceType"] = new[] { "Invalid service type" }
            });

        // Build shipment
        var shipment = new Shipment
        {
            Id = Guid.NewGuid(),
            TrackingNumber = Guid.NewGuid().ToString("N")[..12].ToUpper(),

            SenderId = sender.Id,
            Sender = sender,
            ReceiverId = receiver.Id,
            Receiver = receiver,

            DestinationAddressId = destinationAddress.Id,
            DestinationAddress = destinationAddress,

            Weight = dto.Weight,
            ServiceType = serviceType,
            Status = ShipmentStatus.Created,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        shipment.CalculateEstimatedDelivery();

        // Initial event
        shipment.Events.Add(new ShipmentEvent
        {
            Id = Guid.NewGuid(),
            ShipmentId = shipment.Id,
            Timestamp = DateTime.UtcNow,
            EventType = ShipmentEventType.Created,
            Location = destinationAddress.City,
            Description = "Shipment created"
        });

        return shipment;
    }
}
