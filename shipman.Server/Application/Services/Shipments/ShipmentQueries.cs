using Microsoft.EntityFrameworkCore;
using shipman.Server.Data;

namespace shipman.Server.Application.Services.Shipments;

public class ShipmentQueries(AppDbContext context)
{
    private readonly AppDbContext _context = context;
    /// <summary>
    /// Checks if ANY shipment uses this specific contact.
    /// </summary>

    public async Task<bool> IsContactUsedInShipmentsAsync(Guid contactId, CancellationToken cancellationToken = default)
    {
        return await _context.Shipments
            .AnyAsync(s =>
                s.SenderId == contactId ||
                s.ReceiverId == contactId,
                cancellationToken);
    }

    /// <summary>
    /// Checks if ANY shipment uses this specific contact + address pair.
    /// </summary>
    public async Task<bool> IsContactAddressPairUsedAsync(Guid contactId, Guid addressId, CancellationToken cancellationToken = default)
    {
        return await _context.Shipments
            .AnyAsync(s =>
                (s.SenderId == contactId || s.ReceiverId == contactId)
                && s.DestinationAddressId == addressId,
                cancellationToken);
    }

    /// <summary>
    /// Checks if ANY shipment uses this address as its destination.
    /// </summary>
    public async Task<bool> IsAddressUsedInShipmentsAsync(Guid addressId, CancellationToken cancellationToken = default)
    {
        return await _context.Shipments
            .AnyAsync(s => s.DestinationAddressId == addressId, cancellationToken);
    }
}
