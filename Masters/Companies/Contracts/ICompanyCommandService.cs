using veteran_logistic.Masters.Companies.Models;

namespace veteran_logistic.Masters.Companies.Contracts;

/// <summary>
/// Service contract for company command operations.
/// </summary>
public interface ICompanyCommandService
{
    /// <summary>
    /// Creates a new company.
    /// </summary>
    /// <param name="request">The company creation request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result containing the created company ID.</returns>
    Task<CreateCompanyResult> CreateCompanyAsync(CreateCompanyRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing company.
    /// </summary>
    /// <param name="request">The company update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdateCompanyResult> UpdateCompanyAsync(UpdateCompanyRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a company's active status.
    /// </summary>
    /// <param name="request">The company status update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdateCompanyStatusResult> UpdateCompanyStatusAsync(UpdateCompanyStatusRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a company (soft delete).
    /// </summary>
    /// <param name="request">The delete company request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<DeleteCompanyResult> DeleteCompanyAsync(DeleteCompanyRequest request, CancellationToken cancellationToken = default);
}
