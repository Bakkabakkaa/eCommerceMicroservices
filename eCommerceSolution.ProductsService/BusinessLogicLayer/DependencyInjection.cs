using BusinessLogicLayer.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {
        // TO DO: Add Business Logic Layer services into the IoC container

        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(new []
            {
                typeof(ProductAddRequestToProductMappingProfile).Assembly
            });
        });
        
        return services;
    }
}