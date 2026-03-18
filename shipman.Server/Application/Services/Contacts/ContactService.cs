using Microsoft.EntityFrameworkCore;
using shipman.Server.Application.Dtos.Contacts;
using shipman.Server.Application.Dtos.Shipments;
using shipman.Server.Application.Exceptions;
using shipman.Server.Application.Services.Addresses;
using shipman.Server.Data;
using shipman.Server.Domain.Entities;

namespace shipman.Server.Application.Services.Contacts;

public class ContactService
{
    private readonly IAppDbContext _db;
    private readonly AddressService _addressService;

    public ContactService(IAppDbContext db, AddressService addressService)
    {
        _db = db;
        _addressService = addressService;
    }

    public async Task<Contact> CreateAsync(CreateContactDto dto)
    {
        var primary = await _addressService.CreateAddressAsync(dto.PrimaryAddress, "PrimaryAddress");

        var contact = new Contact
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone,
            PrimaryAddressId = primary.Id,
            PrimaryAddress = primary
        };

        _db.Contacts.Add(contact);
        await _db.SaveChangesAsync();

        return contact;
    }

    public async Task<Contact> GetByIdAsync(Guid id)
    {
        return await _db.Contacts
            .Include(c => c.PrimaryAddress)
            .Include(c => c.DestinationAddresses).ThenInclude(x => x.Address)
            .FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new AppNotFoundException("Contact not found");
    }

    public async Task<List<Contact>> SearchAsync(string? search)
    {
        return await _db.Contacts
            .Where(c =>
                search == null ||
                c.Name.Contains(search) ||
                (c.Email != null && c.Email.Contains(search)) ||
                (c.Phone != null && c.Phone.Contains(search)))
            .OrderBy(c => c.Name)
            .Take(20)
            .ToListAsync();
    }

    public async Task<Contact> AddDestinationAddressAsync(Guid contactId, AddressDto dto)
    {
        var contact = await GetByIdAsync(contactId);

        var address = await _addressService.CreateAddressAsync(dto, "Address");

        _db.ContactDestinationAddresses.Add(new ContactDestinationAddress
        {
            ContactId = contactId,
            AddressId = address.Id
        });

        await _db.SaveChangesAsync();

        return contact;
    }

    public async Task<Contact> UpdateAsync(Guid id, UpdateContactDto dto)
    {
        var contact = await GetByIdAsync(id);

        if (dto.Name != null) contact.Name = dto.Name;
        if (dto.Email != null) contact.Email = dto.Email;
        if (dto.Phone != null) contact.Phone = dto.Phone;

        if (dto.PrimaryAddress != null)
        {
            var newAddress = await _addressService.CreateAddressAsync(dto.PrimaryAddress, "PrimaryAddress");
            contact.PrimaryAddressId = newAddress.Id;
            contact.PrimaryAddress = newAddress;
        }

        await _db.SaveChangesAsync();
        return contact;
    }

    public async Task DeleteAsync(Guid id)
    {
        var contact = await GetByIdAsync(id);
        _db.Contacts.Remove(contact);
        await _db.SaveChangesAsync();
    }
}
