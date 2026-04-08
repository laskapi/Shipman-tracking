using Microsoft.EntityFrameworkCore;
using shipman.Server.Application.Dtos.Addresses;
using shipman.Server.Application.Dtos.Contacts;
using shipman.Server.Application.Exceptions;
using shipman.Server.Application.Services.Addresses;
using shipman.Server.Application.Services.Shipments;
using shipman.Server.Data;
using shipman.Server.Domain.Entities;

namespace shipman.Server.Application.Services.Contacts;

public class ContactService
{
    private readonly IAppDbContext _db;
    private readonly AddressService _addressService;
    private readonly ShipmentQueries _shipmentQueries;


    public ContactService(IAppDbContext db, AddressService addressService, ShipmentQueries shipmentQueries)
    {
        _db = db;
        _addressService = addressService;
        _shipmentQueries = shipmentQueries;
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

    public async Task<Contact> AddDestinationAddressAsync(Guid contactId, CreateAddressDto dto)
    {
        var contact = await GetByIdAsync(contactId);

        var address = await _addressService.CreateAddressAsync(dto, "Address");

        _db.ContactDestinationAddresses.Add(new ContactDestinationAddress
        {
            ContactId = contactId,
            AddressId = address.Id
        });

        await _db.SaveChangesAsync();

        return await GetByIdAsync(contactId);
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

    public async Task DeleteContactAsync(Guid contactId)
    {
        var contact = await _db.Contacts
            .Include(c => c.DestinationAddresses)
            .FirstOrDefaultAsync(c => c.Id == contactId);

        if (contact == null)
            throw new AppNotFoundException("Contact not found");

        if (await _shipmentQueries.IsContactUsedInShipmentsAsync(contactId))
            throw new AppInvalidOperationException(
                "Cannot delete this contact because it is used in shipments");

        _db.ContactDestinationAddresses.RemoveRange(contact.DestinationAddresses);
        _db.Contacts.Remove(contact);

        await _db.SaveChangesAsync();
    }


    public async Task DetachAddressAsync(Guid contactId, Guid addressId)
    {
        var contact = await _db.Contacts
            .Include(c => c.DestinationAddresses)
            .FirstOrDefaultAsync(c => c.Id == contactId);

        if (contact == null)
            throw new AppNotFoundException("Contact not found");

        var link = contact.DestinationAddresses
            .FirstOrDefault(da => da.AddressId == addressId);

        if (link == null)
            throw new AppNotFoundException("This address is not linked to the contact");

        var isUsed = await _shipmentQueries
            .IsContactAddressPairUsedAsync(contactId, addressId);

        if (isUsed)
            throw new AppInvalidOperationException(
                "Cannot detach this address because it is used in shipments with this contact");

        _db.ContactDestinationAddresses.Remove(link);
        await _db.SaveChangesAsync();
    }


}
