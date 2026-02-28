using Microsoft.EntityFrameworkCore;
using shipman.Server.Application.Exceptions;
using shipman.Server.Data;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace shipman.Server.Application.Services.Geocoding;

public class GeocodingService
{
    private readonly HttpClient _http;
    private readonly IAppDbContext _db;

    public GeocodingService(HttpClient http, IAppDbContext db)
    {
        _http = http;
        _db = db;
        _http.DefaultRequestHeaders.UserAgent.ParseAdd("Shipman/1.0");
    }

    private string NormalizeForCache(string input)
    {
        var s = input.Trim().ToLowerInvariant();
        s = Regex.Replace(s, @"[^\p{L}\p{N}\s]", " ");
        s = Regex.Replace(s, @"\s+", " ");
        s = RemoveDiacritics(s);

        var tokens = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        Array.Sort(tokens);

        return string.Join(" ", tokens);
    }

    private string RemoveDiacritics(string text)
    {
        var normalized = text.Normalize(NormalizationForm.FormD);
        var chars = normalized.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark);
        return new string(chars.ToArray()).Normalize(NormalizationForm.FormC);
    }

    public async Task<GeocodeResult> GeocodeAsync(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new AppValidationException(new Dictionary<string, string[]>
            {
                ["Address"] = new[] { "Address is required" }
            });

        var key = NormalizeForCache(address);

        var cached = await _db.GeocodingCache
            .FirstOrDefaultAsync(x => x.AddressKey == key);

        if (cached != null &&
            cached.CachedAt > DateTime.UtcNow.AddDays(-60) &&
            cached.Lat is >= -90 and <= 90 &&
            cached.Lng is >= -180 and <= 180)
        {
            return new GeocodeResult(cached.Lat, cached.Lng, cached.FormattedAddress);
        }

        var url = $"https://nominatim.openstreetmap.org/search?format=json&q={Uri.EscapeDataString(address)}";
        var response = await _http.GetFromJsonAsync<List<NominatimResult>>(url);

        if (response == null || response.Count == 0)
            throw new AppValidationException(new Dictionary<string, string[]>
            {
                ["Address"] = new[] { "Address not found" }
            });

        var result = response[0];

        if (!double.TryParse(result.Lat, NumberStyles.Any, CultureInfo.InvariantCulture, out var lat))
            throw new AppDomainException("Invalid latitude from geocoding API");

        if (!double.TryParse(result.Lon, NumberStyles.Any, CultureInfo.InvariantCulture, out var lng))
            throw new AppDomainException("Invalid longitude from geocoding API");

        var formatted = result.Display_Name?.Trim() ?? address.Trim();

        if (cached == null)
        {
            _db.GeocodingCache.Add(new GeocodingCache
            {
                AddressKey = key,
                FormattedAddress = formatted,
                Lat = lat,
                Lng = lng,
                CachedAt = DateTime.UtcNow
            });
        }
        else
        {
            cached.FormattedAddress = formatted;
            cached.Lat = lat;
            cached.Lng = lng;
            cached.CachedAt = DateTime.UtcNow;
        }

        await _db.SaveChangesAsync();
        return new GeocodeResult(lat, lng, formatted);
    }
}

public class NominatimResult
{
    public string? Lat { get; set; }
    public string? Lon { get; set; }
    public string? Display_Name { get; set; }
}
