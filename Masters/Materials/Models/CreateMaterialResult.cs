namespace veteran_logistic.Masters.Materials.Models;

/// <summary>
/// Result model for material creation operations.
/// </summary>
public sealed class CreateMaterialResult
{
    /// <summary>
    /// Gets or sets the ID of the created material.
    /// </summary>
    public int MaterialId { get; set; }

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
    /// <param name="materialId">The ID of the created material.</param>
    /// <returns>A successful result.</returns>
    public static CreateMaterialResult Success(int materialId)
    {
        return new CreateMaterialResult
        {
            MaterialId = materialId,
            IsSuccess = true
        };
    }

    /// <summary>
    /// Creates a failed result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <returns>A failed result.</returns>
    public static CreateMaterialResult Failure(string errorMessage)
    {
        return new CreateMaterialResult
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
