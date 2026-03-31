using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------
// 1️⃣ Resolve Connection String Professionally
// -----------------------------
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Database connection string is not configured.");
}

// -----------------------------
// 2️⃣ Register DbContext (Resilient + Debuggable)
// -----------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString),
        mySqlOptions =>
        {
            // Retry logic for transient failures
            mySqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null
            );
        });

    // Enable detailed logging ONLY in development
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

// -----------------------------
// 3️⃣ Register services
// -----------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -----------------------------
// 4️⃣ Configure CORS
// -----------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins(
                "http://localhost:3000",       // Frontend localhost
                "http://192.168.43.215:3000"  // LAN IP if testing on network
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

// -----------------------------
// 5️⃣ Build App
// -----------------------------
var app = builder.Build();

// -----------------------------
// 6️⃣ Middleware pipeline
// -----------------------------

// Use CORS first
app.UseCors("AllowFrontend");

// Global error handling
app.UseExceptionHandler("/error");

// Swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

// Authorization (future use)
app.UseAuthorization();

// -----------------------------
// 7️⃣ Health Check Endpoint
// -----------------------------
app.MapGet("/", () => Results.Ok(new
{
    status = "Nyumba API running",
    environment = app.Environment.EnvironmentName,
    time = DateTime.UtcNow
}));

// -----------------------------
// 8️⃣ Database Connection Test Endpoint
// -----------------------------
app.MapGet("/db-test", async (AppDbContext db) =>
{
    try
    {
        var canConnect = await db.Database.CanConnectAsync();

        return Results.Ok(new
        {
            connected = canConnect,
            database = "nyumbadb",
            time = DateTime.UtcNow
        });
    }
    catch (Exception ex)
    {
        return Results.Problem(
            title: "Database connection failed",
            detail: ex.Message
        );
    }
});

// -----------------------------
// 9️⃣ Sample endpoint
// -----------------------------
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild",
    "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast(
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        )
    ).ToArray();

    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

// -----------------------------
// 10️⃣ Map Controllers
// -----------------------------
app.MapControllers();

// -----------------------------
// 11️⃣ Run app
// -----------------------------
app.Run();

// -----------------------------
// WeatherForecast record
// -----------------------------
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}