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
    /// Gets or sets the vendor code.
    /// </summary>
    public string VendorCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the vendor name.
    /// </summary>
    public string VendorName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the GST number.
    /// </summary>
    public string GSTNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the PAN number.
    /// </summary>
    public string PANNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the city.
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the state.
    /// </summary>
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the vendor is active.
    /// </summary>
    public bool IsActive { get; set; }
}
