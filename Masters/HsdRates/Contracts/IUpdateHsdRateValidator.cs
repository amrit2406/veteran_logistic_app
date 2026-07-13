using veteran_logistic.Masters.HsdRates.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.HsdRates.Contracts;

/// <summary>
/// Contract for validating update HSD rate requests.
/// </summary>
public interface IUpdateHsdRateValidator
{
    /// <summary>
    /// Validates an update HSD rate request.
    /// </summary>
    /// <param name="request">The update HSD rate request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(UpdateHsdRateRequest request);
}
