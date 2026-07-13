using veteran_logistic.Masters.HsdRates.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.HsdRates.Contracts;

/// <summary>
/// Contract for validating delete HSD rate requests.
/// </summary>
public interface IDeleteHsdRateValidator
{
    /// <summary>
    /// Validates a delete HSD rate request.
    /// </summary>
    /// <param name="request">The delete HSD rate request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(DeleteHsdRateRequest request);
}
