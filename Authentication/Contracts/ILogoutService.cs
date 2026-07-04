namespace veteran_logistic.Authentication.Contracts;

/// <summary>
/// Contract for handling user logout operations.
/// </summary>
public interface ILogoutService
{
    /// <summary>
    /// Logs out the current user by clearing the runtime session and returning to the Login screen.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task LogoutAsync(CancellationToken cancellationToken = default);
}
