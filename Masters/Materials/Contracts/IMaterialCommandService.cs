using veteran_logistic.Masters.Materials.Models;

namespace veteran_logistic.Masters.Materials.Contracts;

/// <summary>
/// Service contract for material command operations.
/// </summary>
public interface IMaterialCommandService
{
    /// <summary>
    /// Creates a new material.
    /// </summary>
    /// <param name="request">The material creation request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result containing the created material ID.</returns>
    Task<CreateMaterialResult> CreateMaterialAsync(CreateMaterialRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing material.
    /// </summary>
    /// <param name="request">The material update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdateMaterialResult> UpdateMaterialAsync(UpdateMaterialRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a material's active status.
    /// </summary>
    /// <param name="request">The material status update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdateMaterialStatusResult> UpdateMaterialStatusAsync(UpdateMaterialStatusRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a material (soft delete).
    /// </summary>
    /// <param name="request">The delete material request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<DeleteMaterialResult> DeleteMaterialAsync(DeleteMaterialRequest request, CancellationToken cancellationToken = default);
}
