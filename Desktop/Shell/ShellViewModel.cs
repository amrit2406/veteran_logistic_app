using System;
using CommunityToolkit.Mvvm.ComponentModel;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Shell;

/// <summary>
/// Shell view model that exposes the current ViewModel provided by navigation service.
/// </summary>
public sealed class ShellViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private object? _currentViewModel;

    public ShellViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        _navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;
        _currentViewModel = _navigationService.CurrentViewModel;

        // If no current ViewModel is provided yet (e.g., before authentication), expose a placeholder.
        if (_currentViewModel is null)
        {
            _currentViewModel = new PlaceholderViewModel();
        }
    }

    private void OnCurrentViewModelChanged(object? vm)
    {
        CurrentViewModel = vm;
    }

    /// <summary>
    /// Current active view model displayed in the shell.
    /// </summary>
    public object? CurrentViewModel
    {
        get => _currentViewModel;
        private set => SetProperty(ref _currentViewModel, value);
    }

    /// <summary>
    /// Temporary placeholder ViewModel shown before the authentication or initial module loads.
    /// </summary>

}

/// <summary>
/// Temporary placeholder ViewModel shown before authentication or initial module loads.
/// </summary>
public sealed class PlaceholderViewModel
{
    public string Title { get; } = "Veteran Logistics";
    public string Subtitle { get; } = "Enterprise Logistics ERP";
    public string Message { get; } = "Application initialized successfully. Authentication module will appear here.";
}
