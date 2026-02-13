namespace shipman.Server.Application.Dtos.Shipments;

public record ContactDto
{
    public string Name { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string Phone { get; init; } = default!;
}
