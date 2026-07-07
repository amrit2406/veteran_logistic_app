using veteran_logistic.Administration.Users.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Administration.Users.Contracts;

/// <summary>
/// Contract for validating delete user requests.
/// </summary>
public interface IDeleteUserValidator
{
    /// <summary>
    /// Validates a delete user request.
    /// </summary>
    /// <param name="request">The delete user request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(DeleteUserRequest request);
}
