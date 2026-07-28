using veteran_logistic.Transactions.UnloadingRegisters.Contracts;
using veteran_logistic.Transactions.UnloadingRegisters.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.UnloadingRegisters.Validators;

/// <summary>
/// Validates create unloading register requests to ensure required fields are present and valid.
/// </summary>
public sealed class CreateUnloadingRegisterValidator : ICreateUnloadingRegisterValidator
{
    /// <summary>
    /// Validates a create unloading register request.
    /// </summary>
    /// <param name="request">The create unloading register request to validate.</param>
    /// <returns>A ValidationResult indicating whether the request is valid.</returns>
    public ValidationResult Validate(CreateUnloadingRegisterRequest request)
    {
        var result = new ValidationResult();

        if (request is null)
        {
            result.AddError(new ValidationError(nameof(CreateUnloadingRegisterRequest), "Create unloading register request cannot be null."));
            return result;
        }

        // If LoadingRegisterId is provided, it must be positive
        if (request.LoadingRegisterId.HasValue && request.LoadingRegisterId.Value <= 0)
        {
            result.AddError(new ValidationError(nameof(CreateUnloadingRegisterRequest.LoadingRegisterId), "Loading register ID must be positive if provided."));
        }

        // Consignor is required
        if (request.ConsignorId <= 0)
        {
            result.AddError(new ValidationError(nameof(CreateUnloadingRegisterRequest.ConsignorId), "Consignor is required."));
        }

        // Consignee is required
        if (request.ConsigneeId <= 0)
        {
            result.AddError(new ValidationError(nameof(CreateUnloadingRegisterRequest.ConsigneeId), "Consignee is required."));
        }

        // Source is required
        if (request.SourceId <= 0)
        {
            result.AddError(new ValidationError(nameof(CreateUnloadingRegisterRequest.SourceId), "Source is required."));
        }

        // Destination is required
        if (request.DestinationId <= 0)
        {
            result.AddError(new ValidationError(nameof(CreateUnloadingRegisterRequest.DestinationId), "Destination is required."));
        }

        // Vehicle is required
        if (request.VehicleId <= 0)
        {
            result.AddError(new ValidationError(nameof(CreateUnloadingRegisterRequest.VehicleId), "Vehicle is required."));
        }

        // Material is required
        if (request.MaterialId <= 0)
        {
            result.AddError(new ValidationError(nameof(CreateUnloadingRegisterRequest.MaterialId), "Material is required."));
        }

        // Gross Weight must be positive
        if (request.GrossWeight < 0)
        {
            result.AddError(new ValidationError(nameof(CreateUnloadingRegisterRequest.GrossWeight), "Gross weight must be zero or positive."));
        }

        // Tare Weight must be positive
        if (request.TareWeight < 0)
        {
            result.AddError(new ValidationError(nameof(CreateUnloadingRegisterRequest.TareWeight), "Tare weight must be zero or positive."));
        }

        // Gross Weight UL must be positive
        if (request.GrossWeightUL < 0)
        {
            result.AddError(new ValidationError(nameof(CreateUnloadingRegisterRequest.GrossWeightUL), "Gross weight at unloading must be zero or positive."));
        }

        // Tare Weight UL must be positive
        if (request.TareWeightUL < 0)
        {
            result.AddError(new ValidationError(nameof(CreateUnloadingRegisterRequest.TareWeightUL), "Tare weight at unloading must be zero or positive."));
        }

        // Challan Money must be positive
        if (request.ChallanMoney < 0)
        {
            result.AddError(new ValidationError(nameof(CreateUnloadingRegisterRequest.ChallanMoney), "Challan money must be zero or positive."));
        }

        // Rate must be positive
        if (request.Rate < 0)
        {
            result.AddError(new ValidationError(nameof(CreateUnloadingRegisterRequest.Rate), "Rate must be zero or positive."));
        }

        // Driver Commission must be positive
        if (request.DriverCommission < 0)
        {
            result.AddError(new ValidationError(nameof(CreateUnloadingRegisterRequest.DriverCommission), "Driver commission must be zero or positive."));
        }

        // Fuel Quantity must be positive
        if (request.FuelQuantity < 0)
        {
            result.AddError(new ValidationError(nameof(CreateUnloadingRegisterRequest.FuelQuantity), "Fuel quantity must be zero or positive."));
        }

        // Fuel Amount must be positive
        if (request.FuelAmount < 0)
        {
            result.AddError(new ValidationError(nameof(CreateUnloadingRegisterRequest.FuelAmount), "Fuel amount must be zero or positive."));
        }

        // Fuel Cash must be positive
        if (request.FuelCash < 0)
        {
            result.AddError(new ValidationError(nameof(CreateUnloadingRegisterRequest.FuelCash), "Fuel cash must be zero or positive."));
        }

        // Fuel Advance must be positive
        if (request.FuelAdvance < 0)
        {
            result.AddError(new ValidationError(nameof(CreateUnloadingRegisterRequest.FuelAdvance), "Fuel advance must be zero or positive."));
        }

        // Shortage Weight must be positive
        if (request.ShortageWeight < 0)
        {
            result.AddError(new ValidationError(nameof(CreateUnloadingRegisterRequest.ShortageWeight), "Shortage weight must be zero or positive."));
        }

        // Cash Advance must be positive
        if (request.CashAdvance < 0)
        {
            result.AddError(new ValidationError(nameof(CreateUnloadingRegisterRequest.CashAdvance), "Cash advance must be zero or positive."));
        }

        // Other Advance must be positive
        if (request.OtherAdvance < 0)
        {
            result.AddError(new ValidationError(nameof(CreateUnloadingRegisterRequest.OtherAdvance), "Other advance must be zero or positive."));
        }

        return result;
    }
}
