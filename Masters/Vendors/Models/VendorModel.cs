namespace veteran_logistic.Masters.Vendors.Models;

/// <summary>
/// Represents a vendor model for detailed display and editing.
/// </summary>
public sealed class VendorModel
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
    /// Gets or sets the vendor type (Union/Vendor).
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the vendor name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the correspondence address.
    /// </summary>
    public string CorrespondenceAddress { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the city.
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the billing address.
    /// </summary>
    public string BillingAddress { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the phone number.
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the mobile number.
    /// </summary>
    public string Mobile { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the fax number.
    /// </summary>
    public string Fax { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the service tax.
    /// </summary>
    public string ServiceTax { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the CST number.
    /// </summary>
    public string CST { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the PAN number.
    /// </summary>
    public string PAN { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the GSTIN.
    /// </summary>
    public string GSTIN { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the vendor is active.
    /// </summary>
    public bool IsActive { get; set; }
}
