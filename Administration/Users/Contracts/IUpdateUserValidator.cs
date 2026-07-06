using veteran_logistic.Administration.Users.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Administration.Users.Contracts;

/// <summary>
/// Contract for validating user update requests.
/// </summary>
public interface IUpdateUserValidator
{
    /// <summary>
    /// Validates the user update request.
    /// </summary>
    /// <param name="request">The update request to validate.</param>
    /// <returns>A validation result indicating success or failure with error details.</returns>
    ValidationResult Validate(UpdateUserRequest request);
}
