using BookQuotesApi.Data;
using BookQuotesApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BookQuotesApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();

// Use InMemory database
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("BookQuotesDb"));

// Add custom services
builder.Services.AddScoped<AuthService>();

// 🔥 CORS FIX FOR ANGULAR
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
});

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();


// 🌱 MANUELL SEED AV CITAT
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!db.Quotes.Any())
    {
        Console.WriteLine("⚠ InMemory is empty → Seeding quotes...");

        db.Quotes.AddRange(
            new Quote { Id = 1, Text = "Det är vägen, inte målet, som är mödan värd.", Author = "Karin Boye", UserId = 1 },
            new Quote { Id = 2, Text = "Att resa är att leva.", Author = "H.C. Andersen", UserId = 1 },
            new Quote { Id = 3, Text = "Den som är satt i skuld är icke fri.", Author = "Göran Persson", UserId = 1 },
            new Quote { Id = 4, Text = "Vi måste våga ta ansvar för vår framtid.", Author = "Greta Thunberg", UserId = 1 },
            new Quote { Id = 5, Text = "Allt är möjligt, det omöjliga tar bara lite längre tid.", Author = "Oscar II", UserId = 1 }
        );

        db.SaveChanges();
    }

    Console.WriteLine($"🔥 Quotes count at startup: {db.Quotes.Count()}");
}


// 🚨 MIDDLEWARE ORDER (IMPORTANT)
app.UseCors("AllowAngular");   // MUST be BEFORE Auth
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
