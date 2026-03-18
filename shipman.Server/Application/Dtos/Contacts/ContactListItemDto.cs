namespace shipman.Server.Application.Dtos.Contacts;

public class ContactListItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Email { get; set; }
    public string? Phone { get; set; }
}
