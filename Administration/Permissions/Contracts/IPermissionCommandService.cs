using veteran_logistic.Administration.Permissions.Models;

namespace veteran_logistic.Administration.Permissions.Contracts;

/// <summary>
/// Service contract for permission command operations.
/// </summary>
public interface IPermissionCommandService
{
    /// <summary>
    /// Assigns permissions to a role.
    /// </summary>
    /// <param name="request">The assign permissions request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<AssignPermissionsResult> AssignPermissionsAsync(AssignPermissionsRequest request, CancellationToken cancellationToken = default);
}
