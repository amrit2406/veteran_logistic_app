namespace veteran_logistic.Masters.Customers.Models;

/// <summary>
/// Result model for customer update operations.
/// </summary>
public sealed class UpdateCustomerResult
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
    public static UpdateCustomerResult Success()
    {
        return new UpdateCustomerResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static UpdateCustomerResult Failure(string errorMessage)
    {
        return new UpdateCustomerResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
