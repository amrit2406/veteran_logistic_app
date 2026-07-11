namespace veteran_logistic.Masters.FuelPumps.Models;

/// <summary>
/// Data transfer object for detailed fuel pump information, used for editing.
/// </summary>
public sealed class FuelPumpModel
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
