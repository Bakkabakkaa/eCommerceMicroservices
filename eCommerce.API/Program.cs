using System.Text.Json.Serialization;
using eCommerce.API.Middlewares;
using eCommerce.Core;
using eCommerce.Core.Mappers;
using eCommerce.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add Infrastructure services
builder.Services.AddInfrastructure();
builder.Services.AddCore();

// Add controllers to the service collection
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(new []
    {
        typeof(ApplicationUserMappingProfile).Assembly
    });
});

// Build the web application
var app = builder.Build();

app.UseExceptionHandlingMiddleware();

// Routing
app.UseRouting();

// Auth
app.UseAuthentication();
app.UseAuthorization();

// Controller routes
app.MapControllers();

app.Run();