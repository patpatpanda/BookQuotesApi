using System.ComponentModel.DataAnnotations;

namespace BookQuotesApi.Models;

public class Quote
{
    public int Id { get; set; }

    [Required]
    public string Text { get; set; } = string.Empty;

    [Required]
    public string Author { get; set; } = string.Empty;

    public int UserId { get; set; }
    public User? User { get; set; }     // <-- Viktigt!
}
