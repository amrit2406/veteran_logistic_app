using veteran_logistic.Administration.Users.Contracts;
using veteran_logistic.Administration.Users.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Administration.Users.Validators;

/// <summary>
/// Validates create user requests to ensure required fields are present and valid.
/// </summary>
public sealed class CreateUserValidator : ICreateUserValidator
{
    private const int MinUsernameLength = 3;
    private const int MaxUsernameLength = 50;

    /// <summary>
    /// Validates a create user request.
    /// </summary>
    /// <param name="request">The create user request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(CreateUserRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(CreateUserRequest), "Create user request cannot be null."));
            return result;
        }

        if (string.IsNullOrWhiteSpace(request.Username))
        {
            result.AddError(new ValidationError(nameof(CreateUserRequest.Username), "Username is required."));
        }
        else if (request.Username.Length < MinUsernameLength)
        {
            result.AddError(new ValidationError(nameof(CreateUserRequest.Username), $"Username must be at least {MinUsernameLength} characters."));
        }
        else if (request.Username.Length > MaxUsernameLength)
        {
            result.AddError(new ValidationError(nameof(CreateUserRequest.Username), $"Username must not exceed {MaxUsernameLength} characters."));
        }

        if (string.IsNullOrWhiteSpace(request.DisplayName))
        {
            result.AddError(new ValidationError(nameof(CreateUserRequest.DisplayName), "Display name is required."));
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            result.AddError(new ValidationError(nameof(CreateUserRequest.Password), "Password is required."));
        }

        if (string.IsNullOrWhiteSpace(request.ConfirmPassword))
        {
            result.AddError(new ValidationError(nameof(CreateUserRequest.ConfirmPassword), "Confirm password is required."));
        }

        if (!string.IsNullOrWhiteSpace(request.Password) && 
            !string.IsNullOrWhiteSpace(request.ConfirmPassword) && 
            request.Password != request.ConfirmPassword)
        {
            result.AddError(new ValidationError(nameof(CreateUserRequest.ConfirmPassword), "Passwords do not match."));
        }

        if (string.IsNullOrWhiteSpace(request.Role))
        {
            result.AddError(new ValidationError(nameof(CreateUserRequest.Role), "Role is required."));
        }

        return result;
    }
}
