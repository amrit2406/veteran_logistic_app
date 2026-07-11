namespace veteran_logistic.Masters.Vendors.Models;

/// <summary>
/// Request model for updating vendor active status.
/// </summary>
public sealed class UpdateVendorStatusRequest
{
    /// <summary>
    /// Gets or sets the vendor ID.
    /// </summary>
    public int VendorId { get; set; }

    /// <summary>
    /// Gets or sets whether the vendor is active.
    /// </summary>
    public bool IsActive { get; set; }
}
