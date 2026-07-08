using veteran_logistic.Administration.Roles.Models;

namespace veteran_logistic.Administration.Roles.Contracts;

/// <summary>
/// Service contract for role command operations.
/// </summary>
public interface IRoleCommandService
{
    /// <summary>
    /// Creates a new role.
    /// </summary>
    /// <param name="request">The role creation request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result containing the created role ID.</returns>
    Task<CreateRoleResult> CreateRoleAsync(CreateRoleRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing role.
    /// </summary>
    /// <param name="request">The role update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdateRoleResult> UpdateRoleAsync(UpdateRoleRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a role's active status.
    /// </summary>
    /// <param name="request">The role status update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdateRoleStatusResult> UpdateRoleStatusAsync(UpdateRoleStatusRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a role (soft delete).
    /// </summary>
    /// <param name="request">The delete role request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<DeleteRoleResult> DeleteRoleAsync(DeleteRoleRequest request, CancellationToken cancellationToken = default);
}
