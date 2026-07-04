using CommunityToolkit.Mvvm.ComponentModel;
using veteran_logistic.FinancialYear.Contracts;
using veteran_logistic.FinancialYear.Models;

namespace veteran_logistic.FinancialYear.Session;

/// <summary>
/// Implementation of the financial year context.
/// </summary>
public sealed class FinancialYearContext : ObservableObject, IFinancialYearContext
{
    private veteran_logistic.FinancialYear.Models.FinancialYear? _selectedFinancialYear;

    /// <inheritdoc />
    public veteran_logistic.FinancialYear.Models.FinancialYear? SelectedFinancialYear
    {
        get => _selectedFinancialYear;
        private set => SetProperty(ref _selectedFinancialYear, value);
    }

    /// <inheritdoc />
    public void SetFinancialYear(veteran_logistic.FinancialYear.Models.FinancialYear financialYear)
    {
        SelectedFinancialYear = financialYear ?? throw new ArgumentNullException(nameof(financialYear));
    }

    /// <inheritdoc />
    public void ClearFinancialYear()
    {
        SelectedFinancialYear = null;
    }
}
