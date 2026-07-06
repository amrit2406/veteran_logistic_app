using veteran_logistic.Administration.Users.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Administration.Users.Contracts;

/// <summary>
/// Contract for validating create user requests.
/// </summary>
public interface ICreateUserValidator
{
    /// <summary>
    /// Validates a create user request.
    /// </summary>
    /// <param name="request">The create user request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(CreateUserRequest request);
}
