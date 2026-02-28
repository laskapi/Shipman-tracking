namespace shipman.Server.Application.Services.Geocoding;

public record GeocodeResult(double Lat, double Lng, string FormattedAddress);
