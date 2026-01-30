using Microsoft.EntityFrameworkCore;
using shipman.Server.Domain.Entities;

namespace shipman.Server.Data;

public interface IAppDbContext
{
    DbSet<AppUser> Users { get; }
    DbSet<Shipment> Shipments { get; }
    DbSet<ShipmentEvent> ShipmentEvents { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
