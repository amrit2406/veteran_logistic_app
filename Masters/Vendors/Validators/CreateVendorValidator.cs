using veteran_logistic.Masters.Vendors.Contracts;
using veteran_logistic.Masters.Vendors.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.Vendors.Validators;

/// <summary>
/// Validates create vendor requests to ensure required fields are present and valid.
/// </summary>
public sealed class CreateVendorValidator : ICreateVendorValidator
{
    private const int MinCodeLength = 2;
    private const int MaxCodeLength = 50;
    private const int MaxNameLength = 200;

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

        if (string.IsNullOrWhiteSpace(request.Type))
        {
            result.AddError(new ValidationError(nameof(CreateVendorRequest.Type), "Type is required."));
        }
        else if (request.Type != "Union" && request.Type != "Vendor")
        {
            result.AddError(new ValidationError(nameof(CreateVendorRequest.Type), "Type must be either 'Union' or 'Vendor'."));
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            result.AddError(new ValidationError(nameof(CreateVendorRequest.Name), "Name is required."));
        }
        else if (request.Name.Length > MaxNameLength)
        {
            result.AddError(new ValidationError(nameof(CreateVendorRequest.Name), $"Name must not exceed {MaxNameLength} characters."));
        }

        return result;
    }
}
