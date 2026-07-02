using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VeteranLogistics.Shared.Validation;

/// <summary>
/// Represents the result of a validation operation. The collection of errors is exposed as a read-only collection.
/// </summary>
public sealed class ValidationResult
{
    private readonly List<ValidationError> _errors = new();

    /// <summary>
    /// Read-only view of validation errors.
    /// </summary>
    public ReadOnlyCollection<ValidationError> Errors => _errors.AsReadOnly();

    /// <summary>
    /// True when there are no validation errors.
    /// </summary>
    public bool IsValid => _errors.Count == 0;

    /// <summary>
    /// Creates a new ValidationResult optionally initialized with errors.
    /// </summary>
    public ValidationResult(IEnumerable<ValidationError>? errors = null)
    {
        if (errors is not null)
        {
            _errors.AddRange(errors);
        }
    }

    /// <summary>
    /// Adds an error to the result. Internal to the assembly to prevent external mutation.
    /// </summary>
    internal void AddError(ValidationError error)
    {
        _errors.Add(error);
    }
}
