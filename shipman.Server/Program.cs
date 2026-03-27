using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using shipman.Server.Api.Middleware;
using shipman.Server.Application.Interfaces;
using shipman.Server.Application.Services;
using shipman.Server.Application.Services.Addresses;
using shipman.Server.Application.Services.Geocoding;
using shipman.Server.Application.Services.Shipments;
using shipman.Server.Data;
using shipman.Server.Infrastructure.Mail;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

/*builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));
*/
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IShipmentService, ShipmentService>();
builder.Services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());
builder.Services.AddScoped<FakeMailSender>();
builder.Services.AddScoped<IMailSender>(sp =>
{
    var logger = sp.GetRequiredService<ILogger<MailSenderLoggingDecorator>>();
    var inner = sp.GetRequiredService<FakeMailSender>();
    return new MailSenderLoggingDecorator(logger, inner);
});
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ShipmentFactory>();
builder.Services.AddScoped<ShipmentUpdater>();
builder.Services.AddScoped<AddressService>();
builder.Services.AddScoped<ShipmentQueries>();

builder.Services.AddScoped<IGeocodingService, GeocodingService>();
builder.Services.AddHttpClient<GeocodingService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.Cookie.Name = "auth_cookie";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
    // dev only options.LoginPath = "/api/auth/unauthorized";
});
builder.Services.AddAuthorization();

// CORS for React
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var loggerFactory = NullLoggerFactory.Instance;
builder.Services.AddSingleton<IMapper>(sp =>
{
    var config = new MapperConfiguration(cfg =>
    {
        cfg.AddMaps(typeof(Program).Assembly);
    }, loggerFactory);

    return config.CreateMapper();
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbSeeder.Seed(db);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1");
    });
}
app.UseAppExceptionHandling();
app.UseCors();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseHttpsRedirection();
app.MapFallbackToFile("/index.html");
app.Run();
