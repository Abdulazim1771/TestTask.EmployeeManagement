using Syncfusion.Licensing;
using TestTask.Infrastructure.Extensions;

namespace TestTask.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterInfrastructure(configuration);

        AddProviders(configuration);

        return services;
    }

    private static void AddProviders(IConfiguration configuration)
    {
        var key = configuration.GetValue<string>("SyncfusionKey")
            ?? throw new InvalidOperationException("Syncfusion key is not found.");

        SyncfusionLicenseProvider.RegisterLicense(key);
    }
}
