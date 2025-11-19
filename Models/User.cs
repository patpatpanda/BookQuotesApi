namespace BookQuotesApi.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();

    public List<Book> Books { get; set; } = new();
    public List<Quote> Quotes { get; set; } = new();
}
