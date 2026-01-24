using Microsoft.EntityFrameworkCore;
using shipman.Server.Domain.Entities;

namespace shipman.Server.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<AppUser> Users => Set<AppUser>();
//    public DbSet<Shipment> Shipments => Set<Shipment>();
    public DbSet<Shipment> Shipments { get; set; } = default!;
    public DbSet<ShipmentEvent> ShipmentEvents { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Shipment>()
            .HasMany(s => s.Events)
            .WithOne(e => e.Shipment)
            .HasForeignKey(e => e.ShipmentId);
    }


}
