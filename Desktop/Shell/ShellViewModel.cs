using System;
using CommunityToolkit.Mvvm.ComponentModel;
using veteran_logistic.Authentication.ViewModels;
using veteran_logistic.Navigation;

namespace veteran_logistic.Shell;

/// <summary>
/// Shell view model that exposes the current ViewModel provided by navigation service.
/// </summary>
public sealed class ShellViewModel : ObservableObject
{
    private readonly PlaceholderViewModel _placeholder = new();
    private object? _currentViewModel;

    public ShellViewModel(INavigationService navigationService)
    {
        if (navigationService is null) throw new ArgumentNullException(nameof(navigationService));

        navigationService.CurrentViewModelChanged += OnCurrentViewModelChanged;
        _currentViewModel = ResolveShellContent(navigationService.CurrentViewModel) ?? _placeholder;
    }

    private void OnCurrentViewModelChanged(object? vm)
    {
        CurrentViewModel = ResolveShellContent(vm) ?? _placeholder;
    }

    private static object? ResolveShellContent(object? vm) => vm switch
    {
        null or ShellViewModel or LoginViewModel => null,
        _ => vm
    };

    /// <summary>
    /// Current active view model displayed in the shell.
    /// </summary>
    public object? CurrentViewModel
    {
        get => _currentViewModel;
        private set => SetProperty(ref _currentViewModel, value);
    }
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
