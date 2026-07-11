using veteran_logistic.Masters.Vendors.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Vendors.Contracts;

/// <summary>
/// Contract for validating create vendor requests.
/// </summary>
public interface ICreateVendorValidator
{
    /// <summary>
    /// Validates a create vendor request.
    /// </summary>
    /// <param name="request">The create vendor request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(CreateVendorRequest request);
}
