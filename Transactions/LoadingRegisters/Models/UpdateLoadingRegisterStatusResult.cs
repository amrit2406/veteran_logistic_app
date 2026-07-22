namespace veteran_logistic.Transactions.LoadingRegisters.Models;

/// <summary>
/// Result model for loading register status update operations.
/// </summary>
public sealed class UpdateLoadingRegisterStatusResult
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
    public static UpdateLoadingRegisterStatusResult Success()
    {
        return new UpdateLoadingRegisterStatusResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static UpdateLoadingRegisterStatusResult Failure(string errorMessage)
    {
        return new UpdateLoadingRegisterStatusResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
