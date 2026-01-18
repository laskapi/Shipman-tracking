namespace shipman.Server.Domain.Entities;

public class AppUser
{
    public Guid Id { get; set; }
    public string Email { get; set; } = default!;
    public byte[] PasswordHash { get; set; } = default!;
    public byte[] PasswordSalt { get; set; } = default!;
}
