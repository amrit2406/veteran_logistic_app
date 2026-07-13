using VeteranLogistics.Data.Entities.Base;

namespace VeteranLogistics.Data.Entities.Administration;

/// <summary>
/// Represents a payment location in the system.
/// </summary>
public class PaymentLocation : BaseEntity
{
    /// <summary>
    /// Gets or sets the payment location name.
    /// </summary>
    public string PaymentLocationName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the payment location is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the payment location has been soft-deleted.
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Gets or sets the timestamp when the payment location was soft-deleted.
    /// </summary>
    public DateTime? DeletedOn { get; set; }

    /// <summary>
    /// Gets or sets the user who created the payment location.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user who last modified the payment location.
    /// </summary>
    public string ModifiedBy { get; set; } = string.Empty;
}
