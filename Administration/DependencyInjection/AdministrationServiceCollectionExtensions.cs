using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using veteran_logistic.Administration.Users.Contracts;
using veteran_logistic.Administration.Users.Services;
using veteran_logistic.Administration.Users.Validators;
using veteran_logistic.Administration.Users.ViewModels;
using veteran_logistic.Administration.Roles.Contracts;
using veteran_logistic.Administration.Roles.Services;
using veteran_logistic.Administration.Roles.ViewModels;

namespace veteran_logistic.Administration.DependencyInjection;

/// <summary>
/// Extension methods for registering Administration feature infrastructure.
/// </summary>
public static class AdministrationServiceCollectionExtensions
{
    /// <summary>
    /// Adds Administration feature infrastructure to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddAdministration(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<IUserQueryService, UserQueryService>();
        services.AddScoped<IUserCommandService, UserCommandService>();
        services.AddScoped<ICreateUserValidator, CreateUserValidator>();
        services.AddScoped<IUpdateUserValidator, UpdateUserValidator>();
        services.AddScoped<IUpdateUserStatusValidator, UpdateUserStatusValidator>();
        services.AddScoped<IResetPasswordValidator, ResetPasswordValidator>();
        services.AddScoped<IDeleteUserValidator, DeleteUserValidator>();
        services.AddTransient<UsersViewModel>();
        services.AddTransient<AddUserViewModel>();
        services.AddTransient<EditUserViewModel>();
        services.AddTransient<ResetPasswordViewModel>();

        services.AddScoped<IRoleQueryService, RoleQueryService>();
        services.AddTransient<RolesViewModel>();

        return services;
    }
}
