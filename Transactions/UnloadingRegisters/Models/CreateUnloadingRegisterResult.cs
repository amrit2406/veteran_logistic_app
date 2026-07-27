namespace veteran_logistic.Transactions.UnloadingRegisters.Models;

/// <summary>
/// Result model for unloading register creation operations.
/// </summary>
public sealed class CreateUnloadingRegisterResult
{
    /// <summary>
    /// Gets or sets the ID of the created unloading register.
    /// </summary>
    public int UnloadingRegisterId { get; set; }

    /// <summary>
    /// Gets or sets the challan number of the created unloading register.
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
    /// <param name="unloadingRegisterId">The ID of the created unloading register.</param>
    /// <param name="challanNumber">The challan number of the created unloading register.</param>
    /// <returns>A successful result.</returns>
    public static CreateUnloadingRegisterResult Success(int unloadingRegisterId, string challanNumber)
    {
        return new CreateUnloadingRegisterResult
        {
            UnloadingRegisterId = unloadingRegisterId,
            ChallanNumber = challanNumber,
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static CreateUnloadingRegisterResult Failure(string errorMessage)
    {
        return new CreateUnloadingRegisterResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
