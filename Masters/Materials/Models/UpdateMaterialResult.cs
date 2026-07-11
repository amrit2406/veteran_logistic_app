namespace veteran_logistic.Masters.Materials.Models;

/// <summary>
/// Result model for material update operations.
/// </summary>
public sealed class UpdateMaterialResult
{
    /// <summary>
    /// Gets or sets a value indicating whether the operation was successful.
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
    public static UpdateMaterialResult Success()
    {
        return new UpdateMaterialResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failed result.</returns>
    public static UpdateMaterialResult Failure(string errorMessage)
    {
        return new UpdateMaterialResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
