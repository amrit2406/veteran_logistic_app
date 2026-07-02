using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VeteranLogistics.Data.Context;
using VeteranLogistics.Data.Entities.Base;

namespace VeteranLogistics.Data.DependencyInjection;

/// <summary>
/// Registers data access services including the EF Core DbContext.
/// The Desktop project should call <see cref="AddVeteranLogisticsData(IServiceCollection, IConfiguration)"/> to register data services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the VeteranLogistics data layer to the DI container.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">Application configuration root.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddVeteranLogisticsData(this IServiceCollection services, IConfiguration configuration)
    {
        // Bind DatabaseOptions from configuration
        var dbSection = configuration.GetSection("Database");
        var dbOptions = new global::veteran_logistic.Configuration.Options.DatabaseOptions();
        dbSection.Bind(dbOptions);

        // Retrieve connection string using the standard ConnectionStrings section
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        }

        services.AddDbContext<VeteranLogisticsDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                // Configure command timeout if provided
                if (dbOptions.CommandTimeout > 0)
                {
                    sqlOptions.CommandTimeout(dbOptions.CommandTimeout);
                }

                // Configure retry on failure
                if (dbOptions.EnableRetryOnFailure)
                {
                    sqlOptions.EnableRetryOnFailure(dbOptions.MaxRetryCount, TimeSpan.FromSeconds(dbOptions.MaxRetryDelaySeconds), null);
                }
            });

            // Configure EF Core behavior flags
            if (dbOptions.EnableSensitiveDataLogging)
            {
                options.EnableSensitiveDataLogging();
            }

            if (dbOptions.EnableDetailedErrors)
            {
                options.EnableDetailedErrors();
            }
        });

        return services;
    }

    // Connection string construction was removed in favor of using IConfiguration.GetConnectionString("DefaultConnection").
}
