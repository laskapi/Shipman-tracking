using Microsoft.EntityFrameworkCore;
using shipman.Server.Domain.Entities;

namespace shipman.Server.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public virtual DbSet<AppUser> Users { get; set; } = default!;
    public virtual DbSet<Shipment> Shipments { get; set; } = default!;
    public virtual DbSet<ShipmentEvent> ShipmentEvents { get; set; } = default!;
    public DbSet<GeocodingCache> GeocodingCache { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Shipment>(entity =>
        {
            entity.HasMany(s => s.Events)
                  .WithOne(e => e.Shipment)
                  .HasForeignKey(e => e.ShipmentId);

            entity.OwnsOne(s => s.Sender);
            entity.OwnsOne(s => s.Receiver);

            entity.OwnsOne(s => s.OriginCoordinates);
            entity.OwnsOne(s => s.DestinationCoordinates);
        });
    }
}
