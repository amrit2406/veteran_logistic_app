using veteran_logistic.Transactions.LoadingRegisters.Contracts;
using veteran_logistic.Transactions.LoadingRegisters.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.LoadingRegisters.Validators;

/// <summary>
/// Validates update loading register requests to ensure required fields are present and valid.
/// </summary>
public sealed class UpdateLoadingRegisterValidator : IUpdateLoadingRegisterValidator
{
    /// <summary>
    /// Validates an update loading register request.
    /// </summary>
    /// <param name="request">The update loading register request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(UpdateLoadingRegisterRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdateLoadingRegisterRequest), "Update loading register request cannot be null."));
            return result;
        }

        // Loading Register ID must be positive
        if (request.LoadingRegisterId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateLoadingRegisterRequest.LoadingRegisterId), "Loading register ID must be positive."));
        }

        // Loading Date is required
        if (request.LoadingDate == default)
        {
            result.AddError(new ValidationError(nameof(UpdateLoadingRegisterRequest.LoadingDate), "Loading date is required."));
        }

        // TP Number is required
        if (string.IsNullOrWhiteSpace(request.TPNumber))
        {
            result.AddError(new ValidationError(nameof(UpdateLoadingRegisterRequest.TPNumber), "TP number is required."));
        }

        // Vehicle Type is required
        if (string.IsNullOrWhiteSpace(request.VehicleType))
        {
            result.AddError(new ValidationError(nameof(UpdateLoadingRegisterRequest.VehicleType), "Vehicle type is required."));
        }

        // Gross Weight must be positive
        if (request.GrossWeight < 0)
        {
            result.AddError(new ValidationError(nameof(UpdateLoadingRegisterRequest.GrossWeight), "Gross weight must be zero or positive."));
        }

        // Tare Weight must be positive
        if (request.TareWeight < 0)
        {
            result.AddError(new ValidationError(nameof(UpdateLoadingRegisterRequest.TareWeight), "Tare weight must be zero or positive."));
        }

        // Rate must be positive
        if (request.Rate < 0)
        {
            result.AddError(new ValidationError(nameof(UpdateLoadingRegisterRequest.Rate), "Rate must be zero or positive."));
        }

        // Driver Commission must be positive
        if (request.DriverCommission < 0)
        {
            result.AddError(new ValidationError(nameof(UpdateLoadingRegisterRequest.DriverCommission), "Driver commission must be zero or positive."));
        }

        // Fuel Quantity must be positive
        if (request.FuelQuantity < 0)
        {
            result.AddError(new ValidationError(nameof(UpdateLoadingRegisterRequest.FuelQuantity), "Fuel quantity must be zero or positive."));
        }

        // Fuel Amount must be positive
        if (request.FuelAmount < 0)
        {
            result.AddError(new ValidationError(nameof(UpdateLoadingRegisterRequest.FuelAmount), "Fuel amount must be zero or positive."));
        }

        // Fuel Cash must be positive
        if (request.FuelCash < 0)
        {
            result.AddError(new ValidationError(nameof(UpdateLoadingRegisterRequest.FuelCash), "Fuel cash must be zero or positive."));
        }

        // Fuel Advance must be positive
        if (request.FuelAdvance < 0)
        {
            result.AddError(new ValidationError(nameof(UpdateLoadingRegisterRequest.FuelAdvance), "Fuel advance must be zero or positive."));
        }

        // Shortage Weight must be positive
        if (request.ShortageWeight < 0)
        {
            result.AddError(new ValidationError(nameof(UpdateLoadingRegisterRequest.ShortageWeight), "Shortage weight must be zero or positive."));
        }

        // Cash Advance must be positive
        if (request.CashAdvance < 0)
        {
            result.AddError(new ValidationError(nameof(UpdateLoadingRegisterRequest.CashAdvance), "Cash advance must be zero or positive."));
        }

        // Other Advance must be positive
        if (request.OtherAdvance < 0)
        {
            result.AddError(new ValidationError(nameof(UpdateLoadingRegisterRequest.OtherAdvance), "Other advance must be zero or positive."));
        }

        // Driver name is required
        if (string.IsNullOrWhiteSpace(request.Driver))
        {
            result.AddError(new ValidationError(nameof(UpdateLoadingRegisterRequest.Driver), "Driver name is required."));
        }

        return result;
    }
}
