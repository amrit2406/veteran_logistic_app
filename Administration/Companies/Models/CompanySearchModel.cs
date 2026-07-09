namespace veteran_logistic.Administration.Companies.Models;

/// <summary>
/// Represents a search model for filtering companies.
/// </summary>
public sealed class CompanySearchModel
{
    /// <summary>
    /// Gets or sets the search text to filter companies by name or code.
    /// </summary>
    public string SearchText { get; set; } = string.Empty;
}
