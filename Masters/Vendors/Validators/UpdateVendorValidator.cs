using veteran_logistic.Masters.Vendors.Contracts;
using veteran_logistic.Masters.Vendors.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Vendors.Validators;

/// <summary>
/// Validator for vendor update requests.
/// </summary>
public sealed class UpdateVendorValidator : IUpdateVendorValidator
{
    private const int MaxNameLength = 200;

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

        if (string.IsNullOrWhiteSpace(request.Type))
        {
            result.AddError(new ValidationError(nameof(UpdateVendorRequest.Type), "Type is required."));
        }
        else if (request.Type != "Union" && request.Type != "Vendor")
        {
            result.AddError(new ValidationError(nameof(UpdateVendorRequest.Type), "Type must be either 'Union' or 'Vendor'."));
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            result.AddError(new ValidationError(nameof(UpdateVendorRequest.Name), "Name is required."));
        }
        else if (request.Name.Length > MaxNameLength)
        {
            result.AddError(new ValidationError(nameof(UpdateVendorRequest.Name), $"Name must not exceed {MaxNameLength} characters."));
        }

        return result;
    }
}
