namespace veteran_logistic.Transactions.LoadingRegisters.Models;

/// <summary>
/// Result model for loading register creation operations.
/// </summary>
public sealed class CreateLoadingRegisterResult
{
    /// <summary>
    /// Gets or sets the ID of the created loading register.
    /// </summary>
    public int LoadingRegisterId { get; set; }

    /// <summary>
    /// Gets or sets the challan number of the created loading register.
    /// </summary>
    public string ChallanNumber { get; set; } = string.Empty;

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
    /// <param name="loadingRegisterId">The ID of the created loading register.</param>
    /// <param name="challanNumber">The challan number of the created loading register.</param>
    /// <returns>A successful result.</returns>
    public static CreateLoadingRegisterResult Success(int loadingRegisterId, string challanNumber)
    {
        return new CreateLoadingRegisterResult
        {
            LoadingRegisterId = loadingRegisterId,
            ChallanNumber = challanNumber,
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static CreateLoadingRegisterResult Failure(string errorMessage)
    {
        return new CreateLoadingRegisterResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
