namespace veteran_logistic.Masters.Vehicles.Models;

/// <summary>
/// Represents a vehicle item for display in the vehicle listing grid.
/// </summary>
public sealed class VehicleListItem
{
    /// <summary>
    /// Gets or sets the vehicle ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the vehicle number.
    /// </summary>
    public string VehicleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the vehicle type.
    /// </summary>
    public string VehicleType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the owner code.
    /// </summary>
    public string OwnerCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the owner name.
    /// </summary>
    public string OwnerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the vehicle is active.
    /// </summary>
    public bool IsActive { get; set; }
}
