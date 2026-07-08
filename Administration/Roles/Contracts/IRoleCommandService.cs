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
}
