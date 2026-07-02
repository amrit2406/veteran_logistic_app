using Microsoft.Extensions.DependencyInjection;

namespace veteran_logistic.DependencyInjection;

/// <summary>
/// Registers dialog and popup related services and view types.
/// Dialogs are registered as Transient by convention.
/// </summary>
internal static class DialogRegistration
{
    /// <summary>
    /// Adds dialog registrations. No dialogs exist yet; this prepares the structure.
    /// </summary>
    internal static IServiceCollection AddDialogs(this IServiceCollection services)
    {
        // Example for future dialogs:
        // services.AddTransient<IConfirmDialog, ConfirmDialog>();

        return services;
    }
}

