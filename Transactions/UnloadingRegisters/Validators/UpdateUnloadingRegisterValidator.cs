using veteran_logistic.Transactions.UnloadingRegisters.Contracts;
using veteran_logistic.Transactions.UnloadingRegisters.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.UnloadingRegisters.Validators;

/// <summary>
/// Validates update unloading register requests to ensure required fields are present and valid.
/// </summary>
public sealed class UpdateUnloadingRegisterValidator : IUpdateUnloadingRegisterValidator
{
    /// <summary>
    /// Validates an update unloading register request.
    /// </summary>
    /// <param name="request">The update unloading register request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(UpdateUnloadingRegisterRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(UpdateUnloadingRegisterRequest), "Update unloading register request cannot be null."));
            return result;
        }

        // Unloading Register ID must be positive
        if (request.UnloadingRegisterId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateUnloadingRegisterRequest.UnloadingRegisterId), "Unloading register ID must be positive."));
        }

        // If LoadingRegisterId is provided, it must be positive
        if (request.LoadingRegisterId.HasValue && request.LoadingRegisterId.Value <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateUnloadingRegisterRequest.LoadingRegisterId), "Loading register ID must be positive if provided."));
        }

        // Consignor is required
        if (request.ConsignorId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateUnloadingRegisterRequest.ConsignorId), "Consignor is required."));
        }

        // Consignee is required
        if (request.ConsigneeId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateUnloadingRegisterRequest.ConsigneeId), "Consignee is required."));
        }

        // Source is required
        if (request.SourceId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateUnloadingRegisterRequest.SourceId), "Source is required."));
        }

        // Destination is required
        if (request.DestinationId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateUnloadingRegisterRequest.DestinationId), "Destination is required."));
        }

        // Vehicle is required
        if (request.VehicleId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateUnloadingRegisterRequest.VehicleId), "Vehicle is required."));
        }

        // Material is required
        if (request.MaterialId <= 0)
        {
            result.AddError(new ValidationError(nameof(UpdateUnloadingRegisterRequest.MaterialId), "Material is required."));
        }

        // Gross Weight must be positive
        if (request.GrossWeight < 0)
        {
            result.AddError(new ValidationError(nameof(UpdateUnloadingRegisterRequest.GrossWeight), "Gross weight must be zero or positive."));
        }

        // Tare Weight must be positive
        if (request.TareWeight < 0)
        {
            result.AddError(new ValidationError(nameof(UpdateUnloadingRegisterRequest.TareWeight), "Tare weight must be zero or positive."));
        }

        // Rate must be positive
        if (request.Rate < 0)
        {
            result.AddError(new ValidationError(nameof(UpdateUnloadingRegisterRequest.Rate), "Rate must be zero or positive."));
        }

        // Driver Commission must be positive
        if (request.DriverCommission < 0)
        {
            result.AddError(new ValidationError(nameof(UpdateUnloadingRegisterRequest.DriverCommission), "Driver commission must be zero or positive."));
        }

        // Fuel Quantity must be positive
        if (request.FuelQuantity < 0)
        {
            result.AddError(new ValidationError(nameof(UpdateUnloadingRegisterRequest.FuelQuantity), "Fuel quantity must be zero or positive."));
        }

        // Fuel Amount must be positive
        if (request.FuelAmount < 0)
        {
            result.AddError(new ValidationError(nameof(UpdateUnloadingRegisterRequest.FuelAmount), "Fuel amount must be zero or positive."));
        }

        // Fuel Cash must be positive
        if (request.FuelCash < 0)
        {
            result.AddError(new ValidationError(nameof(UpdateUnloadingRegisterRequest.FuelCash), "Fuel cash must be zero or positive."));
        }

        // Fuel Advance must be positive
        if (request.FuelAdvance < 0)
        {
            result.AddError(new ValidationError(nameof(UpdateUnloadingRegisterRequest.FuelAdvance), "Fuel advance must be zero or positive."));
        }

        // Shortage Weight must be positive
        if (request.ShortageWeight < 0)
        {
            result.AddError(new ValidationError(nameof(UpdateUnloadingRegisterRequest.ShortageWeight), "Shortage weight must be zero or positive."));
        }

        // Cash Advance must be positive
        if (request.CashAdvance < 0)
        {
            result.AddError(new ValidationError(nameof(UpdateUnloadingRegisterRequest.CashAdvance), "Cash advance must be zero or positive."));
        }

        // Other Advance must be positive
        if (request.OtherAdvance < 0)
        {
            result.AddError(new ValidationError(nameof(UpdateUnloadingRegisterRequest.OtherAdvance), "Other advance must be zero or positive."));
        }

        return result;
    }
}
