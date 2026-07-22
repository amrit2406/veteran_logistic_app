namespace veteran_logistic.Transactions.LoadingRegisters.Models;

/// <summary>
/// Result model for loading register update operations.
/// </summary>
public sealed class UpdateLoadingRegisterResult
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
    public static UpdateLoadingRegisterResult Success()
    {
        return new UpdateLoadingRegisterResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static UpdateLoadingRegisterResult Failure(string errorMessage)
    {
        return new UpdateLoadingRegisterResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
