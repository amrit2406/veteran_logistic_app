namespace veteran_logistic.Administration.Companies.Models;

/// <summary>
/// Represents a company item for display in the company listing grid.
/// </summary>
public sealed class CompanyListItem
{
    /// <summary>
    /// Gets or sets the company ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the company code.
    /// </summary>
    public string CompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the company name.
    /// </summary>
    public string CompanyName { get; set; } = string.Empty;

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
    /// Gets or sets whether the company is active.
    /// </summary>
    public bool IsActive { get; set; }
}
