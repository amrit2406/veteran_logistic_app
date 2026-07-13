namespace veteran_logistic.Masters.VehicleOwners.Models;

/// <summary>
/// Request model for creating a vehicle owner.
/// </summary>
public sealed class CreateVehicleOwnerRequest
{
    /// <summary>
    /// Gets or sets the PAN type.
    /// </summary>
    public string PANType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the PAN number.
    /// </summary>
    public string PANNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the middle name.
    /// </summary>
    public string MiddleName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the company name.
    /// </summary>
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the city.
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the state.
    /// </summary>
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the address.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the phone number.
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the mobile number.
    /// </summary>
    public string Mobile { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the fax number.
    /// </summary>
    public string Fax { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the vehicle owner is active.
    /// </summary>
    public bool IsActive { get; set; } = true;
}
