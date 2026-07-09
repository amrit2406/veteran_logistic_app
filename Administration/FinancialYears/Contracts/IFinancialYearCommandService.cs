using veteran_logistic.Administration.FinancialYears.Models;

namespace veteran_logistic.Administration.FinancialYears.Contracts;

/// <summary>
/// Service contract for financial year command operations.
/// </summary>
public interface IFinancialYearCommandService
{
    /// <summary>
    /// Creates a new financial year.
    /// </summary>
    /// <param name="request">The financial year creation request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result containing the created financial year ID.</returns>
    Task<CreateFinancialYearResult> CreateFinancialYearAsync(CreateFinancialYearRequest request, CancellationToken cancellationToken = default);
}
