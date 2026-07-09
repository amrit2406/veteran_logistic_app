using Microsoft.Extensions.DependencyInjection;
using veteran_logistic.Masters.Companies.Contracts;
using veteran_logistic.Masters.Companies.Services;
using veteran_logistic.Masters.Companies.ViewModels;

namespace veteran_logistic.Masters.DependencyInjection;

/// <summary>
/// Extension methods for registering Masters feature infrastructure.
/// </summary>
public static class MastersServiceCollectionExtensions
{
    /// <summary>
    /// Adds Masters feature infrastructure to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddMasters(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<ICompanyQueryService, CompanyQueryService>();
        services.AddTransient<CompaniesViewModel>();

        return services;
    }
}
