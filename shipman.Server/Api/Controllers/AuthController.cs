using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using shipman.Server.Domain.Entities;
using shipman.Server.Data;
using shipman.Server.Application.Dtos;

namespace shipman.Server.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly AppDbContext _db;

    public AuthController(
        ILogger<AuthController> logger,
        AppDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        _logger.LogInformation("Registration attempt for email {Email}", dto.Email);

        if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
        {
            _logger.LogWarning("Registration failed: user with email {Email} already exists", dto.Email);
            return BadRequest("User already exists");
        }

        CreatePasswordHash(dto.Password, out var hash, out var salt);

        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            PasswordHash = hash,
            PasswordSalt = salt
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        _logger.LogInformation("User {Email} registered successfully", dto.Email);

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        _logger.LogInformation("Login attempt for email {Email}", dto.Email);

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null || !VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt))
        {
            _logger.LogWarning("Login failed for email {Email}", dto.Email);
            return Unauthorized("Invalid credentials");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        _logger.LogInformation("User {Email} logged in successfully", dto.Email);

        return Ok(new { email = user.Email });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        _logger.LogInformation("User {UserId} logged out", userId);

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return Ok();
    }

    [HttpGet("me")]
    public IActionResult Me()
    {
        _logger.LogInformation("Accessing /me endpoint");

        if (!User.Identity?.IsAuthenticated ?? true)
        {
            _logger.LogWarning("Unauthorized /me access attempt");
            return Unauthorized();
        }

        return Ok(new
        {
            id = User.FindFirstValue(ClaimTypes.NameIdentifier),
            email = User.FindFirstValue(ClaimTypes.Email)
        });
    }

    // Helpers
    private void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
    {
        using var hmac = new HMACSHA512();
        salt = hmac.Key;
        hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private bool VerifyPassword(string password, byte[] hash, byte[] salt)
    {
        using var hmac = new HMACSHA512(salt);
        var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computed.SequenceEqual(hash);
    }
}
