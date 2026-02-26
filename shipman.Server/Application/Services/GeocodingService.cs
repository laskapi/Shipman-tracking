using Microsoft.EntityFrameworkCore;
using shipman.Server.Application.Exceptions;
using shipman.Server.Data;
using shipman.Server.Domain.Entities;

namespace shipman.Server.Application.Services;

public class GeocodingService
{
    private readonly HttpClient _http;
    private readonly IAppDbContext _db;
    public GeocodingService(HttpClient http, IAppDbContext db)
    {
        _http = http;
        _db = db;
        _http.DefaultRequestHeaders.UserAgent.ParseAdd("YourAppName/1.0");
    }

    public async Task<(double lat, double lng)> GeocodeAsync(string address)
    {
        var normalized = address.Trim().ToLowerInvariant();
        var cached = await _db.GeocodingCache
            .FirstOrDefaultAsync(x => x.Address == normalized);
        if (cached != null && cached.CachedAt > DateTime.UtcNow.AddDays(-60))
            return (cached.Lat, cached.Lng);




        var url = $"https://nominatim.openstreetmap.org/search?format=json&q={Uri.EscapeDataString(address)}";

        var response = await _http.GetFromJsonAsync<List<NominatimResult>>(url);

        if (response == null || response.Count == 0)
        {
            throw new AppValidationException(new Dictionary<string, string[]>
            {
                ["Address"] = new[] { "Address not found" }
            });

        }

        var result = response[0];

        if (!double.TryParse(result.Lat, out var lat))
            throw new AppDomainException("Invalid latitude from geocoding API");


        if (!double.TryParse(result.Lon, out var lng))
            throw new AppDomainException("Invalid longitude from geocoding API");
        _db.GeocodingCache.Add(new GeocodingCache
        {
            Address = normalized,
            Lat = lat,
            Lng = lng,
            CachedAt = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
        return (lat, lng);
    }
}

public class NominatimResult
{
    public string? Lat { get; set; }
    public string? Lon { get; set; }
}
