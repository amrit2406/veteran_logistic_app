using veteran_logistic.Administration.Companies.Models;

namespace veteran_logistic.Administration.Companies.Contracts;

/// <summary>
/// Service contract for querying company data.
/// </summary>
public interface ICompanyQueryService
{
    /// <summary>
    /// Gets all companies.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of company list items.</returns>
    Task<IReadOnlyList<CompanyListItem>> GetAllCompaniesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a company by ID.
    /// </summary>
    /// <param name="id">The company ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The company model, or null if not found.</returns>
    Task<CompanyModel?> GetCompanyAsync(int id, CancellationToken cancellationToken = default);
}
