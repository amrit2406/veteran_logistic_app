namespace veteran_logistic.Masters.Sources.Models;

/// <summary>
/// Data transfer object for displaying source information in a list.
/// </summary>
public sealed class SourceListItem
{
    /// <summary>
    /// Gets or sets the source ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the source code.
    /// </summary>
    public string SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the source name.
    /// </summary>
    public string SourceName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the city.
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the state.
    /// </summary>
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the contact person.
    /// </summary>
    public string ContactPerson { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the phone number.
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the source is active.
    /// </summary>
    public bool IsActive { get; set; }
}
