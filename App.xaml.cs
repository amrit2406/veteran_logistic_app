using System;

using System.Threading.Tasks;

using System.Windows;

using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Logging;

using Microsoft.Extensions.Options;

using veteran_logistic.Services.Dialog;

using veteran_logistic.Authentication.Contracts;

using veteran_logistic.Configuration.Options;

using VeteranLogistics.Data.Context;

using VeteranLogistics.Data.Seed;

using Serilog;



namespace veteran_logistic

{

    /// <summary>

    /// Interaction logic for App.xaml

    /// </summary>

    public partial class App : Application

    {

        private static Microsoft.Extensions.Hosting.IHost? _host;



        /// <summary>

        /// The application generic host instance.

        /// </summary>

        public static Microsoft.Extensions.Hosting.IHost Host => _host ?? throw new InvalidOperationException("Host has not been initialized.");



        private async void Application_Startup(object sender, StartupEventArgs e)

        {

            // Build host using Host.CreateDefaultBuilder and central HostConfiguration

            var builder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder();

            builder = veteran_logistic.Configuration.HostConfiguration.ConfigureHost(builder);



            _host = builder.Build();

            await _host.StartAsync();



            // Seed default administrator user

            try

            {

                var dbContext = _host.Services.GetRequiredService<VeteranLogisticsDbContext>();

                var authenticationOptions = _host.Services.GetRequiredService<IOptions<AuthenticationOptions>>().Value;

                var passwordHasher = _host.Services.GetRequiredService<IPasswordHasher>();

                

                await AuthenticationSeed.EnsureDefaultAdministratorAsync(dbContext, authenticationOptions, passwordHasher);

                await PermissionSeed.EnsurePermissionsAsync(dbContext);

            }

            catch (Exception ex)

            {

                var logger = _host.Services.GetRequiredService<ILogger<App>>();

                logger.LogError(ex, "Failed to seed default administrator user.");

            }



            // Register global exception handlers after the host is ready so services can be resolved

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;



            // Resolve MainWindow from DI and show

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();

            mainWindow.Show();

        }



        private async void Application_Exit(object sender, ExitEventArgs e)

        {

            if (_host is null)

            {

                return;

            }



            try

            {

                await _host.StopAsync();

            }

            catch

            {

                // ignored - shutting down anyway

            }

            finally

            {

                try

                {

                    Log.CloseAndFlush(); // Ensure all logs are written

                } catch { /* ignore */ }

                _host.Dispose();

            }

        }



        private async void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)

        {

            try

            {

                var logger = _host?.Services.GetRequiredService<ILogger<App>>();

                logger?.LogCritical(e.Exception, "An unhandled exception occurred on the UI thread.");



                await ShowUnhandledErrorAsync();

            }

            catch

            {

                // If logging/dialog fails, we can't do much more.

            } 



            e.Handled = true;

        }



        private async void CurrentDomain_UnhandledException(object? sender, UnhandledExceptionEventArgs e)

        {

            try

            {

                var ex = e.ExceptionObject as Exception ?? new Exception("Unhandled domain exception");

                var logger = _host?.Services.GetRequiredService<ILogger<App>>();

                logger?.LogCritical(ex, "An unhandled exception occurred in the AppDomain.");



                await ShowUnhandledErrorAsync();

            }

            catch

            {

            }

        }



        private async void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)

        {

            try

            {

                var ex = e.Exception;

                var logger = _host?.Services.GetRequiredService<ILogger<App>>();

                logger?.LogCritical(ex, "An unobserved task exception occurred.");



                await ShowUnhandledErrorAsync();

                e.SetObserved();

            }

            catch

            {

            }

        }



        private async Task ShowUnhandledErrorAsync()

        {

            var dialog = _host?.Services.GetService<IDialogService>();

            if (dialog is null)

                return;



            await dialog.ShowErrorAsync(new DialogOptions

            {

                Title = "An unexpected error occurred",

                Message = "The application has encountered a critical error and may need to close. The error has been logged.",

            });

        }

    }

}

