using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, 
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");
        
        // Add services to container
        // services.AddDbContxt<> ... devamı var msql bağlantısı ve scoped
        
        return services;
    }
}