namespace shipman.Server.Domain.Entities;

public class Address
{
    public Guid Id { get; set; }

    public required string Street { get; set; } = default!;
    public required string HouseNumber { get; set; } = default!;
    public string? ApartmentNumber { get; set; }

    public required string City { get; set; } = default!;
    public required string PostalCode { get; set; } = default!;
    public required string Country { get; set; } = default!;

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public ICollection<ContactDestinationAddress> ContactLinks { get; set; }
        = new List<ContactDestinationAddress>();
}
