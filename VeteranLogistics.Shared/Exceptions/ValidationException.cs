using System;
using VeteranLogistics.Shared.Validation;

namespace VeteranLogistics.Shared.Exceptions;

/// <summary>
/// Exception used to represent validation failures.
/// </summary>
public class ValidationException : BusinessException
{
    public ValidationResult ValidationResult { get; }

    public ValidationException(ValidationResult validationResult)
        : base("One or more validation rules failed.")
    {
        ValidationResult = validationResult;
    }

    public ValidationException(ValidationResult validationResult, Exception innerException)
        : base("One or more validation rules failed.", innerException)
    {
        ValidationResult = validationResult;
    }
}
