using System.Collections.Generic;
using VeteranLogistics.Shared.Validation;

namespace VeteranLogistics.Shared.Results;

/// <summary>
/// Non-generic result type for operations that do not return a value.
/// </summary>
public sealed class Result
{
    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Error code associated with the failure. Nullable and optional.
    /// </summary>
    public string? ErrorCode { get; }

    /// <summary>
    /// Error message for failed operations.
    /// </summary>
    public string? ErrorMessage { get; }

    /// <summary>
    /// Validation result containing validation errors when applicable.
    /// </summary>
    public ValidationResult? Validation { get; }

    private Result(bool isSuccess, string? errorCode = null, string? errorMessage = null, ValidationResult? validation = null)
    {
        IsSuccess = isSuccess;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
        Validation = validation;
    }

    /// <summary>
    /// Create a successful result.
    /// </summary>
    public static Result Success() => new Result(true);

    /// <summary>
    /// Create a failed result with an error message and optional error code.
    /// </summary>
    public static Result Failure(string errorMessage, string? errorCode = null) => new Result(false, errorCode, errorMessage);

    /// <summary>
    /// Create a validation failure result.
    /// </summary>
    public static Result ValidationFailure(IEnumerable<ValidationError> errors, string? errorCode = null) => new Result(false, errorCode, null, new ValidationResult(errors));
}
