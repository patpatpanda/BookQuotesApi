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

    [HttpGet]
    public IActionResult GetQuotes()
    {
        // Visa alla citat
        return Ok(_db.Quotes.ToList());
    }


    [HttpPost]
    public IActionResult CreateQuote([FromBody] Quote quote)
    {
        quote.UserId = GetUserId();
        _db.Quotes.Add(quote);
        _db.SaveChanges();
        return Ok(quote);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateQuote(int id, [FromBody] Quote updated)
    {
        var quote = _db.Quotes.Find(id);
        if (quote == null) return NotFound();
        if (quote.UserId != GetUserId()) return Unauthorized();

        quote.Text = updated.Text;
        quote.Author = updated.Author;
        _db.SaveChanges();

        return Ok(quote);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteQuote(int id)
    {
        var quote = _db.Quotes.Find(id);
        if (quote == null) return NotFound();
      

        _db.Quotes.Remove(quote);
        _db.SaveChanges();

        return Ok();
    }

    private int GetUserId()
        => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
