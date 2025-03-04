using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestTask.Domain.Interfaces;
using TestTask.Infrastructure.Persistence;

namespace TestTask.Infrastructure.Extensions;
public static class DependencyInjection
{
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DbConnection"));
        });

        return services;
    }
}
