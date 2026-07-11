using veteran_logistic.Masters.Vendors.Contracts;
using veteran_logistic.Masters.Vendors.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Vendors.Validators;

/// <summary>
/// Validator for vendor status update requests.
/// </summary>
public sealed class UpdateVendorStatusValidator : IUpdateVendorStatusValidator
{
    /// <inheritdoc />
    public ValidationResult Validate(UpdateVendorStatusRequest request, bool currentIsActive)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdateVendorStatusRequest), "Update vendor status request cannot be null."));
            return result;
        }

        if (request.VendorId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateVendorStatusRequest.VendorId), "Vendor ID must be a positive value."));
        }

        if (request.IsActive == currentIsActive)
        {
            var message = request.IsActive
                ? "Vendor is already active."
                : "Vendor is already inactive.";

            result.AddError(new ValidationError(nameof(UpdateVendorStatusRequest.IsActive), message));
        }

        return result;
    }
}
