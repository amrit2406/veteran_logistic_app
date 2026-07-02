using Microsoft.Extensions.DependencyInjection;

namespace veteran_logistic.DependencyInjection;

/// <summary>
/// Registers ViewModel types.
/// ViewModels are registered as Transient by convention.
/// </summary>
internal static class ViewModelRegistration
{
    /// <summary>
    /// Adds ViewModel registrations for the application.
    /// </summary>
    internal static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        // No view models yet. Add registrations like:
        // services.AddTransient<MainViewModel>();

        return services;
    }
}

