using veteran_logistic.Masters.Companies.Models;

namespace veteran_logistic.Masters.Companies.Contracts;

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
    /// Searches companies by company code, company name, city, or state.
    /// </summary>
    /// <param name="search">The search term.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of company list items matching the search criteria.</returns>
    Task<IReadOnlyList<CompanyListItem>> SearchCompaniesAsync(string? search, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a company for editing by company ID.
    /// </summary>
    /// <param name="id">The company ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The company model, or null if not found.</returns>
    Task<CompanyModel?> GetCompanyForEditAsync(int id, CancellationToken cancellationToken = default);
}
