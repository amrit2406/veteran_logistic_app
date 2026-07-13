using veteran_logistic.Masters.HsdRates.Contracts;
using veteran_logistic.Masters.HsdRates.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.HsdRates.Validators;

/// <summary>
/// Validates delete HSD rate requests to ensure required fields are present and valid.
/// </summary>
public sealed class DeleteHsdRateValidator : IDeleteHsdRateValidator
{
    /// <summary>
    /// Validates a delete HSD rate request.
    /// </summary>
    /// <param name="request">The delete HSD rate request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(DeleteHsdRateRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(DeleteHsdRateRequest), "Delete HSD rate request cannot be null."));
            return result;
        }

        if (request.HsdRateId <= 0)
        {
            result.AddError(new ValidationError(nameof(DeleteHsdRateRequest.HsdRateId), "HSD rate ID must be a positive value."));
        }

        return result;
    }
}
