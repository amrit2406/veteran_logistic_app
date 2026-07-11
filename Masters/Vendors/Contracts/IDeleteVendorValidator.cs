using veteran_logistic.Masters.Vendors.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Vendors.Contracts;

/// <summary>
/// Contract for validating delete vendor requests.
/// </summary>
public interface IDeleteVendorValidator
{
    /// <summary>
    /// Validates a delete vendor request.
    /// </summary>
    /// <param name="request">The delete vendor request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    ValidationResult Validate(DeleteVendorRequest request);
}
