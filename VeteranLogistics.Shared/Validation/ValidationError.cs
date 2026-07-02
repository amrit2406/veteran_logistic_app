namespace VeteranLogistics.Shared.Validation;

/// <summary>
/// Represents a single validation error for a property.
/// </summary>
public sealed class ValidationError
{
    /// <summary>
    /// Name of the property that failed validation.
    /// </summary>
    public string PropertyName { get; }

    /// <summary>
    /// Human-readable error message.
    /// </summary>
    public string ErrorMessage { get; }

    /// <summary>
    /// Creates a new ValidationError for the specified property and message.
    /// </summary>
    public ValidationError(string propertyName, string errorMessage)
    {
        PropertyName = propertyName;
        ErrorMessage = errorMessage;
    }
}
