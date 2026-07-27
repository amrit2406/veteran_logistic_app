namespace veteran_logistic.Transactions.UnloadingRegisters.Models;

/// <summary>
/// Result model for unloading register delete operations.
/// </summary>
public sealed class DeleteUnloadingRegisterResult
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
    public static DeleteUnloadingRegisterResult Success()
    {
        return new DeleteUnloadingRegisterResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static DeleteUnloadingRegisterResult Failure(string errorMessage)
    {
        return new DeleteUnloadingRegisterResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
