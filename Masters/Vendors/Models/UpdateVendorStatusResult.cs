namespace veteran_logistic.Masters.Vendors.Models;

/// <summary>
/// Result model for vendor status update operations.
/// </summary>
public sealed class UpdateVendorStatusResult
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
    public static UpdateVendorStatusResult Success()
    {
        return new UpdateVendorStatusResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static UpdateVendorStatusResult Failure(string errorMessage)
    {
        return new UpdateVendorStatusResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
