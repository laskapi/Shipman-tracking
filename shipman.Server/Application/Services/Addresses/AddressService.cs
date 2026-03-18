using shipman.Server.Application.Dtos.Shipments;
using shipman.Server.Application.Exceptions;
using shipman.Server.Application.Interfaces;
using shipman.Server.Data;
using shipman.Server.Domain.Entities;
namespace shipman.Server.Application.Services.Addresses;

public class AddressService
{
    private readonly IAppDbContext _db;
    private readonly IGeocodingService _geocoding;

    public AddressService(IAppDbContext db, IGeocodingService geocoding)
    {
        _db = db;
        _geocoding = geocoding;
    }

    public async Task<Address> CreateAddressAsync(AddressDto dto, string fieldName)
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
        catch
        {
            throw new AppValidationException(new Dictionary<string, string[]>
            {
                [fieldName] = new[] { "Address not found" }
            });
        }

        _db.Addresses.Add(address);
        return address;
    }
}
