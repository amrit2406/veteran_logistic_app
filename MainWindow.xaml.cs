using System;
using System.Windows;
using Microsoft.Extensions.Options;
using veteran_logistic.Authentication.ViewModels;
using veteran_logistic.Configuration.Options;
using veteran_logistic.Navigation;

namespace veteran_logistic
{
    /// <summary>
    /// Main application window. Hosts the LoginView via DataTemplate mapping of LoginViewModel.
    /// After successful authentication, navigation will transition to the Shell.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly INavigationService _navigationService;

        public MainWindow(LoginViewModel loginViewModel, INavigationService navigationService, IOptions<ApplicationOptions> appOptions)
        {
            InitializeComponent();

            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            // Set DataContext to the LoginViewModel so the ContentControl displays the Login screen via DataTemplates
            DataContext = loginViewModel ?? throw new ArgumentNullException(nameof(loginViewModel));

            Loaded += async (_, _) =>
            {
                await loginViewModel.InitializeAsync().ConfigureAwait(true);
            };

            // Subscribe to navigation changes to update DataContext
            _navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;

            // Set window title from configuration when available
            Title = !string.IsNullOrWhiteSpace(appOptions?.Value?.Name) ? appOptions.Value.Name : "Veteran Logistics";
        }

        private void OnCurrentViewModelChanged(object? newViewModel)
        {
            // Update DataContext when navigation occurs
            // Must use Dispatcher to ensure UI thread access
            Dispatcher.Invoke(() => DataContext = newViewModel);
        }
    }
}
