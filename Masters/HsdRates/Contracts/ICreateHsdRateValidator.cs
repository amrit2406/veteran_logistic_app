using veteran_logistic.Masters.HsdRates.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.HsdRates.Contracts;

/// <summary>
/// Contract for validating create HSD rate requests.
/// </summary>
public interface ICreateHsdRateValidator
{
    /// <summary>
    /// Validates a create HSD rate request.
    /// </summary>
    /// <param name="request">The create HSD rate request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(CreateHsdRateRequest request);
}
