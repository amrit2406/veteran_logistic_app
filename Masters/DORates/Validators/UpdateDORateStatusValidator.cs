using veteran_logistic.Masters.DORates.Contracts;
using veteran_logistic.Masters.DORates.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.DORates.Validators;

/// <summary>
/// Validator for DO Rate status update requests.
/// </summary>
public sealed class UpdateDORateStatusValidator : IUpdateDORateStatusValidator
{
    /// <inheritdoc />
    public ValidationResult Validate(UpdateDORateStatusRequest request)
    {
        var result = new ValidationResult();

        if (request.DORateId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateDORateStatusRequest.DORateId), "DO Rate ID is required."));
        }

        return result;
    }
}
