namespace veteran_logistic.Masters.Customers.Models;

/// <summary>
/// Result model for customer creation operations.
/// </summary>
public sealed class CreateCustomerResult
{
    /// <summary>
    /// Gets or sets the ID of the created customer.
    /// </summary>
    public int CustomerId { get; set; }

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
    /// <param name="customerId">The ID of the created customer.</param>
    /// <returns>A successful result.</returns>
    public static CreateCustomerResult Success(int customerId)
    {
        return new CreateCustomerResult
        {
            CustomerId = customerId,
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static CreateCustomerResult Failure(string errorMessage)
    {
        return new CreateCustomerResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
