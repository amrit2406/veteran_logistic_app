using System;
using Microsoft.Extensions.DependencyInjection;

namespace veteran_logistic.Navigation;

/// <summary>
/// Default ViewModel factory that resolves instances from the application's IServiceProvider.
/// Only this class knows about the DI container.
/// </summary>
public sealed class ViewModelFactory : IViewModelFactory
{
    private readonly IServiceProvider _provider;

    public ViewModelFactory(IServiceProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    public TViewModel Create<TViewModel>() where TViewModel : class
    {
        return _provider.GetRequiredService<TViewModel>();
    }
}
