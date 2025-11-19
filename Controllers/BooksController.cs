using BookQuotesApi.Data;
using BookQuotesApi.Dtos;
using BookQuotesApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookQuotesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BooksController : ControllerBase
{
    private readonly AppDbContext _db;

    public BooksController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult GetBooks()
    {
        int userId = GetUserId();
        return Ok(_db.Books.Where(b => b.UserId == userId).ToList());
    }

    [HttpPost]
    public IActionResult CreateBook([FromBody] CreateBookDto dto)
    {
        var book = new Book
        {
            Title = dto.Title,
            Author = dto.Author,
            PublishedAt = dto.PublishedAt,
            UserId = GetUserId()
        };

        _db.Books.Add(book);
        _db.SaveChanges();

        return Ok(book);
    }


    [HttpPut("{id}")]
    public IActionResult UpdateBook(int id, [FromBody] UpdateBookDto updated)
    {
        var book = _db.Books.Find(id);
        if (book == null) return NotFound();
        if (book.UserId != GetUserId()) return Unauthorized();

        book.Title = updated.Title;
        book.Author = updated.Author;
        book.PublishedAt = updated.PublishedAt;

        _db.SaveChanges();

        return Ok(book);
    }

    [HttpGet("{id}")]
    public IActionResult GetBook(int id)
    {
        var userId = GetUserId();

        var book = _db.Books.FirstOrDefault(b => b.Id == id && b.UserId == userId);

        if (book == null)
            return NotFound();

        return Ok(book);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBook(int id)
    {
        var book = _db.Books.Find(id);
        if (book == null) return NotFound();
        if (book.UserId != GetUserId()) return Unauthorized();

        _db.Books.Remove(book);
        _db.SaveChanges();

        return Ok();
    }

    private int GetUserId()
        => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
