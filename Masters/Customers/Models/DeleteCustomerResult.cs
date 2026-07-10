namespace veteran_logistic.Masters.Customers.Models;

/// <summary>
/// Result model for customer deletion operations.
/// </summary>
public sealed class DeleteCustomerResult
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
    public static DeleteCustomerResult Success()
    {
        return new DeleteCustomerResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static DeleteCustomerResult Failure(string errorMessage)
    {
        return new DeleteCustomerResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
