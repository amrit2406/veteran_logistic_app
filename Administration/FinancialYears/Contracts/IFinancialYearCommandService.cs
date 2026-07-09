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

    /// <summary>
    /// Updates an existing financial year.
    /// </summary>
    /// <param name="request">The financial year update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdateFinancialYearResult> UpdateFinancialYearAsync(UpdateFinancialYearRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets a financial year as the current financial year.
    /// </summary>
    /// <param name="request">The set current financial year request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<SetCurrentFinancialYearResult> SetCurrentFinancialYearAsync(SetCurrentFinancialYearRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Closes a financial year.
    /// </summary>
    /// <param name="request">The close financial year request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<CloseFinancialYearResult> CloseFinancialYearAsync(CloseFinancialYearRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a financial year (soft delete).
    /// </summary>
    /// <param name="request">The delete financial year request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<DeleteFinancialYearResult> DeleteFinancialYearAsync(DeleteFinancialYearRequest request, CancellationToken cancellationToken = default);
}
