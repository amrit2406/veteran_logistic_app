namespace veteran_logistic.Masters.DORates.Models;

/// <summary>
/// Lightweight lookup item for dropdown selections.
/// </summary>
public sealed class LookupItem
{
    /// <summary>
    /// Gets or sets the ID of the lookup item.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the lookup item.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}
