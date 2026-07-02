using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using veteran_logistic.DependencyInjection;

namespace veteran_logistic.Configuration;

/// <summary>
/// Centralizes Generic Host configuration for the application including Serilog setup.
/// </summary>
public static class HostConfiguration
{
    /// <summary>
    /// Configures the provided IHostBuilder with configuration, logging and DI registrations.
    /// Serilog is configured and registered as the logging provider.
    /// </summary>
    /// <param name="builder">The host builder to configure.</param>
    /// <returns>The configured host builder.</returns>
    public static IHostBuilder ConfigureHost(IHostBuilder builder)
    {
        return builder
            .ConfigureAppConfiguration((context, config) =>
            {
                // Base configuration
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                // Allow environment variables to override configuration
                config.AddEnvironmentVariables();
            })
            // Configure logging and set Serilog as the provider
            .ConfigureLogging((context, logging) =>
            {
                // Build Serilog logger from configuration and install as the logging provider.
                // Enrich logs with application and environment properties so business code
                // does not need to supply these values.
                try
                {
                    var appName = context.Configuration["Application:Name"] ?? "Veteran Logistics";
                    var appVersion = context.Configuration["Application:Version"] ?? "0.0.0";
                    var environment = context.HostingEnvironment?.EnvironmentName ?? "Production";

                    // Retention: read from configuration with sensible default; do not hardcode in code paths.
                    var retainedFiles = context.Configuration.GetValue<int?>("Logging:RetainedFileCountLimit")
                                      ?? context.Configuration.GetValue<int?>("Serilog:RetainedFileCountLimit")
                                      ?? 31;

                    // Output template optimized for structured logging and easy parsing. Avoids exposing sensitive data.
                    var outputTemplate = context.Configuration["Serilog:OutputTemplate"]
                                         ?? "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}]\n[{ApplicationName}]\n[{ApplicationVersion}]\n[Machine: {Machine}]\n[Thread: {ThreadId}]\n\n{Message:lj}{NewLine}{Exception}";

                    // File path can be controlled from configuration (defaults to Logs/VeteranLogistics-.log for daily rolling files)
                    var filePath = context.Configuration["Serilog:FilePath"] ?? "Logs/VeteranLogistics-.log";

                    var loggerConfig = new LoggerConfiguration()
                        .ReadFrom.Configuration(context.Configuration)
                        .Enrich.FromLogContext()
                        // Attach application metadata to every log entry
                        .Enrich.WithProperty("ApplicationName", appName)
                        .Enrich.WithProperty("ApplicationVersion", appVersion)
                        .Enrich.WithProperty("Environment", environment)
                        // Attach machine/process/thread info without requiring extra packages
                        .Enrich.WithProperty("Machine", Environment.MachineName)
                        .Enrich.WithProperty("ProcessId", Environment.ProcessId)
                        .Enrich.WithProperty("ThreadId", Environment.CurrentManagedThreadId)
                        .MinimumLevel.Is(GetMinimumLevel(context.Configuration));

                    // Configure file sink with async wrapper to avoid blocking UI threads. Use retainedFiles from config.
                    // Note: ensure Serilog.Sinks.Async is referenced in the project for WriteTo.Async to be available.
                    loggerConfig = loggerConfig.WriteTo.Async(a => a.File(
                        path: filePath,
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: retainedFiles,
                        outputTemplate: outputTemplate,
                        shared: true));

                    Serilog.Log.Logger = loggerConfig.CreateLogger();

                    logging.ClearProviders();
                    logging.AddSerilog(Serilog.Log.Logger, dispose: true);
                }
                catch (Exception)
                {
                    // If Serilog fails to configure, fall back to default providers.
                    // Do not log sensitive configuration values here.
                    logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                }
            })
            .ConfigureServices((context, services) =>
            {
                // Central place to register application services and UI components.
                services.RegisterApplication(context.Configuration);
            });
    }

    private static LogEventLevel GetMinimumLevel(IConfiguration configuration)
    {
        var min = configuration["Logging:MinimumLevel"] ?? configuration["Serilog:MinimumLevel:Default"] ?? "Information";
        return min.ToLowerInvariant() switch
        {
            "verbose" => LogEventLevel.Verbose,
            "debug" => LogEventLevel.Debug,
            "information" => LogEventLevel.Information,
            "warning" => LogEventLevel.Warning,
            "error" => LogEventLevel.Error,
            "fatal" => LogEventLevel.Fatal,
            _ => LogEventLevel.Information,
        };
    }
}
