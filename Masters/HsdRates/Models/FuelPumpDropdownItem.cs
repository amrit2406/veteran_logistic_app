namespace veteran_logistic.Masters.HsdRates.Models;

/// <summary>
/// Data transfer object for fuel pump dropdown items.
/// </summary>
public sealed class FuelPumpDropdownItem
{
    /// <summary>
    /// Gets or sets the fuel pump ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the fuel pump name.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}
