using veteran_logistic.Masters.Sources.Contracts;
using veteran_logistic.Masters.Sources.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Sources.Validators;

/// <summary>
/// Validator for source status update requests.
/// </summary>
public sealed class UpdateSourceStatusValidator : IUpdateSourceStatusValidator
{
    /// <inheritdoc />
    public ValidationResult Validate(UpdateSourceStatusRequest request, bool currentIsActive)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdateSourceStatusRequest), "Update source status request cannot be null."));
            return result;
        }

        if (request.SourceId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateSourceStatusRequest.SourceId), "Source ID must be a positive value."));
        }

        if (request.IsActive == currentIsActive)
        {
            var message = request.IsActive
                ? "Source is already active."
                : "Source is already inactive.";

            result.AddError(new ValidationError(nameof(UpdateSourceStatusRequest.IsActive), message));
        }

        return result;
    }
}
