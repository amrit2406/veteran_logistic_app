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

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="request">The user update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdateUserResult> UpdateUserAsync(UpdateUserRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a user's active status.
    /// </summary>
    /// <param name="request">The user status update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdateUserStatusResult> UpdateUserStatusAsync(UpdateUserStatusRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Resets a user's password.
    /// </summary>
    /// <param name="request">The password reset request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<ResetPasswordResult> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a user (soft delete).
    /// </summary>
    /// <param name="request">The delete user request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<DeleteUserResult> DeleteUserAsync(DeleteUserRequest request, CancellationToken cancellationToken = default);
}
