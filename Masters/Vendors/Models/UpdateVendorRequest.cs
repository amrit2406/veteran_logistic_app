namespace veteran_logistic.Masters.Vendors.Models;

/// <summary>
/// Request model for updating an existing vendor.
/// </summary>
public sealed class UpdateVendorRequest
{
    /// <summary>
    /// Gets or sets the vendor ID.
    /// </summary>
    public int VendorId { get; set; }

    /// <summary>
    /// Gets or sets the vendor code.
    /// </summary>
    public string VendorCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the vendor name.
    /// </summary>
    public string VendorName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the first line of the address.
    /// </summary>
    public string AddressLine1 { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the second line of the address.
    /// </summary>
    public string AddressLine2 { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the city.
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the state.
    /// </summary>
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the country.
    /// </summary>
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the postal code.
    /// </summary>
    public string PostalCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the phone number.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the GST number.
    /// </summary>
    public string GSTNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the PAN number.
    /// </summary>
    public string PANNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the contact person.
    /// </summary>
    public string ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the vendor is active.
    /// </summary>
    public bool IsActive { get; set; }
}
