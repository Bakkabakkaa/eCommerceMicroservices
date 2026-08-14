using System.Text.Json.Serialization;
using eCommerce.API.Middlewares;
using eCommerce.Core;
using eCommerce.Core.Mappers;
using eCommerce.Infrastructure;
using FluentValidation.AspNetCore;

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

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();

// Add API explorer services
builder.Services.AddEndpointsApiExplorer();

// Add swagger generation services to create swagger specification
builder.Services.AddSwaggerGen();

// Add cors services
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http:localhost:4200").AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Build the web application
var app = builder.Build();

app.UseExceptionHandlingMiddleware();

// Routing
app.UseRouting();

// Adds endpoint that can serve the swagger.json
app.UseSwagger();

// Adds swagger UI (interactive page to explore and test EPI endpoints)
app.UseSwaggerUI();

app.UseCors();

// Auth
app.UseAuthentication();
app.UseAuthorization();

// Controller routes
app.MapControllers();

app.Run();