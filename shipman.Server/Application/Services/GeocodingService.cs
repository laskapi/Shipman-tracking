using shipman.Server.Application.Exceptions;

namespace shipman.Server.Application.Services;

public class GeocodingService
{
    private readonly HttpClient _http;

    public GeocodingService(HttpClient http)
    {
        _http = http;
        _http.DefaultRequestHeaders.UserAgent.ParseAdd("YourAppName/1.0");
    }

    public async Task<(double lat, double lng)> GeocodeAsync(string address)
    {
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


        return (lat, lng);
    }
}

public class NominatimResult
{
    public string? Lat { get; set; }
    public string? Lon { get; set; }
}
