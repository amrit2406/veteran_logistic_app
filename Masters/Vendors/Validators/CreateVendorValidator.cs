using veteran_logistic.Masters.Vendors.Contracts;
using veteran_logistic.Masters.Vendors.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Vendors.Validators;

/// <summary>
/// Validates create vendor requests to ensure required fields are present and valid.
/// </summary>
public sealed class CreateVendorValidator : ICreateVendorValidator
{
    private const int MinVendorCodeLength = 2;
    private const int MaxVendorCodeLength = 50;
    private const int MaxVendorNameLength = 200;

    /// <summary>
    /// Validates a create vendor request.
    /// </summary>
    /// <param name="request">The create vendor request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(CreateVendorRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(CreateVendorRequest), "Create vendor request cannot be null."));
            return result;
        }

        if (string.IsNullOrWhiteSpace(request.VendorCode))
        {
            result.AddError(new ValidationError(nameof(CreateVendorRequest.VendorCode), "Vendor code is required."));
        }
        else if (request.VendorCode.Length < MinVendorCodeLength)
        {
            result.AddError(new ValidationError(nameof(CreateVendorRequest.VendorCode), $"Vendor code must be at least {MinVendorCodeLength} characters."));
        }
        else if (request.VendorCode.Length > MaxVendorCodeLength)
        {
            result.AddError(new ValidationError(nameof(CreateVendorRequest.VendorCode), $"Vendor code must not exceed {MaxVendorCodeLength} characters."));
        }

        if (string.IsNullOrWhiteSpace(request.VendorName))
        {
            result.AddError(new ValidationError(nameof(CreateVendorRequest.VendorName), "Vendor name is required."));
        }
        else if (request.VendorName.Length > MaxVendorNameLength)
        {
            result.AddError(new ValidationError(nameof(CreateVendorRequest.VendorName), $"Vendor name must not exceed {MaxVendorNameLength} characters."));
        }

        if (string.IsNullOrWhiteSpace(request.GSTNumber))
        {
            result.AddError(new ValidationError(nameof(CreateVendorRequest.GSTNumber), "GST number is required."));
        }

        return result;
    }
}
