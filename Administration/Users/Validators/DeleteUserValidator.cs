using veteran_logistic.Administration.Users.Contracts;
using veteran_logistic.Administration.Users.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Administration.Users.Validators;

/// <summary>
/// Validates delete user requests to ensure required fields are present and valid.
/// </summary>
public sealed class DeleteUserValidator : IDeleteUserValidator
{
    /// <summary>
    /// Validates a delete user request.
    /// </summary>
    /// <param name="request">The delete user request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(DeleteUserRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(DeleteUserRequest), "Delete user request cannot be null."));
            return result;
        }

        if (request.UserId <= 0)
        {
            result.AddError(new ValidationError(nameof(DeleteUserRequest.UserId), "User ID must be a positive value."));
        }

        return result;
    }
}
