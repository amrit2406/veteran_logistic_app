namespace veteran_logistic.Masters.VehicleAssignments.Models;

/// <summary>
/// Detailed model for vehicle assignment editing.
/// </summary>
public sealed class VehicleAssignmentModel
{
    /// <summary>
    /// Gets or sets the assignment ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the vehicle ID.
    /// </summary>
    public int VehicleId { get; set; }

    /// <summary>
    /// Gets or sets the vehicle number.
    /// </summary>
    public string VehicleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the vehicle type.
    /// </summary>
    public string VehicleType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the vehicle owner ID.
    /// </summary>
    public int VehicleOwnerId { get; set; }

    /// <summary>
    /// Gets or sets the owner PAN type.
    /// </summary>
    public string PANType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the owner PAN number.
    /// </summary>
    public string PANNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the owner first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the owner middle name.
    /// </summary>
    public string MiddleName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the owner last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the owner address.
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the owner mobile number.
    /// </summary>
    public string MobileNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the assign date.
    /// </summary>
    public DateTime AssignDate { get; set; }

    /// <summary>
    /// Gets or sets the release date (nullable).
    /// </summary>
    public DateTime? ReleaseDate { get; set; }

    /// <summary>
    /// Gets or sets whether the assignment is active.
    /// </summary>
    public bool IsActive { get; set; }
}
