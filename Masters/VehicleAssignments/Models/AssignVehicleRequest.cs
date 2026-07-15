namespace veteran_logistic.Masters.VehicleAssignments.Models;

/// <summary>
/// Request model for assigning a vehicle to an owner.
/// </summary>
public sealed class AssignVehicleRequest
{
    /// <summary>
    /// Gets or sets the vehicle ID.
    /// </summary>
    public int VehicleId { get; set; }

    /// <summary>
    /// Gets or sets the owner PAN type.
    /// </summary>
    public string OwnerPanType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the owner PAN number.
    /// </summary>
    public string OwnerPanNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the owner first name.
    /// </summary>
    public string OwnerFirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the owner middle name.
    /// </summary>
    public string OwnerMiddleName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the owner last name.
    /// </summary>
    public string OwnerLastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the owner address.
    /// </summary>
    public string OwnerAddress { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the owner mobile number.
    /// </summary>
    public string OwnerMobileNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the assign date.
    /// </summary>
    public DateTime AssignDate { get; set; }
}
