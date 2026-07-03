namespace veteran_logistic.Authentication.Models;

/// <summary>
/// Represents the result of an authentication attempt.
/// </summary>
public sealed class AuthenticationResult
{
    /// <summary>
    /// Gets or sets a value indicating whether authentication was successful.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Gets or sets the error message if authentication failed.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Creates a successful authentication result.
    /// </summary>
    public static AuthenticationResult Success() => new() { IsSuccess = true };

    /// <summary>
    /// Creates a failed authentication result with an error message.
    /// </summary>
    /// <param name="errorMessage">The error message describing the failure.</param>
    public static AuthenticationResult Failure(string errorMessage) => new() 
    { 
        IsSuccess = false, 
        ErrorMessage = errorMessage 
    };
}
