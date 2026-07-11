namespace veteran_logistic.Masters.FuelPumps.Models;

/// <summary>
/// Data transfer object for displaying fuel pump information in a list.
/// </summary>
public sealed class FuelPumpListItem
{
    /// <summary>
    /// Gets or sets the fuel pump ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the fuel pump name.
    /// </summary>
    public string FuelPumpName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the fuel pump is active.
    /// </summary>
    public bool IsActive { get; set; }
}
