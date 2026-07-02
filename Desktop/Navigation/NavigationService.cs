using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using veteran_logistic.MVVM;

namespace veteran_logistic.Navigation;

/// <summary>
/// Simple DI-friendly ViewModel-first navigation service.
/// Resolves ViewModels via an IViewModelFactory and maintains a lightweight history stack.
/// </summary>
public sealed class NavigationService : INavigationService
{
    private readonly IViewModelFactory _factory;
    private readonly Stack<object> _history = new();

    private object? _current;

    public event Action<object?>? CurrentViewModelChanged;

    public NavigationService(IViewModelFactory factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public object? CurrentViewModel => _current;

    public bool CanGoBack => _history.Count > 0;

    public async Task<NavigationResult> NavigateAsync<TViewModel>() where TViewModel : class
    {
        return await NavigateAsync<TViewModel>(null).ConfigureAwait(false);
    }

    public async Task<NavigationResult> NavigateAsync<TViewModel>(NavigationParameter? parameter) where TViewModel : class
    {
        try
        {
            // Create the ViewModel via factory which encapsulates the DI container
            var vm = _factory.Create<TViewModel>();

            // Push current to history if present
            if (_current is not null)
            {
                _history.Push(_current);
            }

            // Set current and notify
            _current = vm;
            CurrentViewModelChanged?.Invoke(_current);

            // If the ViewModel is a ViewModelBase, call InitializeAsync()
            if (vm is ViewModelBase lifecycle)
            {
                await lifecycle.InitializeAsync().ConfigureAwait(false);
            }

            return NavigationResult.Success();
        }
        catch (Exception ex)
        {
            return NavigationResult.Failure(ex.Message);
        }
    }

    public Task<bool> GoBackAsync()
    {
        if (!CanGoBack)
        {
            return Task.FromResult(false);
        }

        var previous = _history.Pop();
        _current = previous;
        CurrentViewModelChanged?.Invoke(_current);
        return Task.FromResult(true);
    }
}
