using BookQuotesApi.Data;
using BookQuotesApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookQuotesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class QuotesController : ControllerBase
{
    private readonly AppDbContext _db;

    public QuotesController(AppDbContext db)
    {
        _db = db;
    }

    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public IActionResult GetQuotes()
        => Ok(_db.Quotes.Where(q => q.UserId == UserId).ToList());

    [HttpPost]
    public IActionResult CreateQuote([FromBody] Quote quote)
    {
        quote.UserId = UserId;
        _db.Quotes.Add(quote);
        _db.SaveChanges();
        return Ok(quote);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateQuote(int id, [FromBody] Quote updated)
    {
        var quote = _db.Quotes.Find(id);
        if (quote == null || quote.UserId != UserId) return Unauthorized();

        quote.Text = updated.Text;
        quote.Author = updated.Author;

        _db.SaveChanges();
        return Ok(quote);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteQuote(int id)
    {
        var quote = _db.Quotes.Find(id);
        if (quote == null || quote.UserId != UserId) return Unauthorized();

        _db.Quotes.Remove(quote);
        _db.SaveChanges();
        return Ok();
    }
}
