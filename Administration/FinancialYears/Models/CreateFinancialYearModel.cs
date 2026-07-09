namespace veteran_logistic.Administration.FinancialYears.Models;

/// <summary>
/// Model representing financial year data for creation.
/// </summary>
public sealed class CreateFinancialYearModel
{
    /// <summary>
    /// Gets or sets the financial year name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the start date of the financial year.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Gets or sets the end date of the financial year.
    /// </summary>
    public DateTime EndDate { get; set; }
}
