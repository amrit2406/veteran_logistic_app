using veteran_logistic.Masters.HsdRates.Contracts;
using veteran_logistic.Masters.HsdRates.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.HsdRates.Validators;

/// <summary>
/// Validator for HSD rate update requests.
/// </summary>
public sealed class UpdateHsdRateValidator : IUpdateHsdRateValidator
{
    /// <inheritdoc />
    public ValidationResult Validate(UpdateHsdRateRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdateHsdRateRequest), "Update HSD rate request cannot be null."));
            return result;
        }

        if (request.HsdRateId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateHsdRateRequest.HsdRateId), "HSD rate ID must be a positive value."));
        }

        if (request.FuelPumpId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateHsdRateRequest.FuelPumpId), "Fuel pump is required."));
        }

        if (request.ApplicableDate == default)
        {
            result.AddError(new ValidationError(nameof(UpdateHsdRateRequest.ApplicableDate), "Applicable date is required."));
        }

        if (request.RatePerLitre <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateHsdRateRequest.RatePerLitre), "Rate per litre must be greater than zero."));
        }

        return result;
    }
}
