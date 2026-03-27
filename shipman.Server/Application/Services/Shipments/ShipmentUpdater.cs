using Microsoft.EntityFrameworkCore;
using shipman.Server.Application.Dtos.Shipments;
using shipman.Server.Application.Exceptions;
using shipman.Server.Data;
using shipman.Server.Domain.Entities;
using shipman.Server.Domain.Enums;

public class ShipmentUpdater
{
    private readonly IAppDbContext _db;

    public ShipmentUpdater(IAppDbContext db)
    {
        _db = db;
    }

    public async Task UpdateAsync(Shipment shipment, ShipmentUpdateDto dto)
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
