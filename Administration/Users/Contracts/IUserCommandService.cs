using veteran_logistic.Administration.Users.Models;

namespace veteran_logistic.Administration.Users.Contracts;

/// <summary>
/// Service contract for user command operations.
/// </summary>
public interface IUserCommandService
{
    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="request">The user creation request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result containing the created user ID.</returns>
    Task<CreateUserResult> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
}
