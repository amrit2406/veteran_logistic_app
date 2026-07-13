using veteran_logistic.Masters.VehicleOwners.Contracts;
using veteran_logistic.Masters.VehicleOwners.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.VehicleOwners.Validators;

/// <summary>
/// Validator for vehicle owner update requests.
/// </summary>
public sealed class UpdateVehicleOwnerValidator : IUpdateVehicleOwnerValidator
{
    private static readonly string[] ValidPANTypes =
    [
        "Individual",
        "AOP",
        "AOP (Trust)",
        "Cooperative",
        "Company Not Public Interested",
        "Firm",
        "HUF"
    ];

    /// <inheritdoc />
    public ValidationResult Validate(UpdateVehicleOwnerRequest request)
    {
        var result = new ValidationResult();

        // Validate Vehicle Owner ID
        if (request.VehicleOwnerId <= 0)
        {
            result.AddError(new ValidationError(nameof(request.VehicleOwnerId), "Vehicle Owner ID must be positive."));
        }

        // Validate PAN Type
        if (string.IsNullOrWhiteSpace(request.PANType))
        {
            result.AddError(new ValidationError(nameof(request.PANType), "PAN Type is required."));
        }
        else if (!ValidPANTypes.Contains(request.PANType))
        {
            result.AddError(new ValidationError(nameof(request.PANType), "PAN Type must be one of: Individual, AOP, AOP (Trust), Cooperative, Company Not Public Interested, Firm, HUF."));
        }

        // Validate PAN Number
        if (string.IsNullOrWhiteSpace(request.PANNumber))
        {
            result.AddError(new ValidationError(nameof(request.PANNumber), "PAN Number is required."));
        }
        else if (request.PANNumber.Length > 10)
        {
            result.AddError(new ValidationError(nameof(request.PANNumber), "PAN Number cannot exceed 10 characters."));
        }

        // Validate First Name
        if (string.IsNullOrWhiteSpace(request.FirstName))
        {
            result.AddError(new ValidationError(nameof(request.FirstName), "First Name is required."));
        }
        else if (request.FirstName.Length > 100)
        {
            result.AddError(new ValidationError(nameof(request.FirstName), "First Name cannot exceed 100 characters."));
        }

        // Validate Middle Name
        if (!string.IsNullOrWhiteSpace(request.MiddleName) && request.MiddleName.Length > 100)
        {
            result.AddError(new ValidationError(nameof(request.MiddleName), "Middle Name cannot exceed 100 characters."));
        }

        // Validate Last Name
        if (!string.IsNullOrWhiteSpace(request.LastName) && request.LastName.Length > 100)
        {
            result.AddError(new ValidationError(nameof(request.LastName), "Last Name cannot exceed 100 characters."));
        }

        // Validate Company Name
        if (!string.IsNullOrWhiteSpace(request.CompanyName) && request.CompanyName.Length > 200)
        {
            result.AddError(new ValidationError(nameof(request.CompanyName), "Company Name cannot exceed 200 characters."));
        }

        // Validate City
        if (string.IsNullOrWhiteSpace(request.City))
        {
            result.AddError(new ValidationError(nameof(request.City), "City is required."));
        }
        else if (request.City.Length > 100)
        {
            result.AddError(new ValidationError(nameof(request.City), "City cannot exceed 100 characters."));
        }

        // Validate State
        if (string.IsNullOrWhiteSpace(request.State))
        {
            result.AddError(new ValidationError(nameof(request.State), "State is required."));
        }
        else if (request.State.Length > 100)
        {
            result.AddError(new ValidationError(nameof(request.State), "State cannot exceed 100 characters."));
        }

        // Validate Address
        if (!string.IsNullOrWhiteSpace(request.Address) && request.Address.Length > 500)
        {
            result.AddError(new ValidationError(nameof(request.Address), "Address cannot exceed 500 characters."));
        }

        // Validate Phone
        if (!string.IsNullOrWhiteSpace(request.Phone) && request.Phone.Length > 20)
        {
            result.AddError(new ValidationError(nameof(request.Phone), "Phone cannot exceed 20 characters."));
        }

        // Validate Mobile
        if (string.IsNullOrWhiteSpace(request.Mobile))
        {
            result.AddError(new ValidationError(nameof(request.Mobile), "Mobile is required."));
        }
        else if (request.Mobile.Length > 20)
        {
            result.AddError(new ValidationError(nameof(request.Mobile), "Mobile cannot exceed 20 characters."));
        }

        // Validate Email
        if (!string.IsNullOrWhiteSpace(request.Email) && request.Email.Length > 150)
        {
            result.AddError(new ValidationError(nameof(request.Email), "Email cannot exceed 150 characters."));
        }

        // Validate Fax
        if (!string.IsNullOrWhiteSpace(request.Fax) && request.Fax.Length > 50)
        {
            result.AddError(new ValidationError(nameof(request.Fax), "Fax cannot exceed 50 characters."));
        }

        return result;
    }
}
