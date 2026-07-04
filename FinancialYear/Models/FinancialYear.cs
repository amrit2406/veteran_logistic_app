namespace veteran_logistic.FinancialYear.Models;

/// <summary>
/// Represents a Financial Year entity.
/// </summary>
public sealed class FinancialYear
{
    /// <summary>
    /// Gets or sets the unique identifier for the financial year.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the display name of the financial year (e.g., "2023-2024").
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

    /// <summary>
    /// Gets or sets whether the financial year is active.
    /// </summary>
    public bool IsActive { get; set; }
}
