using Microsoft.Extensions.DependencyInjection;

namespace veteran_logistic.DependencyInjection;

/// <summary>
/// Prepares navigation registration for future phases.
/// Navigation infrastructure will be implemented in Phase 0.7; this file exposes an internal registration method only.
/// </summary>
internal static class NavigationRegistration
{
    /// <summary>
    /// Adds navigation related registrations. This is a placeholder for future navigation service implementations.
    /// </summary>
    internal static IServiceCollection AddNavigation(this IServiceCollection services)
    {
        // Register NavigationService and related factory as singletons; ViewModels should be registered as Transient elsewhere
        services.AddSingleton<veteran_logistic.Navigation.IViewModelFactory, veteran_logistic.Navigation.ViewModelFactory>();
        services.AddSingleton<veteran_logistic.Navigation.INavigationService, veteran_logistic.Navigation.NavigationService>();
        services.AddSingleton<veteran_logistic.Shell.ShellViewModel>();

        // Dialog and Notification infrastructure
        services.AddSingleton<veteran_logistic.Services.Dialog.IDialogService, veteran_logistic.Services.Dialog.DialogService>();
        services.AddSingleton<veteran_logistic.Services.Notification.INotificationService, veteran_logistic.Services.Notification.NotificationService>();

        return services;
    }
}

