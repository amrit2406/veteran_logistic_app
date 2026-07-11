using veteran_logistic.Masters.Vendors.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Vendors.Contracts;

/// <summary>
/// Contract for validating vendor status update requests.
/// </summary>
public interface IUpdateVendorStatusValidator
{
    /// <summary>
    /// Validates the vendor status update request.
    /// </summary>
    /// <param name="request">The status update request to validate.</param>
    /// <param name="currentIsActive">The current active status of the vendor.</param>
    /// <returns>A validation result indicating success or failure with error details.</returns>
    ValidationResult Validate(UpdateVendorStatusRequest request, bool currentIsActive);
}
