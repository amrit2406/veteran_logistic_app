using veteran_logistic.Administration.FinancialYears.Models;

namespace veteran_logistic.Administration.FinancialYears.Contracts;

/// <summary>
/// Service contract for querying financial year data.
/// </summary>
public interface IFinancialYearQueryService
{
    /// <summary>
    /// Gets all financial years.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of financial year list items.</returns>
    Task<IReadOnlyList<FinancialYearListItem>> GetAllFinancialYearsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a financial year by ID.
    /// </summary>
    /// <param name="id">The financial year ID.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The financial year model, or null if not found.</returns>
    Task<FinancialYearModel?> GetFinancialYearAsync(int id, CancellationToken cancellationToken = default);
}
