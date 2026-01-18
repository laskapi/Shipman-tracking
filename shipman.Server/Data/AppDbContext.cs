using Microsoft.EntityFrameworkCore;
using shipman.Server.Domain.Entities;

namespace shipman.Server.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Shipment> Shipments => Set<Shipment>();

}
