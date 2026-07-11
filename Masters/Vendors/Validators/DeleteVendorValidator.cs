using veteran_logistic.Masters.Vendors.Contracts;
using veteran_logistic.Masters.Vendors.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Vendors.Validators;

/// <summary>
/// Validates delete vendor requests to ensure required fields are present and valid.
/// </summary>
public sealed class DeleteVendorValidator : IDeleteVendorValidator
{
    /// <summary>
    /// Validates a delete vendor request.
    /// </summary>
    /// <param name="request">The delete vendor request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(DeleteVendorRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(DeleteVendorRequest), "Delete vendor request cannot be null."));
            return result;
        }

        if (request.VendorId <= 0)
        {
            result.AddError(new ValidationError(nameof(DeleteVendorRequest.VendorId), "Vendor ID must be a positive value."));
        }

        return result;
    }
}
