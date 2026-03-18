using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using shipman.Server.Data;
using shipman.Server.Domain.Entities;

namespace shipman.Tests.Integration.Factories;

public class TestApplicationFactory : WebApplicationFactory<Program>
{
    private SqliteConnection? _connection;

    public Guid SenderId { get; private set; }
    public Guid ReceiverId { get; private set; }
    public Guid DestinationAddressId { get; private set; }
    public ShipmentDtoFactory Dtos { get; private set; } = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting(WebHostDefaults.ApplicationKey, typeof(Program).Assembly.FullName);

        builder.ConfigureServices(services =>
        {
            services.AddSingleton<IStartupFilter, TestExceptionLoggingStartupFilter>();
        });


        builder.ConfigureServices(services =>
        {
            var descriptor = services.Single(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            services.Remove(descriptor);

            _connection = new SqliteConnection("DataSource=:memory:;Cache=Shared");
            _connection.Open();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(_connection));

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();

            SeedContactsAndAddress(db);
            Dtos = new ShipmentDtoFactory(
                SenderId,
                ReceiverId,
                DestinationAddressId
            );
        });
    }

    private void SeedContactsAndAddress(AppDbContext db)
    {
        var address = new Address
        {
            Id = Guid.NewGuid(),
            Street = "Test Street",
            HouseNumber = "10A",
            City = "Testville",
            PostalCode = "00-000",
            Country = "Testland"
        };

        var sender = new Contact
        {
            Id = Guid.NewGuid(),
            Name = "Test Sender",
            Email = "sender@test.com",
            Phone = "123456",
            PrimaryAddress = address
        };

        var receiver = new Contact
        {
            Id = Guid.NewGuid(),
            Name = "Test Receiver",
            Email = "receiver@test.com",
            Phone = "654321",
            PrimaryAddress = address
        };

        db.Addresses.Add(address);
        db.Contacts.AddRange(sender, receiver);
        db.SaveChanges();

        SenderId = sender.Id;
        ReceiverId = receiver.Id;
        DestinationAddressId = address.Id;
    }


    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _connection?.Dispose();
    }
}
