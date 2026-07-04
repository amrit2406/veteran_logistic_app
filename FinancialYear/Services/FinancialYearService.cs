using veteran_logistic.FinancialYear.Contracts;
using veteran_logistic.FinancialYear.Models;

namespace veteran_logistic.FinancialYear.Services;

/// <summary>
/// Implementation of the financial year service.
/// </summary>
public sealed class FinancialYearService : IFinancialYearService
{
    private readonly IFinancialYearRepository _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="FinancialYearService"/> class.
    /// </summary>
    /// <param name="repository">The financial year repository.</param>
    public FinancialYearService(IFinancialYearRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<veteran_logistic.FinancialYear.Models.FinancialYear>> GetAvailableFinancialYearsAsync(CancellationToken cancellationToken = default)
    {
        return await _repository.GetActiveFinancialYearsAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<FinancialYearSelectionResult> SelectFinancialYearAsync(int financialYearId, CancellationToken cancellationToken = default)
    {
        var activeYears = await _repository.GetActiveFinancialYearsAsync(cancellationToken).ConfigureAwait(false);
        var selectedYear = activeYears.FirstOrDefault(fy => fy.Id == financialYearId);

        if (selectedYear == null)
        {
            return FinancialYearSelectionResult.Failure("Selected financial year is invalid or inactive.");
        }

        return FinancialYearSelectionResult.Success(selectedYear);
    }
}
