using Microsoft.EntityFrameworkCore;
using BookQuotesApi.Models;

namespace BookQuotesApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Quote> Quotes => Set<Quote>();
    public DbSet<TemplateQuote> TemplateQuotes => Set<TemplateQuote>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TemplateQuote>().HasData(
            new TemplateQuote { Id = 1, Text = "Livet är en jobbig period.", Author = "Per Morberg" },
            new TemplateQuote { Id = 2, Text = "Det gäller att ha tungan rätt i mun.", Author = "Emil Arrenius" },
            new TemplateQuote { Id = 3, Text = "Den som är satt i skuld är icke fri.", Author = "Göran Persson" },
            new TemplateQuote { Id = 4, Text = "Som man bäddar får man ligga.", Author = "Krister Pettersson" },
            new TemplateQuote { Id = 5, Text = "Allt är möjligt, det omöjliga tar bara lite längre tid.", Author = "Oscar II" }
        );
    }
}
