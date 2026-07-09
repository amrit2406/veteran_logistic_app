namespace veteran_logistic.Administration.FinancialYears.Models;

/// <summary>
/// Request model for updating an existing financial year.
/// </summary>
public sealed class UpdateFinancialYearRequest
{
    /// <summary>
    /// Gets or sets the financial year ID.
    /// </summary>
    public int Id { get; set; }

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

    /// <summary>
    /// Gets or sets whether this is the current financial year.
    /// </summary>
    public bool IsCurrent { get; set; }

    /// <summary>
    /// Gets or sets whether the financial year is closed.
    /// </summary>
    public bool IsClosed { get; set; }
}
