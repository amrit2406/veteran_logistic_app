using Microsoft.Extensions.DependencyInjection;
using veteran_logistic.Transactions.LoadingRegisters.Contracts;
using veteran_logistic.Transactions.LoadingRegisters.Services;
using veteran_logistic.Transactions.LoadingRegisters.Validators;
using veteran_logistic.Transactions.LoadingRegisters.ViewModels;

namespace veteran_logistic.Transactions.DependencyInjection;

/// <summary>
/// Extension methods for registering Transactions feature infrastructure.
/// </summary>
public static class TransactionsServiceCollectionExtensions
{
    /// <summary>
    /// Adds Transactions feature infrastructure to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddTransactions(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Loading Register services
        services.AddScoped<ILoadingRegisterQueryService, LoadingRegisterQueryService>();
        services.AddScoped<ILoadingRegisterCommandService, LoadingRegisterCommandService>();
        services.AddScoped<ICreateLoadingRegisterValidator, CreateLoadingRegisterValidator>();
        services.AddScoped<IUpdateLoadingRegisterValidator, UpdateLoadingRegisterValidator>();
        services.AddScoped<IUpdateLoadingRegisterStatusValidator, UpdateLoadingRegisterStatusValidator>();
        services.AddScoped<IDeleteLoadingRegisterValidator, DeleteLoadingRegisterValidator>();
        services.AddTransient<LoadingRegistersViewModel>();
        services.AddTransient<AddLoadingRegisterViewModel>();
        services.AddTransient<EditLoadingRegisterViewModel>();

        return services;
    }
}
