namespace veteran_logistic.Masters.Companies.Models;

/// <summary>
/// Result model for company update operations.
/// </summary>
public sealed class UpdateCompanyResult
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
    public static UpdateCompanyResult Success()
    {
        return new UpdateCompanyResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static UpdateCompanyResult Failure(string errorMessage)
    {
        return new UpdateCompanyResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
