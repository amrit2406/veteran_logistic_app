using System.ComponentModel;
using veteran_logistic.FinancialYear.Models;

namespace veteran_logistic.FinancialYear.Contracts;

/// <summary>
/// Represents the runtime context for the selected financial year.
/// </summary>
public interface IFinancialYearContext : INotifyPropertyChanged
{
    /// <summary>
    /// Gets the currently selected financial year.
    /// </summary>
    veteran_logistic.FinancialYear.Models.FinancialYear? SelectedFinancialYear { get; }

    /// <summary>
    /// Sets the selected financial year for the context.
    /// </summary>
    /// <param name="financialYear">The financial year to set.</param>
    void SetFinancialYear(veteran_logistic.FinancialYear.Models.FinancialYear financialYear);

    /// <summary>
    /// Clears the selected financial year from the context.
    /// </summary>
    void ClearFinancialYear();
}
