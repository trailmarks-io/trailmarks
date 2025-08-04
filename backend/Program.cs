using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TrailmarksApi.Data;
using TrailmarksApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Configure Entity Framework with PostgreSQL or SQLite fallback
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var useSqlite = builder.Configuration.GetValue<bool>("UseSqlite") || string.IsNullOrEmpty(connectionString);

if (useSqlite)
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite("Data Source=trailmarks.db"));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionString));
}

// Register services
builder.Services.AddScoped<DatabaseService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Trailmarks API",
        Version = "v1.0",
        Description = "API for managing Wandersteine (hiking stones)",
        TermsOfService = new Uri("http://swagger.io/terms/"),
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "API Support",
            Url = new Uri("http://www.trailmarks.io/support"),
            Email = "support@trailmarks.io"
        },
        License = new Microsoft.OpenApi.Models.OpenApiLicense
        {
            Name = "MIT",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    // Include XML comments for API documentation
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Initialize database only if -DbInit flag is provided
if (args.Contains("-DbInit"))
{
    using (var scope = app.Services.CreateScope())
    {
        var databaseService = scope.ServiceProvider.GetRequiredService<DatabaseService>();
        await databaseService.InitializeAsync();
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Trailmarks API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseCors();

app.UseRouting();

app.MapControllers();

Console.WriteLine("Starting server on http://localhost:8080");
app.Run("http://localhost:8080");
