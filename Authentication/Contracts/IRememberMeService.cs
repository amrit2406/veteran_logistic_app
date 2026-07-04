using veteran_logistic.Authentication.Models;

namespace veteran_logistic.Authentication.Contracts;

/// <summary>
/// Contract for persisting and restoring Remember Me login preferences.
/// </summary>
public interface IRememberMeService
{
    /// <summary>
    /// Loads saved Remember Me settings from local storage.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The saved settings, or defaults when none exist or the file is invalid.</returns>
    Task<RememberMeSettings> LoadAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves Remember Me settings to local storage.
    /// </summary>
    /// <param name="settings">The settings to persist.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task SaveAsync(RememberMeSettings settings, CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes any previously saved Remember Me settings.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task ClearAsync(CancellationToken cancellationToken = default);
}
