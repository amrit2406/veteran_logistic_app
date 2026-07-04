using veteran_logistic.FinancialYear.Contracts;
using veteran_logistic.FinancialYear.Models;

namespace veteran_logistic.FinancialYear.Repositories;

/// <summary>
/// Stub repository implementation for retrieving financial years.
/// </summary>
public sealed class FinancialYearRepository : IFinancialYearRepository
{
    /// <inheritdoc />
    public Task<IEnumerable<veteran_logistic.FinancialYear.Models.FinancialYear>> GetActiveFinancialYearsAsync(CancellationToken cancellationToken = default)
    {
        // For Phase 1.6, we return a hardcoded list to simulate an EF/database repository.
        var years = new List<veteran_logistic.FinancialYear.Models.FinancialYear>
        {
            new veteran_logistic.FinancialYear.Models.FinancialYear { Id = 1, Name = "2023-2024", StartDate = new DateTime(2023, 4, 1), EndDate = new DateTime(2024, 3, 31), IsActive = true },
            new veteran_logistic.FinancialYear.Models.FinancialYear { Id = 2, Name = "2024-2025", StartDate = new DateTime(2024, 4, 1), EndDate = new DateTime(2025, 3, 31), IsActive = true },
            new veteran_logistic.FinancialYear.Models.FinancialYear { Id = 3, Name = "2025-2026", StartDate = new DateTime(2025, 4, 1), EndDate = new DateTime(2026, 3, 31), IsActive = true }
        };

        return Task.FromResult<IEnumerable<veteran_logistic.FinancialYear.Models.FinancialYear>>(years);
    }
}
