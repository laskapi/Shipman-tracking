namespace shipman.Server.Domain.Entities;

public class GeocodingCache
{
    public int Id { get; set; }
    public string AddressKey { get; set; } = default!;
    public string FormattedAddress { get; set; } = default!;
    public double Lat { get; set; }
    public double Lng { get; set; }
    public DateTime CachedAt { get; set; }
}
