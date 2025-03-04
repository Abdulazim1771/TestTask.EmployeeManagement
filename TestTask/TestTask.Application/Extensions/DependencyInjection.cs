using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestTask.Application.Interfaces;
using TestTask.Application.Services;
using TestTask.Domain.Entities;

namespace TestTask.Application.Extensions;
public static class DependencyInjection
{
    public static IServiceCollection RegisterApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<ICsvParser<Employee>, EmployeeCsvParserService>();

        return services;
    }
}
