namespace veteran_logistic.Masters.Companies.Models;

/// <summary>
/// Result model for company creation operations.
/// </summary>
public sealed class CreateCompanyResult
{
    /// <summary>
    /// Gets or sets the ID of the created company.
    /// </summary>
    public int CompanyId { get; set; }

    /// <summary>
    /// Gets or sets whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets the error message if the operation failed.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Creates a successful result.
    /// </summary>
    /// <param name="companyId">The ID of the created company.</param>
    /// <returns>A successful result.</returns>
    public static CreateCompanyResult Success(int companyId)
    {
        return new CreateCompanyResult
        {
            CompanyId = companyId,
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static CreateCompanyResult Failure(string errorMessage)
    {
        return new CreateCompanyResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
