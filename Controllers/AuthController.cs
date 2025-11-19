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
    private readonly AuthService _auth;

    public AuthController(AppDbContext db, AuthService auth)
    {
        _db = db;
        _auth = auth;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (await _db.Users.AnyAsync(u => u.Username == dto.Username))
            return BadRequest("User already exists");

        _auth.CreatePasswordHash(dto.Password, out var hash, out var salt);

        var user = new User
        {
            Username = dto.Username,
            PasswordHash = hash,
            PasswordSalt = salt
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        // ✨ Kopiera template-citat till ny användare
        var templates = await _db.TemplateQuotes.ToListAsync();

        foreach (var t in templates)
        {
            _db.Quotes.Add(new Quote
            {
                Text = t.Text,
                Author = t.Author,
                UserId = user.Id
            });
        }

        await _db.SaveChangesAsync();

        return Ok("User registered.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _db.Users.SingleOrDefaultAsync(u => u.Username == dto.Username);
        if (user == null) return Unauthorized("Invalid username or password");

        if (!_auth.VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt))
            return Unauthorized("Invalid username or password");

        var token = _auth.CreateToken(user);
        return Ok(new { token });
    }
}
