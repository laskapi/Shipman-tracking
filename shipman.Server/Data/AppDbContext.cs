using Microsoft.EntityFrameworkCore;
using shipman.Server.Domain.Entities;

namespace shipman.Server.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<AppUser> Users { get; set; } = default!;
    public DbSet<Shipment> Shipments { get; set; } = default!;
    public DbSet<ShipmentEvent> ShipmentEvents { get; set; } = default!;
    public DbSet<GeocodingCache> GeocodingCache { get; set; } = default!;
    public DbSet<Contact> Contacts { get; set; } = default!;
    public DbSet<Address> Addresses { get; set; } = default!;
    public DbSet<ContactDestinationAddress> ContactDestinationAddresses { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.ToTable("Contacts");
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name).IsRequired();

            entity.HasOne(c => c.PrimaryAddress)
                  .WithMany()
                  .HasForeignKey(c => c.PrimaryAddressId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(c => c.DestinationAddresses)
                  .WithOne(x => x.Contact)
                  .HasForeignKey(x => x.ContactId);
        });

        modelBuilder.Entity<ContactDestinationAddress>(entity =>
        {
            entity.ToTable("ContactDestinationAddresses");

            entity.HasKey(x => new { x.ContactId, x.AddressId });

            entity.HasOne(x => x.Contact)
                  .WithMany(c => c.DestinationAddresses)
                  .HasForeignKey(x => x.ContactId);

            entity.HasOne(x => x.Address)
                  .WithMany(a => a.ContactLinks)
                  .HasForeignKey(x => x.AddressId);
        });

        modelBuilder.Entity<Shipment>(entity =>
        {
            entity.ToTable("Shipments");
            entity.HasKey(s => s.Id);

            entity.HasMany(s => s.Events)
                  .WithOne(e => e.Shipment)
                  .HasForeignKey(e => e.ShipmentId);

            entity.HasOne(s => s.Sender)
                  .WithMany()
                  .HasForeignKey(s => s.SenderId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.Receiver)
                  .WithMany()
                  .HasForeignKey(s => s.ReceiverId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.DestinationAddress)
                  .WithMany()
                  .HasForeignKey(s => s.DestinationAddressId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.ToTable("Addresses");
            entity.HasKey(a => a.Id);

            entity.Property(a => a.Street).IsRequired();
            entity.Property(a => a.HouseNumber).IsRequired();
            entity.Property(a => a.ApartmentNumber);
            entity.Property(a => a.City).IsRequired();
            entity.Property(a => a.PostalCode).IsRequired();
            entity.Property(a => a.Country).IsRequired();

            entity.Property(a => a.Latitude);
            entity.Property(a => a.Longitude);
        });
    }
}
