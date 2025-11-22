using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TrailmarksApi.Data;
using TrailmarksApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults (includes OpenTelemetry, Service Discovery, Health Checks)
builder.AddServiceDefaults();

// Add services to the container
builder.Services.AddControllers();

// Configure ProblemDetails for standardized error responses (RFC 7807)
builder.Services.AddProblemDetails();

// Configure Entity Framework with PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException(
        "PostgreSQL connection string 'DefaultConnection' is not configured. " +
        "Please ensure the connection string is set in one of the supported configuration sources: " +
        "appsettings.json, appsettings.{Environment}.json, environment variables, or command-line arguments. " +
        "For example, you can set the environment variable 'ConnectionStrings__DefaultConnection' to your connection string. " +
        "See https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/ for details on configuration hierarchy."
    );
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString, DbContextOptionsHelper.ConfigureNpgsql));

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
// Use exception handler middleware to convert exceptions to ProblemDetails
app.UseExceptionHandler();
app.UseStatusCodePages();

// Enable Swagger in all environments for API testing
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Trailmarks API v1");
    c.RoutePrefix = "swagger";
});

app.UseCors();

app.UseRouting();

app.MapControllers();

// Map default health check endpoints provided by Aspire
app.MapDefaultEndpoints();

app.Run();
