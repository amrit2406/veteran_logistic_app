using System;
using VeteranLogistics.Shared.Validation;

namespace VeteranLogistics.Shared.Results;

/// <summary>
/// Generic result type that carries a value on success.
/// </summary>
/// <typeparam name="T">Value type.</typeparam>
public sealed class Result<T>
{
    /// <summary>
    /// True when operation succeeded.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Value when successful.
    /// </summary>
    public T? Value { get; }

    /// <summary>
    /// Error code associated with the failure. Nullable and optional.
    /// </summary>
    public string? ErrorCode { get; }

    /// <summary>
    /// Error message when failed.
    /// </summary>
    public string? ErrorMessage { get; }

    /// <summary>
    /// Validation result when validation failed.
    /// </summary>
    public ValidationResult? Validation { get; }

    private Result(bool isSuccess, T? value = default, string? errorCode = null, string? errorMessage = null, ValidationResult? validation = null)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
        Validation = validation;
    }

    /// <summary>
    /// Create a successful result with value.
    /// </summary>
    public static Result<T> Success(T value) => new Result<T>(true, value);

    /// <summary>
    /// Create a failed result with message and optional error code.
    /// </summary>
    public static Result<T> Failure(string errorMessage, string? errorCode = null) => new Result<T>(false, default, errorCode, errorMessage);

    /// <summary>
    /// Create a validation failure result.
    /// </summary>
    public static Result<T> ValidationFailure(ValidationResult validation, string? errorCode = null) => new Result<T>(false, default, errorCode, null, validation);
}
