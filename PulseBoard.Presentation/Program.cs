using Microsoft.EntityFrameworkCore;
using PulseBoard.Application.Interfaces;
using PulseBoard.Services.Data;

var builder = WebApplication.CreateBuilder(args);

// ------------------------
// Add Controllers
// ------------------------
builder.Services.AddControllers();

// ------------------------
// Swagger
// ------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ------------------------
// Cors Policy
// ------------------------

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ------------------------
// DbContext (SQLite)
// ------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=pulseboard.db"));

// ------------------------
// Dependency Injection
// ------------------------
builder.Services.AddScoped<IRevenueRepository, RevenueRepository>();

var app = builder.Build();

// ------------------------
// Migrate + Seed
// ------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    await SeedData.InitialiseAsync(db);
}

// ------------------------
// Middleware
// ------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
