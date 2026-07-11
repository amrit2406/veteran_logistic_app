using veteran_logistic.Masters.Vendors.Contracts;
using veteran_logistic.Masters.Vendors.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Vendors.Validators;

/// <summary>
/// Validator for vendor update requests.
/// </summary>
public sealed class UpdateVendorValidator : IUpdateVendorValidator
{
    private const int MaxVendorNameLength = 200;

    /// <inheritdoc />
    public ValidationResult Validate(UpdateVendorRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdateVendorRequest), "Update vendor request cannot be null."));
            return result;
        }

        if (request.VendorId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateVendorRequest.VendorId), "Vendor ID must be a positive value."));
        }

        if (string.IsNullOrWhiteSpace(request.VendorCode))
        {
            result.AddError(new ValidationError(nameof(UpdateVendorRequest.VendorCode), "Vendor code is required."));
        }

        if (string.IsNullOrWhiteSpace(request.VendorName))
        {
            result.AddError(new ValidationError(nameof(UpdateVendorRequest.VendorName), "Vendor name is required."));
        }
        else if (request.VendorName.Length > MaxVendorNameLength)
        {
            result.AddError(new ValidationError(nameof(UpdateVendorRequest.VendorName), $"Vendor name must not exceed {MaxVendorNameLength} characters."));
        }

        if (string.IsNullOrWhiteSpace(request.GSTNumber))
        {
            result.AddError(new ValidationError(nameof(UpdateVendorRequest.GSTNumber), "GST number is required."));
        }

        return result;
    }
}
