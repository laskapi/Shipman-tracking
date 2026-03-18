using Microsoft.EntityFrameworkCore;
using shipman.Server.Application.Dtos.Shipments;
using shipman.Server.Application.Exceptions;
using shipman.Server.Application.Services.Addresses;
using shipman.Server.Data;
using shipman.Server.Domain.Entities;
using shipman.Server.Domain.Enums;

public class ShipmentUpdater
{
    private readonly IAppDbContext _db;
    private readonly AddressService _addressService;

    public ShipmentUpdater(IAppDbContext db, AddressService addressService)
    {
        _db = db;
        _addressService = addressService;
    }

    public async Task UpdateAsync(Shipment shipment, UpdateShipmentDto dto)
    {
        if (dto.DestinationAddressId is not null)
        {
            var address = await _db.Addresses
                .FirstOrDefaultAsync(a => a.Id == dto.DestinationAddressId.Value)
                ?? throw new AppValidationException(new Dictionary<string, string[]>
                {
                    ["DestinationAddressId"] = new[] { "Destination address not found" }
                });


            shipment.DestinationAddressId = address.Id;
            shipment.DestinationAddress = address;
        }
        else if (dto.DestinationAddress is not null)
        {
            var newAddress = await _addressService.CreateAddressAsync(dto.DestinationAddress, "DestinationAddress");

            _db.ContactDestinationAddresses.Add(new ContactDestinationAddress
            {
                ContactId = shipment.ReceiverId,
                AddressId = newAddress.Id
            });

            shipment.DestinationAddressId = newAddress.Id;
            shipment.DestinationAddress = newAddress;
        }

        if (!string.IsNullOrWhiteSpace(dto.ServiceType))
        {
            if (!Enum.TryParse<ServiceType>(dto.ServiceType, true, out var serviceType))
                throw new AppValidationException(new Dictionary<string, string[]>
                {
                    ["ServiceType"] = new[] { "Invalid service type" }
                });

            shipment.ServiceType = serviceType;
            shipment.CalculateEstimatedDelivery();
        }

        shipment.UpdatedAt = DateTime.UtcNow;
    }
}
