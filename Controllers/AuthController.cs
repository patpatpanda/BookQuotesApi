using BookQuotesApi.Data;
using BookQuotesApi.Dtos;
using BookQuotesApi.Models;
using BookQuotesApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookQuotesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly AuthService _authService;

    public AuthController(AppDbContext db, AuthService authService)
    {
        _db = db;
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        // Kontrollera om användarnamn redan finns i DB
        if (await _db.Users.AnyAsync(u => u.Username == dto.Username))
            return BadRequest("User already exists");

        // Skapa hashar
        _authService.CreatePasswordHash(dto.Password, out var hash, out var salt);

        // Spara ny användare
        var user = new User
        {
            Username = dto.Username,
            PasswordHash = hash,
            PasswordSalt = salt
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return Ok("User registered successfully");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        // Hitta användare
        var user = await _db.Users.SingleOrDefaultAsync(u => u.Username == dto.Username);
        if (user == null || !_authService.VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt))
            return Unauthorized("Invalid username or password");

        // Skapa JWT-token
        var token = _authService.CreateToken(user);
        return Ok(new { token });
    }
}
