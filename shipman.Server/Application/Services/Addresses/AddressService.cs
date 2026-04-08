using Microsoft.EntityFrameworkCore;
using shipman.Server.Application.Dtos.Addresses;
using shipman.Server.Application.Exceptions;
using shipman.Server.Application.Interfaces;
using shipman.Server.Application.Services.Shipments;
using shipman.Server.Data;
using shipman.Server.Domain.Entities;
using System.Linq;
namespace shipman.Server.Application.Services.Addresses;

public class AddressService
{
    private readonly IAppDbContext _db;
    private readonly IGeocodingService _geocoding;
    private readonly ShipmentQueries _shipmentQueries;


    public AddressService(IAppDbContext db, IGeocodingService geocoding, ShipmentQueries shipmentQueries)
    {
        _db = db;
        _geocoding = geocoding;
        _shipmentQueries = shipmentQueries;
    }

    public async Task<Address> CreateAddressAsync(CreateAddressDto dto, string fieldName)
    {
        var address = new Address
        {
            Id = Guid.NewGuid(),
            Street = dto.Street,
            HouseNumber = dto.HouseNumber,
            ApartmentNumber = dto.ApartmentNumber,
            City = dto.City,
            PostalCode = dto.PostalCode,
            Country = dto.Country
        };

        try
        {
            var geo = await _geocoding.GeocodeAsync(address);
            address.Latitude = geo.Lat;
            address.Longitude = geo.Lng;
        }
        catch (AppValidationException ex)
        {
            var message = ex.Errors.Values.SelectMany(a => a).FirstOrDefault() ?? "Address not found";
            throw new AppValidationException(new Dictionary<string, string[]>
            {
                [fieldName] = new[] { message }
            });
        }
        catch
        {
            throw new AppValidationException(new Dictionary<string, string[]>
            {
                [fieldName] = new[]
                {
                    "Could not verify this address (geocoding failed). Check the address or try again later."
                }
            });
        }

        _db.Addresses.Add(address);
        return address;
    }
    public async Task DeleteAddressAsync(Guid addressId)
    {
        var address = await _db.Addresses
            .Include(a => a.ContactLinks)
            .FirstOrDefaultAsync(a => a.Id == addressId);

        if (address == null)
            throw new AppNotFoundException("Address not found");

        if (await _shipmentQueries.IsAddressUsedInShipmentsAsync(addressId))
            throw new AppInvalidOperationException(
                "Cannot delete this address because it is used in shipments");

        var isPrimaryForAnyContact = await _db.Contacts
            .AnyAsync(c => c.PrimaryAddressId == addressId);

        if (isPrimaryForAnyContact)
            throw new AppInvalidOperationException(
                "Cannot delete this address because it is the primary address of a contact");

        _db.ContactDestinationAddresses.RemoveRange(address.ContactLinks);
        _db.Addresses.Remove(address);

        await _db.SaveChangesAsync();
    }

}
