using veteran_logistic.Masters.Vendors.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Vendors.Contracts;

/// <summary>
/// Contract for validating update vendor requests.
/// </summary>
public interface IUpdateVendorValidator
{
    /// <summary>
    /// Validates an update vendor request.
    /// </summary>
    /// <param name="request">The update vendor request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(UpdateVendorRequest request);
}
