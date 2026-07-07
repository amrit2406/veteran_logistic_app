namespace veteran_logistic.Navigation;

/// <summary>
/// Interface for ViewModels that need to receive navigation parameters.
/// Implement this interface to receive parameters during navigation
/// instead of storing them as permanent state on the ViewModel.
/// </summary>
public interface INavigationAware
{
    /// <summary>
    /// Called when the ViewModel is navigated to with optional parameters.
    /// </summary>
    /// <param name="parameter">The navigation parameter, or null if none was provided.</param>
    void OnNavigatedTo(NavigationParameter? parameter);
}
