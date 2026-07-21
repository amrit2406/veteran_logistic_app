namespace veteran_logistic.Masters.Vendors.Models;

/// <summary>
/// Represents a vendor item for display in the vendor listing grid.
/// </summary>
public sealed class VendorListItem
{
    /// <summary>
    /// Gets or sets the vendor ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the vendor code (auto-generated).
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the vendor name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the GSTIN.
    /// </summary>
    public string GSTIN { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the PAN number.
    /// </summary>
    public string PAN { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the city.
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the vendor is active.
    /// </summary>
    public bool IsActive { get; set; }
}
