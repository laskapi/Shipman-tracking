using shipman.Server.Application.Services.Geocoding;
using shipman.Server.Domain.Entities;

namespace shipman.Server.Application.Interfaces;

public interface IGeocodingService
{
    Task<GeocodeResult> GeocodeAsync(Address address);
}
