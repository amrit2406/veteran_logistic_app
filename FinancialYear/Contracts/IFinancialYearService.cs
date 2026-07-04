using veteran_logistic.FinancialYear.Models;

namespace veteran_logistic.FinancialYear.Contracts;

/// <summary>
/// Service contract for financial year selection.
/// </summary>
public interface IFinancialYearService
{
    /// <summary>
    /// Gets the available financial years.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A collection of financial years available for selection.</returns>
    Task<IEnumerable<veteran_logistic.FinancialYear.Models.FinancialYear>> GetAvailableFinancialYearsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Selects a financial year and validates the selection.
    /// </summary>
    /// <param name="financialYearId">The ID of the financial year to select.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the selection operation.</returns>
    Task<FinancialYearSelectionResult> SelectFinancialYearAsync(int financialYearId, CancellationToken cancellationToken = default);
}
