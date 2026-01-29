using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using shipman.Server.Data;

namespace shipman.Tests.Integration.Factories;

public class TestApplicationFactory : WebApplicationFactory<Program>
{
    private SqliteConnection? _connection;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Ensures the test host loads your real Program.cs pipeline
        builder.UseSetting(WebHostDefaults.ApplicationKey, typeof(Program).Assembly.FullName);

        builder.ConfigureServices(services =>
        {
            // Remove the real database registration
            var descriptor = services.Single(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            services.Remove(descriptor);

            // Create shared in-memory SQLite connection
            _connection = new SqliteConnection("DataSource=:memory:;Cache=Shared");
            _connection.Open();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(_connection));

            // Build schema
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _connection?.Dispose();
    }
}
