using veteran_logistic.Masters.DORates.Contracts;
using veteran_logistic.Masters.DORates.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.DORates.Validators;

/// <summary>
/// Validator for DO Rate update requests.
/// </summary>
public sealed class UpdateDORateValidator : IUpdateDORateValidator
{
    /// <inheritdoc />
    public ValidationResult Validate(UpdateDORateRequest request)
    {
        var result = new ValidationResult();

        // ID validation
        if (request.DORateId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateDORateRequest.DORateId), "DO Rate ID is required."));
        }

        // Consignor validation
        if (request.ConsignorId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateDORateRequest.ConsignorId), "Consignor is required."));
        }

        // Consignee validation
        if (request.ConsigneeId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateDORateRequest.ConsigneeId), "Consignee is required."));
        }

        // Source validation
        if (request.SourceId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateDORateRequest.SourceId), "Source is required."));
        }

        // Destination validation
        if (request.DestinationId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateDORateRequest.DestinationId), "Destination is required."));
        }

        // Effective Date validation
        if (request.EffectiveDate == default)
        {
            result.AddError(new ValidationError(nameof(UpdateDORateRequest.EffectiveDate), "Effective Date is required."));
        }

        // Freight Rate validation
        if (request.FreightRate < 0)
        {
            result.AddError(new ValidationError(nameof(UpdateDORateRequest.FreightRate), "Freight Rate must be greater than or equal to 0."));
        }

        // Union Rate validation
        if (request.UnionRate < 0)
        {
            result.AddError(new ValidationError(nameof(UpdateDORateRequest.UnionRate), "Union Rate must be greater than or equal to 0."));
        }

        // Vendor Rate validation
        if (request.VendorRate < 0)
        {
            result.AddError(new ValidationError(nameof(UpdateDORateRequest.VendorRate), "Vendor Rate must be greater than or equal to 0."));
        }

        // DO Number validation
        if (string.IsNullOrWhiteSpace(request.DONumber))
        {
            result.AddError(new ValidationError(nameof(UpdateDORateRequest.DONumber), "DO Number is required."));
        }
        else if (request.DONumber.Length > 50)
        {
            result.AddError(new ValidationError(nameof(UpdateDORateRequest.DONumber), "DO Number cannot exceed 50 characters."));
        }

        // Billing Rate validation
        if (request.BillingRate < 0)
        {
            result.AddError(new ValidationError(nameof(UpdateDORateRequest.BillingRate), "Billing Rate must be greater than or equal to 0."));
        }

        // Allowed Shortage validation
        if (request.AllowedShortage < 0)
        {
            result.AddError(new ValidationError(nameof(UpdateDORateRequest.AllowedShortage), "Allowed Shortage must be greater than or equal to 0."));
        }

        // Rate Per Kg validation
        if (request.RatePerKg < 0)
        {
            result.AddError(new ValidationError(nameof(UpdateDORateRequest.RatePerKg), "Rate Per Kg must be greater than or equal to 0."));
        }

        // Vessel Name validation
        if (!string.IsNullOrWhiteSpace(request.VesselName) && request.VesselName.Length > 200)
        {
            result.AddError(new ValidationError(nameof(UpdateDORateRequest.VesselName), "Vessel Name cannot exceed 200 characters."));
        }

        // Trader Name validation
        if (!string.IsNullOrWhiteSpace(request.TraderName) && request.TraderName.Length > 200)
        {
            result.AddError(new ValidationError(nameof(UpdateDORateRequest.TraderName), "Trader Name cannot exceed 200 characters."));
        }

        // Narration validation
        if (!string.IsNullOrWhiteSpace(request.Narration) && request.Narration.Length > 1000)
        {
            result.AddError(new ValidationError(nameof(UpdateDORateRequest.Narration), "Narration cannot exceed 1000 characters."));
        }

        return result;
    }
}
