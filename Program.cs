using Microsoft.EntityFrameworkCore;
using JokesApp.Data;
using JokesApp.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

if (!db.Jokes.Any())
{
    db.Jokes.AddRange(
        new Joke { Id = Guid.NewGuid().ToString(), Question = "Why did the chicken cross the road?", JokeAnswer = "road" },
        new Joke { Id = Guid.NewGuid().ToString(), Question = "What do you call fake spaghetti?", JokeAnswer = "impasta" },
        new Joke { Id = Guid.NewGuid().ToString(), Question = "Why don't scientists trust atoms?", JokeAnswer = "everything" }
    );
    db.SaveChanges();
}

app.Run();


