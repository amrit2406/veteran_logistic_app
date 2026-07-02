using System.Threading.Tasks;

namespace veteran_logistic.Navigation;

/// <summary>
/// Navigation service contract for ViewModel-first navigation.
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// Navigate asynchronously to the specified ViewModel type.
    /// </summary>
    Task<NavigationResult> NavigateAsync<TViewModel>() where TViewModel : class;

    /// <summary>
    /// Navigate asynchronously to the specified ViewModel type with a parameter.
    /// </summary>
    Task<NavigationResult> NavigateAsync<TViewModel>(NavigationParameter? parameter) where TViewModel : class;

    /// <summary>
    /// Navigate back in the navigation stack if possible.
    /// </summary>
    Task<bool> GoBackAsync();

    /// <summary>
    /// Whether it's possible to go back.
    /// </summary>
    bool CanGoBack { get; }

    /// <summary>
    /// The currently active ViewModel.
    /// </summary>
    object? CurrentViewModel { get; }

    /// <summary>
    /// Event raised when the CurrentViewModel changes.
    /// </summary>
    event System.Action<object?>? CurrentViewModelChanged;
}
