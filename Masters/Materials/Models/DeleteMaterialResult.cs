namespace veteran_logistic.Masters.Materials.Models;

/// <summary>
/// Result model for material delete operations.
/// </summary>
public sealed class DeleteMaterialResult
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
    public static DeleteMaterialResult Success()
    {
        return new DeleteMaterialResult
        {
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failed result.</returns>
    public static DeleteMaterialResult Failure(string errorMessage)
    {
        return new DeleteMaterialResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
