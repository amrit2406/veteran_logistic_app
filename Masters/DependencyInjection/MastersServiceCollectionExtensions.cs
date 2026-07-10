using Microsoft.Extensions.DependencyInjection;
using veteran_logistic.Masters.Companies.Contracts;
using veteran_logistic.Masters.Companies.Services;
using veteran_logistic.Masters.Companies.Validators;
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
        services.AddScoped<ICompanyCommandService, CompanyCommandService>();
        services.AddScoped<ICreateCompanyValidator, CreateCompanyValidator>();
        services.AddScoped<IUpdateCompanyValidator, UpdateCompanyValidator>();
        services.AddScoped<IUpdateCompanyStatusValidator, UpdateCompanyStatusValidator>();
        services.AddScoped<IDeleteCompanyValidator, DeleteCompanyValidator>();
        services.AddTransient<CompaniesViewModel>();
        services.AddTransient<AddCompanyViewModel>();
        services.AddTransient<EditCompanyViewModel>();

        return services;
    }
}
