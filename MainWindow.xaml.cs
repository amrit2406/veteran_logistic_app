using System;
using System.Windows;
using Microsoft.Extensions.Options;
using veteran_logistic.Shell;
using veteran_logistic.Configuration.Options;

namespace veteran_logistic
{
    /// <summary>
    /// Main application window. Hosts the ShellView via DataTemplate mapping of ShellViewModel.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(ShellViewModel shellViewModel, IOptions<ApplicationOptions> appOptions)
        {
            InitializeComponent();

            // Set DataContext to the ShellViewModel so the ContentControl displays the Shell via DataTemplates
            DataContext = shellViewModel ?? throw new ArgumentNullException(nameof(shellViewModel));

            // Set window title from configuration when available
            Title = !string.IsNullOrWhiteSpace(appOptions?.Value?.Name) ? appOptions.Value.Name : "Veteran Logistics";
        }
    }
}
