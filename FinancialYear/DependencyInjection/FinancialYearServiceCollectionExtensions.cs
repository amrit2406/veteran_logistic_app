using Microsoft.Extensions.DependencyInjection;
using veteran_logistic.FinancialYear.Contracts;
using veteran_logistic.FinancialYear.Repositories;
using veteran_logistic.FinancialYear.Services;
using veteran_logistic.FinancialYear.Session;
using veteran_logistic.FinancialYear.ViewModels;
using veteran_logistic.FinancialYear.Views;

namespace veteran_logistic.FinancialYear.DependencyInjection;

/// <summary>
/// Extension methods for registering Financial Year Selection dependencies.
/// </summary>
public static class FinancialYearServiceCollectionExtensions
{
    /// <summary>
    /// Registers Financial Year related services, repositories, and UI.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddFinancialYearSelection(this IServiceCollection services)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));

        // Repositories & Services
        services.AddSingleton<IFinancialYearRepository, FinancialYearRepository>();
        services.AddSingleton<IFinancialYearService, FinancialYearService>();

        // Context
        services.AddSingleton<IFinancialYearContext, FinancialYearContext>();

        // ViewModels
        services.AddTransient<FinancialYearSelectionViewModel>();

        // Views
        services.AddTransient<FinancialYearSelectionView>();

        return services;
    }
}
