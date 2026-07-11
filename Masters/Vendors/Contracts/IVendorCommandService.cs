using veteran_logistic.Masters.Vendors.Models;

namespace veteran_logistic.Masters.Vendors.Contracts;

/// <summary>
/// Service contract for vendor command operations.
/// </summary>
public interface IVendorCommandService
{
    /// <summary>
    /// Creates a new vendor.
    /// </summary>
    /// <param name="request">The vendor creation request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result containing the created vendor ID.</returns>
    Task<CreateVendorResult> CreateVendorAsync(CreateVendorRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing vendor.
    /// </summary>
    /// <param name="request">The vendor update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdateVendorResult> UpdateVendorAsync(UpdateVendorRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a vendor's active status.
    /// </summary>
    /// <param name="request">The vendor status update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdateVendorStatusResult> UpdateVendorStatusAsync(UpdateVendorStatusRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a vendor (soft delete).
    /// </summary>
    /// <param name="request">The delete vendor request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<DeleteVendorResult> DeleteVendorAsync(DeleteVendorRequest request, CancellationToken cancellationToken = default);
}
