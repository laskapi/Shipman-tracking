using Microsoft.EntityFrameworkCore;
using shipman.Server.Domain.Entities;

namespace shipman.Server.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public virtual DbSet<AppUser> Users { get; set; } = default!;
    public virtual DbSet<Shipment> Shipments { get; set; } = default!;
    public virtual DbSet<ShipmentEvent> ShipmentEvents { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Shipment>(entity =>
        {
            entity.HasMany(s => s.Events)
                  .WithOne(e => e.Shipment)
                  .HasForeignKey(e => e.ShipmentId);

            entity.OwnsOne(s => s.Receiver, r =>
            {
                r.Property(x => x.Name)
                    .HasColumnName("ReceiverName")
                    .IsRequired();

                r.Property(x => x.Email)
                    .HasColumnName("ReceiverEmail")
                    .IsRequired();

                r.Property(x => x.Phone)
                    .HasColumnName("ReceiverPhone")
                    .IsRequired();
            });
        });

    }

}



