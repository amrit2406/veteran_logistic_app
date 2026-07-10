namespace veteran_logistic.Masters.Companies.Models;

/// <summary>
/// Request model for deleting a company.
/// </summary>
public sealed class DeleteCompanyRequest
{
    /// <summary>
    /// Gets or sets the company ID.
    /// </summary>
    public int CompanyId { get; set; }
}
