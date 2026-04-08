namespace shipman.Server.Application.Dtos.Shipments;

public record ContactDto
{
    public ContactDto() { }

    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string Phone { get; init; } = default!;
    public AddressDto Address { get; init; } = default!;
}
