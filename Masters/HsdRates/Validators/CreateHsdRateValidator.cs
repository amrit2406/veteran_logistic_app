using veteran_logistic.Masters.HsdRates.Contracts;
using veteran_logistic.Masters.HsdRates.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.HsdRates.Validators;

/// <summary>
/// Validates create HSD rate requests to ensure required fields are present and valid.
/// </summary>
public sealed class CreateHsdRateValidator : ICreateHsdRateValidator
{
    /// <summary>
    /// Validates a create HSD rate request.
    /// </summary>
    /// <param name="request">The create HSD rate request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(CreateHsdRateRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(CreateHsdRateRequest), "Create HSD rate request cannot be null."));
            return result;
        }

        if (request.FuelPumpId <= 0)
        {
            result.AddError(new ValidationError(nameof(CreateHsdRateRequest.FuelPumpId), "Fuel pump is required."));
        }

        if (request.ApplicableDate == default)
        {
            result.AddError(new ValidationError(nameof(CreateHsdRateRequest.ApplicableDate), "Applicable date is required."));
        }

        if (request.RatePerLitre <= 0)
        {
            result.AddError(new ValidationError(nameof(CreateHsdRateRequest.RatePerLitre), "Rate per litre must be greater than zero."));
        }

        return result;
    }
}
