namespace veteran_logistic.Masters.VehicleOwners.Models;

/// <summary>
/// Represents a vehicle owner item for display in the vehicle owner listing grid.
/// </summary>
public sealed class VehicleOwnerListItem
{
    /// <summary>
    /// Gets or sets the vehicle owner ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the owner code.
    /// </summary>
    public string OwnerCode { get; set; } = string.Empty;

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
    /// Gets or sets the last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the company name.
    /// </summary>
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>
    /// Gets the owner name (company name if available, otherwise first and last name).
    /// </summary>
    public string OwnerName
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(CompanyName))
            {
                return CompanyName;
            }
            return $"{FirstName} {LastName}".Trim();
        }
    }

    /// <summary>
    /// Gets or sets the mobile number.
    /// </summary>
    public string Mobile { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the city.
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the state.
    /// </summary>
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the vehicle owner is active.
    /// </summary>
    public bool IsActive { get; set; }
}
