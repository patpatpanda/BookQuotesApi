namespace BookQuotesApi.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Author { get; set; } = default!;
    public DateTime PublishedAt { get; set; }
    public int UserId { get; set; }
}
