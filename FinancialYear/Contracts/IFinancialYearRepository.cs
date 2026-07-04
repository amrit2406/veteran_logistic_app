using veteran_logistic.FinancialYear.Models;

namespace veteran_logistic.FinancialYear.Contracts;

/// <summary>
/// Repository contract for retrieving financial years.
/// </summary>
public interface IFinancialYearRepository
{
    /// <summary>
    /// Retrieves all active financial years.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of active financial years.</returns>
    Task<IEnumerable<veteran_logistic.FinancialYear.Models.FinancialYear>> GetActiveFinancialYearsAsync(CancellationToken cancellationToken = default);
}
