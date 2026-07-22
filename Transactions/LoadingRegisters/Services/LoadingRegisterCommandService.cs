using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using LoadingRegisterEntity = VeteranLogistics.Data.Entities.Administration.LoadingRegister;
using veteran_logistic.Transactions.LoadingRegisters.Contracts;
using veteran_logistic.Transactions.LoadingRegisters.Models;

namespace veteran_logistic.Transactions.LoadingRegisters.Services;

/// <summary>
/// Implementation of the loading register command service.
/// </summary>
public sealed class LoadingRegisterCommandService : ILoadingRegisterCommandService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ICreateLoadingRegisterValidator _createValidator;
    private readonly IUpdateLoadingRegisterValidator _updateValidator;
    private readonly IUpdateLoadingRegisterStatusValidator _updateStatusValidator;
    private readonly IDeleteLoadingRegisterValidator _deleteValidator;
    private readonly ILogger<LoadingRegisterCommandService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoadingRegisterCommandService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="createValidator">The loading register creation validator.</param>
    /// <param name="updateValidator">The loading register update validator.</param>
    /// <param name="updateStatusValidator">The loading register status update validator.</param>
    /// <param name="deleteValidator">The delete loading register validator.</param>
    /// <param name="logger">The logger.</param>
    public LoadingRegisterCommandService(
        VeteranLogisticsDbContext dbContext,
        ICreateLoadingRegisterValidator createValidator,
        IUpdateLoadingRegisterValidator updateValidator,
        IUpdateLoadingRegisterStatusValidator updateStatusValidator,
        IDeleteLoadingRegisterValidator deleteValidator,
        ILogger<LoadingRegisterCommandService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
        _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
        _updateStatusValidator = updateStatusValidator ?? throw new ArgumentNullException(nameof(updateStatusValidator));
        _deleteValidator = deleteValidator ?? throw new ArgumentNullException(nameof(deleteValidator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<CreateLoadingRegisterResult> CreateLoadingRegisterAsync(CreateLoadingRegisterRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _createValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return CreateLoadingRegisterResult.Failure(errorMessage);
            }

            // Calculate LoadingWeight
            var loadingWeight = request.GrossWeight - request.TareWeight;
            if (loadingWeight < 0)
            {
                return CreateLoadingRegisterResult.Failure("Loading weight cannot be negative. Gross weight must be greater than or equal to tare weight.");
            }

            // Calculate GrossAmount
            var grossAmount = loadingWeight * request.Rate;

            // Generate Challan Number
            var challanNumber = await GenerateChallanNumberAsync(cancellationToken).ConfigureAwait(false);

            var loadingRegister = new LoadingRegisterEntity
            {
                ChallanNumber = challanNumber,
                ConsignorId = request.ConsignorId,
                ConsigneeId = request.ConsigneeId,
                SourceId = request.SourceId,
                DestinationId = request.DestinationId,
                LoadingDate = request.LoadingDate,
                TPNumber = request.TPNumber,
                VehicleId = request.VehicleId,
                VehicleType = request.VehicleType,
                UnionVendorId = request.UnionVendorId,
                DriverCommission = request.DriverCommission,
                GrossWeight = request.GrossWeight,
                TareWeight = request.TareWeight,
                LoadingWeight = loadingWeight,
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

            _dbContext.LoadingRegisters.Add(loadingRegister);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Loading register '{ChallanNumber}' created successfully with ID {LoadingRegisterId}", challanNumber, loadingRegister.Id);
            return CreateLoadingRegisterResult.Success(loadingRegister.Id, challanNumber);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating loading register");
            return CreateLoadingRegisterResult.Failure("An unexpected error occurred while creating the loading register.");
        }
    }

    /// <inheritdoc />
    public async Task<UpdateLoadingRegisterResult> UpdateLoadingRegisterAsync(UpdateLoadingRegisterRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _updateValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdateLoadingRegisterResult.Failure(errorMessage);
            }

            var loadingRegister = await _dbContext.LoadingRegisters
                .FirstOrDefaultAsync(lr => lr.Id == request.LoadingRegisterId, cancellationToken)
                .ConfigureAwait(false);

            if (loadingRegister is null)
            {
                return UpdateLoadingRegisterResult.Failure("Loading register not found.");
            }

            // Calculate LoadingWeight
            var loadingWeight = request.GrossWeight - request.TareWeight;
            if (loadingWeight < 0)
            {
                return UpdateLoadingRegisterResult.Failure("Loading weight cannot be negative. Gross weight must be greater than or equal to tare weight.");
            }

            // Calculate GrossAmount
            var grossAmount = loadingWeight * request.Rate;

            loadingRegister.ConsignorId = request.ConsignorId;
            loadingRegister.ConsigneeId = request.ConsigneeId;
            loadingRegister.SourceId = request.SourceId;
            loadingRegister.DestinationId = request.DestinationId;
            loadingRegister.LoadingDate = request.LoadingDate;
            loadingRegister.TPNumber = request.TPNumber;
            loadingRegister.VehicleId = request.VehicleId;
            loadingRegister.VehicleType = request.VehicleType;
            loadingRegister.UnionVendorId = request.UnionVendorId;
            loadingRegister.DriverCommission = request.DriverCommission;
            loadingRegister.GrossWeight = request.GrossWeight;
            loadingRegister.TareWeight = request.TareWeight;
            loadingRegister.LoadingWeight = loadingWeight;
            loadingRegister.MaterialId = request.MaterialId;
            loadingRegister.Rate = request.Rate;
            loadingRegister.GrossAmount = grossAmount;
            loadingRegister.VehicleLoadedBy = request.VehicleLoadedBy;
            loadingRegister.FuelQuantity = request.FuelQuantity;
            loadingRegister.FuelAmount = request.FuelAmount;
            loadingRegister.FuelCash = request.FuelCash;
            loadingRegister.FuelAdvance = request.FuelAdvance;
            loadingRegister.ShortageWeight = request.ShortageWeight;
            loadingRegister.CashAdvance = request.CashAdvance;
            loadingRegister.PaymentLocationId = request.PaymentLocationId;
            loadingRegister.OtherAdvance = request.OtherAdvance;
            loadingRegister.OtherAdvanceDate = request.OtherAdvanceDate;
            loadingRegister.ThirdParty = request.ThirdParty;
            loadingRegister.OwnerId = request.OwnerId;
            loadingRegister.OwnerMobile = request.OwnerMobile;
            loadingRegister.OwnerAddress = request.OwnerAddress;
            loadingRegister.Driver = request.Driver;
            loadingRegister.DrivingLicenceNumber = request.DrivingLicenceNumber;
            loadingRegister.DriverMobile = request.DriverMobile;
            loadingRegister.Notes = request.Notes;
            loadingRegister.IsActive = request.IsActive;
            loadingRegister.ModifiedOn = DateTime.UtcNow;
            loadingRegister.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Loading register '{LoadingRegisterId}' updated successfully", request.LoadingRegisterId);
            return UpdateLoadingRegisterResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating loading register '{LoadingRegisterId}'", request.LoadingRegisterId);
            return UpdateLoadingRegisterResult.Failure("An unexpected error occurred while updating the loading register.");
        }
    }

    /// <inheritdoc />
    public async Task<UpdateLoadingRegisterStatusResult> UpdateLoadingRegisterStatusAsync(UpdateLoadingRegisterStatusRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var loadingRegister = await _dbContext.LoadingRegisters
                .FirstOrDefaultAsync(lr => lr.Id == request.LoadingRegisterId, cancellationToken)
                .ConfigureAwait(false);

            if (loadingRegister is null)
            {
                return UpdateLoadingRegisterStatusResult.Failure("Loading register not found.");
            }

            var validationResult = _updateStatusValidator.Validate(request, loadingRegister.IsActive);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdateLoadingRegisterStatusResult.Failure(errorMessage);
            }

            loadingRegister.IsActive = request.IsActive;
            loadingRegister.ModifiedOn = DateTime.UtcNow;
            loadingRegister.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Loading register '{LoadingRegisterId}' status updated to {IsActive}", request.LoadingRegisterId, request.IsActive);
            return UpdateLoadingRegisterStatusResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating loading register status '{LoadingRegisterId}'", request.LoadingRegisterId);
            return UpdateLoadingRegisterStatusResult.Failure("An unexpected error occurred while updating the loading register status.");
        }
    }

    /// <inheritdoc />
    public async Task<DeleteLoadingRegisterResult> DeleteLoadingRegisterAsync(DeleteLoadingRegisterRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _deleteValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return DeleteLoadingRegisterResult.Failure(errorMessage);
            }

            var loadingRegister = await _dbContext.LoadingRegisters
                .FirstOrDefaultAsync(lr => lr.Id == request.LoadingRegisterId, cancellationToken)
                .ConfigureAwait(false);

            if (loadingRegister is null)
            {
                return DeleteLoadingRegisterResult.Failure("Loading register not found.");
            }

            loadingRegister.IsDeleted = true;
            loadingRegister.DeletedOn = DateTime.UtcNow;
            loadingRegister.ModifiedOn = DateTime.UtcNow;
            loadingRegister.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Loading register '{LoadingRegisterId}' deleted successfully", request.LoadingRegisterId);
            return DeleteLoadingRegisterResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting loading register '{LoadingRegisterId}'", request.LoadingRegisterId);
            return DeleteLoadingRegisterResult.Failure("An unexpected error occurred while deleting the loading register.");
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
        var maxChallanNumber = await _dbContext.LoadingRegisters
            .AsNoTracking()
            .Where(lr => lr.ChallanNumber.StartsWith(prefix))
            .Select(lr => lr.ChallanNumber)
            .OrderByDescending(lr => lr)
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
