namespace veteran_logistic.Masters.Companies.Models;

/// <summary>
/// Request model for updating a company's active status.
/// </summary>
public sealed class UpdateCompanyStatusRequest
{
    /// <summary>
    /// Gets or sets the company ID.
    /// </summary>
    public int CompanyId { get; set; }

    /// <summary>
    /// Gets or sets the new active status.
    /// </summary>
    public bool IsActive { get; set; }
}
