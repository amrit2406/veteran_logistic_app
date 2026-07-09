using VeteranLogistics.Data.Entities.Base;

namespace VeteranLogistics.Data.Entities.Administration;

/// <summary>
/// Represents a financial year in the system.
/// </summary>
public class FinancialYear : BaseEntity
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

    /// <summary>
    /// Gets or sets whether this is the current financial year.
    /// </summary>
    public bool IsCurrent { get; set; } = false;

    /// <summary>
    /// Gets or sets whether the financial year is closed.
    /// </summary>
    public bool IsClosed { get; set; } = false;

    /// <summary>
    /// Gets or sets whether the financial year has been soft-deleted.
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Gets or sets the timestamp when the financial year was soft-deleted.
    /// </summary>
    public DateTime? DeletedOn { get; set; }
}
