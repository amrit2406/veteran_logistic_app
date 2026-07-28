using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using UnloadingRegisterEntity = VeteranLogistics.Data.Entities.Administration.UnloadingRegister;
using veteran_logistic.Transactions.UnloadingRegisters.Contracts;
using veteran_logistic.Transactions.UnloadingRegisters.Models;

namespace veteran_logistic.Transactions.UnloadingRegisters.Services;

/// <summary>
/// Implementation of the unloading register command service.
/// </summary>
public sealed class UnloadingRegisterCommandService : IUnloadingRegisterCommandService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ICreateUnloadingRegisterValidator _createValidator;
    private readonly IUpdateUnloadingRegisterValidator _updateValidator;
    private readonly IUpdateUnloadingRegisterStatusValidator _updateStatusValidator;
    private readonly IDeleteUnloadingRegisterValidator _deleteValidator;
    private readonly ILogger<UnloadingRegisterCommandService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnloadingRegisterCommandService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="createValidator">The unloading register creation validator.</param>
    /// <param name="updateValidator">The unloading register update validator.</param>
    /// <param name="updateStatusValidator">The unloading register status update validator.</param>
    /// <param name="deleteValidator">The delete unloading register validator.</param>
    /// <param name="logger">The logger.</param>
    public UnloadingRegisterCommandService(
        VeteranLogisticsDbContext dbContext,
        ICreateUnloadingRegisterValidator createValidator,
        IUpdateUnloadingRegisterValidator updateValidator,
        IUpdateUnloadingRegisterStatusValidator updateStatusValidator,
        IDeleteUnloadingRegisterValidator deleteValidator,
        ILogger<UnloadingRegisterCommandService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
        _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
        _updateStatusValidator = updateStatusValidator ?? throw new ArgumentNullException(nameof(updateStatusValidator));
        _deleteValidator = deleteValidator ?? throw new ArgumentNullException(nameof(deleteValidator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<CreateUnloadingRegisterResult> CreateUnloadingRegisterAsync(CreateUnloadingRegisterRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _createValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return CreateUnloadingRegisterResult.Failure(errorMessage);
            }

            // Calculate LoadingWeight
            var loadingWeight = request.GrossWeight - request.TareWeight;
            if (loadingWeight < 0)
            {
                return CreateUnloadingRegisterResult.Failure("Loading weight cannot be negative. Gross weight must be greater than or equal to tare weight.");
            }

            // Calculate UnloadingWeight
            var unloadingWeight = request.GrossWeightUL - request.TareWeightUL;
            if (unloadingWeight < 0)
            {
                return CreateUnloadingRegisterResult.Failure("Unloading weight cannot be negative. Gross weight at unloading must be greater than or equal to tare weight at unloading.");
            }

            // Calculate GrossAmount
            var grossAmount = loadingWeight * request.Rate;

            // Generate Challan Number
            var challanNumber = await GenerateChallanNumberAsync(cancellationToken).ConfigureAwait(false);

            var unloadingRegister = new UnloadingRegisterEntity
            {
                ChallanNumber = challanNumber,
                LoadingRegisterId = request.LoadingRegisterId,
                ConsignorId = request.ConsignorId,
                ConsigneeId = request.ConsigneeId,
                SourceId = request.SourceId,
                DestinationId = request.DestinationId,
                UnloadingDate = request.UnloadingDate,
                TPNumber = request.TPNumber,
                VehicleId = request.VehicleId,
                VehicleType = request.VehicleType,
                UnionVendorId = request.UnionVendorId,
                DriverCommission = request.DriverCommission,
                GrossWeight = request.GrossWeight,
                TareWeight = request.TareWeight,
                LoadingWeight = loadingWeight,
                GrossWeightUL = request.GrossWeightUL,
                TareWeightUL = request.TareWeightUL,
                UnloadingWeight = unloadingWeight,
                ChallanMoney = request.ChallanMoney,
                MaterialId = request.MaterialId,
                Rate = request.Rate,
                GrossAmount = grossAmount,
                VehicleLoadedBy = request.VehicleLoadedBy,
                FuelQuantity = request.FuelQuantity,
                FuelAmount = request.FuelAmount,
                FuelCash = request.FuelCash,
                FuelAdvance = request.FuelAdvance,
                ShortageWeight = request.ShortageWeight,
                CashAdvance = request.CashAdvance,
                PaymentLocationId = request.PaymentLocationId,
                OtherAdvance = request.OtherAdvance,
                OtherAdvanceDate = request.OtherAdvanceDate,
                ThirdParty = request.ThirdParty,
                OwnerId = request.OwnerId,
                OwnerMobile = request.OwnerMobile,
                OwnerAddress = request.OwnerAddress,
                Driver = request.Driver,
                DrivingLicenceNumber = request.DrivingLicenceNumber,
                DriverMobile = request.DriverMobile,
                Notes = request.Notes,
                IsActive = request.IsActive,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System", // TODO: Replace with actual user from session
                ModifiedBy = "System"
            };

            _dbContext.UnloadingRegisters.Add(unloadingRegister);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Unloading register '{ChallanNumber}' created successfully with ID {UnloadingRegisterId}", challanNumber, unloadingRegister.Id);
            return CreateUnloadingRegisterResult.Success(unloadingRegister.Id, challanNumber);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating unloading register");
            var errorMessage = $"Error: {ex.Message}";
            if (ex.InnerException != null)
            {
                errorMessage += $" | Inner: {ex.InnerException.Message}";
            }
            return CreateUnloadingRegisterResult.Failure(errorMessage);
        }
    }

    /// <inheritdoc />
    public async Task<UpdateUnloadingRegisterResult> UpdateUnloadingRegisterAsync(UpdateUnloadingRegisterRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _updateValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdateUnloadingRegisterResult.Failure(errorMessage);
            }

            var unloadingRegister = await _dbContext.UnloadingRegisters
                .FirstOrDefaultAsync(ur => ur.Id == request.UnloadingRegisterId, cancellationToken)
                .ConfigureAwait(false);

            if (unloadingRegister is null)
            {
                return UpdateUnloadingRegisterResult.Failure("Unloading register not found.");
            }

            // Calculate LoadingWeight
            var loadingWeight = request.GrossWeight - request.TareWeight;
            if (loadingWeight < 0)
            {
                return UpdateUnloadingRegisterResult.Failure("Loading weight cannot be negative. Gross weight must be greater than or equal to tare weight.");
            }

            // Calculate UnloadingWeight
            var unloadingWeight = request.GrossWeightUL - request.TareWeightUL;
            if (unloadingWeight < 0)
            {
                return UpdateUnloadingRegisterResult.Failure("Unloading weight cannot be negative. Gross weight at unloading must be greater than or equal to tare weight at unloading.");
            }

            // Calculate GrossAmount
            var grossAmount = loadingWeight * request.Rate;

            unloadingRegister.LoadingRegisterId = request.LoadingRegisterId;
            unloadingRegister.ConsignorId = request.ConsignorId;
            unloadingRegister.ConsigneeId = request.ConsigneeId;
            unloadingRegister.SourceId = request.SourceId;
            unloadingRegister.DestinationId = request.DestinationId;
            unloadingRegister.UnloadingDate = request.UnloadingDate;
            unloadingRegister.TPNumber = request.TPNumber;
            unloadingRegister.VehicleId = request.VehicleId;
            unloadingRegister.VehicleType = request.VehicleType;
            unloadingRegister.UnionVendorId = request.UnionVendorId;
            unloadingRegister.DriverCommission = request.DriverCommission;
            unloadingRegister.GrossWeight = request.GrossWeight;
            unloadingRegister.TareWeight = request.TareWeight;
            unloadingRegister.LoadingWeight = loadingWeight;
            unloadingRegister.GrossWeightUL = request.GrossWeightUL;
            unloadingRegister.TareWeightUL = request.TareWeightUL;
            unloadingRegister.UnloadingWeight = unloadingWeight;
            unloadingRegister.ChallanMoney = request.ChallanMoney;
            unloadingRegister.MaterialId = request.MaterialId;
            unloadingRegister.Rate = request.Rate;
            unloadingRegister.GrossAmount = grossAmount;
            unloadingRegister.VehicleLoadedBy = request.VehicleLoadedBy;
            unloadingRegister.FuelQuantity = request.FuelQuantity;
            unloadingRegister.FuelAmount = request.FuelAmount;
            unloadingRegister.FuelCash = request.FuelCash;
            unloadingRegister.FuelAdvance = request.FuelAdvance;
            unloadingRegister.ShortageWeight = request.ShortageWeight;
            unloadingRegister.CashAdvance = request.CashAdvance;
            unloadingRegister.PaymentLocationId = request.PaymentLocationId;
            unloadingRegister.OtherAdvance = request.OtherAdvance;
            unloadingRegister.OtherAdvanceDate = request.OtherAdvanceDate;
            unloadingRegister.ThirdParty = request.ThirdParty;
            unloadingRegister.OwnerId = request.OwnerId;
            unloadingRegister.OwnerMobile = request.OwnerMobile;
            unloadingRegister.OwnerAddress = request.OwnerAddress;
            unloadingRegister.Driver = request.Driver;
            unloadingRegister.DrivingLicenceNumber = request.DrivingLicenceNumber;
            unloadingRegister.DriverMobile = request.DriverMobile;
            unloadingRegister.Notes = request.Notes;
            unloadingRegister.IsActive = request.IsActive;
            unloadingRegister.ModifiedOn = DateTime.UtcNow;
            unloadingRegister.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Unloading register '{UnloadingRegisterId}' updated successfully", request.UnloadingRegisterId);
            return UpdateUnloadingRegisterResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating unloading register '{UnloadingRegisterId}'", request.UnloadingRegisterId);
            var errorMessage = $"Error: {ex.Message}";
            if (ex.InnerException != null)
            {
                errorMessage += $" | Inner: {ex.InnerException.Message}";
            }
            return UpdateUnloadingRegisterResult.Failure(errorMessage);
        }
    }

    /// <inheritdoc />
    public async Task<UpdateUnloadingRegisterStatusResult> UpdateUnloadingRegisterStatusAsync(UpdateUnloadingRegisterStatusRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var unloadingRegister = await _dbContext.UnloadingRegisters
                .FirstOrDefaultAsync(ur => ur.Id == request.UnloadingRegisterId, cancellationToken)
                .ConfigureAwait(false);

            if (unloadingRegister is null)
            {
                return UpdateUnloadingRegisterStatusResult.Failure("Unloading register not found.");
            }

            var validationResult = _updateStatusValidator.Validate(request, unloadingRegister.IsActive);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdateUnloadingRegisterStatusResult.Failure(errorMessage);
            }

            unloadingRegister.IsActive = request.IsActive;
            unloadingRegister.ModifiedOn = DateTime.UtcNow;
            unloadingRegister.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Unloading register '{UnloadingRegisterId}' status updated to {IsActive}", request.UnloadingRegisterId, request.IsActive);
            return UpdateUnloadingRegisterStatusResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating unloading register status '{UnloadingRegisterId}'", request.UnloadingRegisterId);
            return UpdateUnloadingRegisterStatusResult.Failure("An unexpected error occurred while updating the unloading register status.");
        }
    }

    /// <inheritdoc />
    public async Task<DeleteUnloadingRegisterResult> DeleteUnloadingRegisterAsync(DeleteUnloadingRegisterRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _deleteValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return DeleteUnloadingRegisterResult.Failure(errorMessage);
            }

            var unloadingRegister = await _dbContext.UnloadingRegisters
                .FirstOrDefaultAsync(ur => ur.Id == request.UnloadingRegisterId, cancellationToken)
                .ConfigureAwait(false);

            if (unloadingRegister is null)
            {
                return DeleteUnloadingRegisterResult.Failure("Unloading register not found.");
            }

            unloadingRegister.IsDeleted = true;
            unloadingRegister.DeletedOn = DateTime.UtcNow;
            unloadingRegister.ModifiedOn = DateTime.UtcNow;
            unloadingRegister.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Unloading register '{UnloadingRegisterId}' deleted successfully", request.UnloadingRegisterId);
            return DeleteUnloadingRegisterResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting unloading register '{UnloadingRegisterId}'", request.UnloadingRegisterId);
            return DeleteUnloadingRegisterResult.Failure("An unexpected error occurred while deleting the unloading register.");
        }
    }

    /// <summary>
    /// Generates a unique challan number.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A unique challan number.</returns>
    private async Task<string> GenerateChallanNumberAsync(CancellationToken cancellationToken = default)
    {
        var year = DateTime.UtcNow.Year;
        var prefix = $"CH{year}";

        // Get the maximum existing challan number for the current year
        var maxChallanNumber = await _dbContext.UnloadingRegisters
            .AsNoTracking()
            .Where(ur => ur.ChallanNumber.StartsWith(prefix))
            .Select(ur => ur.ChallanNumber)
            .OrderByDescending(ur => ur)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        int sequenceNumber = 1;

        if (!string.IsNullOrEmpty(maxChallanNumber))
        {
            // Extract the sequence number from the existing challan number
            var sequencePart = maxChallanNumber.Substring(prefix.Length);
            if (int.TryParse(sequencePart, out var existingSequence))
            {
                sequenceNumber = existingSequence + 1;
            }
        }

        return $"{prefix}{sequenceNumber:D6}";
    }
}
