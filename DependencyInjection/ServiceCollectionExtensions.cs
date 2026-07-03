using Microsoft.Extensions.DependencyInjection;
using veteran_logistic.Authentication.DependencyInjection;
using veteran_logistic.Services.Dialog;
using veteran_logistic.Services.Notification;
using VeteranLogistics.Data.DependencyInjection;

namespace veteran_logistic.DependencyInjection;

/// <summary>
/// Extension methods to register dialog and notification infrastructure.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers dialog and notification services as singletons.
    /// </summary>
    /// <param name="services">The service collection to modify.</param>
    /// <returns>The same service collection for chaining.</returns>
    public static IServiceCollection AddDialogAndNotificationServices(this IServiceCollection services)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));

        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<INotificationService, NotificationService>();

        return services;
    }

    /// <summary>
    /// Registers application services. This method is the single place used by the host to
    /// configure DI for the application. It intentionally keeps registrations minimal so
    /// modules can add their own services independently.
    /// </summary>
    public static IServiceCollection RegisterApplication(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));
        
        // Register data layer first (required by authentication repository)
        services.AddVeteranLogisticsData(configuration);
        
        // Preserve existing registrations expected by the host. Keep lightweight and non-destructive.
        services.AddDialogAndNotificationServices();
        services.AddAuthenticationInfrastructure();
        services.AddAuthenticationPersistence();
        services.AddAuthenticationUI();
        services.AddAuthenticationWorkflow();

        // Bind strongly-typed options from configuration so components can receive IOptions<T>
        if (configuration is not null)
        {
            services.Configure<veteran_logistic.Configuration.Options.ApplicationOptions>(configuration.GetSection("Application"));
            services.Configure<veteran_logistic.Configuration.Options.AuthenticationOptions>(configuration.GetSection("Authentication"));
        }

        // Register navigation and shell-related services (includes ShellViewModel)
        services.AddNavigation();

        // Register MainWindow so it can be resolved from DI in App.xaml.cs
        // MainWindow depends on LoginViewModel and IOptions<ApplicationOptions>
        services.AddSingleton<MainWindow>();

        // Additional module registrations should be added in their respective composition roots.
        return services;
    }
}
