using veteran_logistic.Masters.HsdRates.Contracts;
using veteran_logistic.Masters.HsdRates.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.HsdRates.Validators;

/// <summary>
/// Validator for HSD rate status update requests.
/// </summary>
public sealed class UpdateHsdRateStatusValidator : IUpdateHsdRateStatusValidator
{
    /// <inheritdoc />
    public ValidationResult Validate(UpdateHsdRateStatusRequest request, bool currentIsActive)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdateHsdRateStatusRequest), "Update HSD rate status request cannot be null."));
            return result;
        }

        if (request.HsdRateId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateHsdRateStatusRequest.HsdRateId), "HSD rate ID must be a positive value."));
        }

        if (request.IsActive == currentIsActive)
        {
            var message = request.IsActive
                ? "HSD rate is already active."
                : "HSD rate is already inactive.";

            result.AddError(new ValidationError(nameof(UpdateHsdRateStatusRequest.IsActive), message));
        }

        return result;
    }
}
