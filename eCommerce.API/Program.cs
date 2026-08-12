using System.Text.Json.Serialization;
using eCommerce.API.Middlewares;
using eCommerce.Core;
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