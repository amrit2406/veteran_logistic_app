using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using HsdRateEntity = VeteranLogistics.Data.Entities.Administration.HsdRate;
using veteran_logistic.Masters.HsdRates.Contracts;
using veteran_logistic.Masters.HsdRates.Models;

namespace veteran_logistic.Masters.HsdRates.Services;

/// <summary>
/// Implementation of the HSD rate command service.
/// </summary>
public sealed class HsdRateCommandService : IHsdRateCommandService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ICreateHsdRateValidator _createValidator;
    private readonly IUpdateHsdRateValidator _updateValidator;
    private readonly IUpdateHsdRateStatusValidator _updateStatusValidator;
    private readonly IDeleteHsdRateValidator _deleteValidator;
    private readonly ILogger<HsdRateCommandService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="HsdRateCommandService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="createValidator">The HSD rate creation validator.</param>
    /// <param name="updateValidator">The HSD rate update validator.</param>
    /// <param name="updateStatusValidator">The HSD rate status update validator.</param>
    /// <param name="deleteValidator">The delete HSD rate validator.</param>
    /// <param name="logger">The logger.</param>
    public HsdRateCommandService(
        VeteranLogisticsDbContext dbContext,
        ICreateHsdRateValidator createValidator,
        IUpdateHsdRateValidator updateValidator,
        IUpdateHsdRateStatusValidator updateStatusValidator,
        IDeleteHsdRateValidator deleteValidator,
        ILogger<HsdRateCommandService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
        _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
        _updateStatusValidator = updateStatusValidator ?? throw new ArgumentNullException(nameof(updateStatusValidator));
        _deleteValidator = deleteValidator ?? throw new ArgumentNullException(nameof(deleteValidator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<CreateHsdRateResult> CreateHsdRateAsync(CreateHsdRateRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _createValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return CreateHsdRateResult.Failure(errorMessage);
            }

            // Validate Fuel Pump exists
            var fuelPump = await _dbContext.FuelPumps
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == request.FuelPumpId, cancellationToken)
                .ConfigureAwait(false);

            if (fuelPump is null)
            {
                return CreateHsdRateResult.Failure("Fuel pump not found.");
            }

            // Check for duplicate FuelPumpId + ApplicableDate combination
            var existingHsdRate = await _dbContext.HsdRates
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.FuelPumpId == request.FuelPumpId && h.ApplicableDate == request.ApplicableDate, cancellationToken)
                .ConfigureAwait(false);

            if (existingHsdRate is not null)
            {
                return CreateHsdRateResult.Failure("An HSD rate for this fuel pump and applicable date already exists.");
            }

            var hsdRate = new HsdRateEntity
            {
                FuelPumpId = request.FuelPumpId,
                ApplicableDate = request.ApplicableDate,
                RatePerLitre = request.RatePerLitre,
                IsActive = request.IsActive,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System", // TODO: Replace with actual user from session
                ModifiedBy = "System"
            };

            _dbContext.HsdRates.Add(hsdRate);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("HSD rate for FuelPump {FuelPumpId} on {ApplicableDate} created successfully with ID {HsdRateId}", request.FuelPumpId, request.ApplicableDate.ToShortDateString(), hsdRate.Id);
            return CreateHsdRateResult.Success(hsdRate.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating HSD rate for FuelPump {FuelPumpId} on {ApplicableDate}", request.FuelPumpId, request.ApplicableDate.ToShortDateString());
            return CreateHsdRateResult.Failure("An unexpected error occurred while creating the HSD rate.");
        }
    }

    /// <inheritdoc />
    public async Task<UpdateHsdRateResult> UpdateHsdRateAsync(UpdateHsdRateRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _updateValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdateHsdRateResult.Failure(errorMessage);
            }

            var hsdRate = await _dbContext.HsdRates
                .FirstOrDefaultAsync(h => h.Id == request.HsdRateId, cancellationToken)
                .ConfigureAwait(false);

            if (hsdRate is null)
            {
                return UpdateHsdRateResult.Failure("HSD rate not found.");
            }

            // Validate Fuel Pump exists
            var fuelPump = await _dbContext.FuelPumps
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == request.FuelPumpId, cancellationToken)
                .ConfigureAwait(false);

            if (fuelPump is null)
            {
                return UpdateHsdRateResult.Failure("Fuel pump not found.");
            }

            // Check for duplicate FuelPumpId + ApplicableDate (excluding current HSD rate)
            var existingHsdRate = await _dbContext.HsdRates
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.FuelPumpId == request.FuelPumpId && h.ApplicableDate == request.ApplicableDate && h.Id != request.HsdRateId, cancellationToken)
                .ConfigureAwait(false);

            if (existingHsdRate is not null)
            {
                return UpdateHsdRateResult.Failure("An HSD rate for this fuel pump and applicable date already exists.");
            }

            hsdRate.FuelPumpId = request.FuelPumpId;
            hsdRate.ApplicableDate = request.ApplicableDate;
            hsdRate.RatePerLitre = request.RatePerLitre;
            hsdRate.IsActive = request.IsActive;
            hsdRate.ModifiedOn = DateTime.UtcNow;
            hsdRate.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("HSD rate '{HsdRateId}' updated successfully", request.HsdRateId);
            return UpdateHsdRateResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating HSD rate '{HsdRateId}'", request.HsdRateId);
            return UpdateHsdRateResult.Failure("An unexpected error occurred while updating the HSD rate.");
        }
    }

    /// <inheritdoc />
    public async Task<UpdateHsdRateStatusResult> UpdateHsdRateStatusAsync(UpdateHsdRateStatusRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var hsdRate = await _dbContext.HsdRates
                .FirstOrDefaultAsync(h => h.Id == request.HsdRateId, cancellationToken)
                .ConfigureAwait(false);

            if (hsdRate is null)
            {
                return UpdateHsdRateStatusResult.Failure("HSD rate not found.");
            }

            var validationResult = _updateStatusValidator.Validate(request, hsdRate.IsActive);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdateHsdRateStatusResult.Failure(errorMessage);
            }

            hsdRate.IsActive = request.IsActive;
            hsdRate.ModifiedOn = DateTime.UtcNow;
            hsdRate.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("HSD rate '{HsdRateId}' status updated to {IsActive}", request.HsdRateId, request.IsActive);
            return UpdateHsdRateStatusResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating HSD rate status '{HsdRateId}'", request.HsdRateId);
            return UpdateHsdRateStatusResult.Failure("An unexpected error occurred while updating the HSD rate status.");
        }
    }

    /// <inheritdoc />
    public async Task<DeleteHsdRateResult> DeleteHsdRateAsync(DeleteHsdRateRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _deleteValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return DeleteHsdRateResult.Failure(errorMessage);
            }

            var hsdRate = await _dbContext.HsdRates
                .FirstOrDefaultAsync(h => h.Id == request.HsdRateId, cancellationToken)
                .ConfigureAwait(false);

            if (hsdRate is null)
            {
                return DeleteHsdRateResult.Failure("HSD rate not found.");
            }

            hsdRate.IsDeleted = true;
            hsdRate.DeletedOn = DateTime.UtcNow;
            hsdRate.ModifiedOn = DateTime.UtcNow;
            hsdRate.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("HSD rate '{HsdRateId}' deleted successfully", request.HsdRateId);
            return DeleteHsdRateResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting HSD rate '{HsdRateId}'", request.HsdRateId);
            return DeleteHsdRateResult.Failure("An unexpected error occurred while deleting the HSD rate.");
        }
    }
}
