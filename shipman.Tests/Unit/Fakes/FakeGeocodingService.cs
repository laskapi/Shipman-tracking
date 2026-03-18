using shipman.Server.Application.Interfaces;
using shipman.Server.Application.Services.Geocoding;
using shipman.Server.Domain.Entities;

namespace shipman.Tests.Unit.Fakes;

public class FakeGeocodingService : IGeocodingService
{
    public Task<GeocodeResult> GeocodeAsync(Address address)
    {
        return Task.FromResult(new GeocodeResult(
            Lat: 50.0,
            Lng: 20.0,
            FormattedAddress: $"{address.Street}, {address.City}"
        ));
    }
}
