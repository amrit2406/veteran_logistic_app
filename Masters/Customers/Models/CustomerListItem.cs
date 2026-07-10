namespace veteran_logistic.Masters.Customers.Models;

/// <summary>
/// Represents a customer item for display in the customer listing grid.
/// </summary>
public sealed class CustomerListItem
{
    /// <summary>
    /// Gets or sets the customer ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the customer code.
    /// </summary>
    public string CustomerCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer name.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the GST number.
    /// </summary>
    public string GSTNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the PAN number.
    /// </summary>
    public string PANNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the city.
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the state.
    /// </summary>
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the customer is active.
    /// </summary>
    public bool IsActive { get; set; }
}
