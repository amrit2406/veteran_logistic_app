namespace veteran_logistic.Masters.Destinations.Models;

/// <summary>
/// Data transfer object for displaying destination information in a list.
/// </summary>
public sealed class DestinationListItem
{
    /// <summary>
    /// Gets or sets the destination ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the destination code.
    /// </summary>
    public string DestinationCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the destination name.
    /// </summary>
    public string DestinationName { get; set; } = string.Empty;

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
    /// Gets or sets whether the destination is active.
    /// </summary>
    public bool IsActive { get; set; }
}
