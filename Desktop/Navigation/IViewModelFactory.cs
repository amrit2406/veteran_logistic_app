using System;

namespace veteran_logistic.Navigation;

/// <summary>
/// Factory abstraction responsible for creating ViewModel instances.
/// This decouples navigation from the DI container implementation.
/// </summary>
public interface IViewModelFactory
{
    /// <summary>
    /// Create a ViewModel instance resolved from DI or constructed as needed.
    /// </summary>
    TViewModel Create<TViewModel>() where TViewModel : class;
}
