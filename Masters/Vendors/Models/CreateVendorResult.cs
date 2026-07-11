namespace veteran_logistic.Masters.Vendors.Models;

/// <summary>
/// Result model for vendor creation operations.
/// </summary>
public sealed class CreateVendorResult
{
    /// <summary>
    /// Gets or sets the ID of the created vendor.
    /// </summary>
    public int VendorId { get; set; }

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
    /// <param name="vendorId">The ID of the created vendor.</param>
    /// <returns>A successful result.</returns>
    public static CreateVendorResult Success(int vendorId)
    {
        return new CreateVendorResult
        {
            VendorId = vendorId,
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failure result.</returns>
    public static CreateVendorResult Failure(string errorMessage)
    {
        return new CreateVendorResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
