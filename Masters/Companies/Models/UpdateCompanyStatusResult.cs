namespace veteran_logistic.Masters.Companies.Models;

/// <summary>
/// Result model for company status update operations.
/// </summary>
public sealed class UpdateCompanyStatusResult
{
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
    /// <returns>A successful result.</returns>
    public static UpdateCompanyStatusResult Success()
    {
        return new UpdateCompanyStatusResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static UpdateCompanyStatusResult Failure(string errorMessage)
    {
        return new UpdateCompanyStatusResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
