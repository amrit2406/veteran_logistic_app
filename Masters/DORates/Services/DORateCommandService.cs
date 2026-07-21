using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using VeteranLogistics.Data.Entities.Administration;
using veteran_logistic.Masters.DORates.Contracts;
using veteran_logistic.Masters.DORates.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.DORates.Services;

/// <summary>
/// Service for DO Rate command operations.
/// </summary>
public sealed class DORateCommandService : IDORateCommandService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<DORateCommandService> _logger;
    private readonly ICreateDORateValidator _createValidator;
    private readonly IUpdateDORateValidator _updateValidator;
    private readonly IUpdateDORateStatusValidator _updateStatusValidator;
    private readonly IDeleteDORateValidator _deleteValidator;

    /// <summary>
    /// Initializes a new instance of the <see cref="DORateCommandService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="createValidator">The create validator.</param>
    /// <param name="updateValidator">The update validator.</param>
    /// <param name="updateStatusValidator">The update status validator.</param>
    /// <param name="deleteValidator">The delete validator.</param>
    public DORateCommandService(
        VeteranLogisticsDbContext dbContext,
        ILogger<DORateCommandService> logger,
        ICreateDORateValidator createValidator,
        IUpdateDORateValidator updateValidator,
        IUpdateDORateStatusValidator updateStatusValidator,
        IDeleteDORateValidator deleteValidator)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
        _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
        _updateStatusValidator = updateStatusValidator ?? throw new ArgumentNullException(nameof(updateStatusValidator));
        _deleteValidator = deleteValidator ?? throw new ArgumentNullException(nameof(deleteValidator));
    }

    /// <inheritdoc />
    public async Task<CreateDORateResult> CreateDORateAsync(CreateDORateRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating DO rate with DO number: {DONumber}", request.DONumber);

        var validationResult = _createValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("DO Rate creation validation failed: {Errors}", string.Join(", ", validationResult.Errors));
            return new CreateDORateResult
            {
                IsSuccess = false,
                ErrorMessage = validationResult.Errors.FirstOrDefault()?.ErrorMessage
            };
        }

        // Validate Source exists
        var source = await _dbContext.SourceDestinations
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == request.SourceId, cancellationToken)
            .ConfigureAwait(false);

        if (source == null)
        {
            _logger.LogWarning("Source with ID {SourceId} not found", request.SourceId);
            return new CreateDORateResult
            {
                IsSuccess = false,
                ErrorMessage = "Source not found."
            };
        }

        // Validate Destination exists
        var destination = await _dbContext.SourceDestinations
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == request.DestinationId, cancellationToken)
            .ConfigureAwait(false);

        if (destination == null)
        {
            _logger.LogWarning("Destination with ID {DestinationId} not found", request.DestinationId);
            return new CreateDORateResult
            {
                IsSuccess = false,
                ErrorMessage = "Destination not found."
            };
        }

        // Check for duplicate active DO setup
        var exists = await _dbContext.DORates
            .AnyAsync(d => d.SourceId == request.SourceId &&
                           d.DestinationId == request.DestinationId &&
                           d.EffectiveDate == request.EffectiveDate &&
                           d.DONumber == request.DONumber &&
                           d.IsActive &&
                           !d.IsDeleted, cancellationToken)
            .ConfigureAwait(false);

        if (exists)
        {
            _logger.LogWarning("DO Rate with same Source, Destination, Effective Date and DO Number already exists");
            return new CreateDORateResult
            {
                IsSuccess = false,
                ErrorMessage = "An active DO rate with the same Source, Destination, Effective Date and DO Number already exists."
            };
        }

        var doRate = new DORate
        {
            ConsignorId = request.ConsignorId,
            ConsigneeId = request.ConsigneeId,
            SourceId = request.SourceId,
            DestinationId = request.DestinationId,
            EffectiveDate = request.EffectiveDate,
            FreightRate = request.FreightRate,
            UnionRate = request.UnionRate,
            VendorRate = request.VendorRate,
            DONumber = request.DONumber,
            BillingRate = request.BillingRate,
            AllowedShortage = request.AllowedShortage,
            RatePerKg = request.RatePerKg,
            VesselName = request.VesselName,
            TraderName = request.TraderName,
            Narration = request.Narration,
            IsActive = request.IsActive,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = "System",
            ModifiedOn = DateTime.UtcNow,
            ModifiedBy = "System"
        };

        _dbContext.DORates.Add(doRate);

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("DO Rate created successfully with ID: {Id}", doRate.Id);

            return new CreateDORateResult
            {
                IsSuccess = true,
                CreatedId = doRate.Id
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating DO rate");
            return new CreateDORateResult
            {
                IsSuccess = false,
                ErrorMessage = "An error occurred while creating the DO rate."
            };
        }
    }

    /// <inheritdoc />
    public async Task<UpdateDORateResult> UpdateDORateAsync(UpdateDORateRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating DO rate with ID: {Id}", request.DORateId);

        var validationResult = _updateValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("DO Rate update validation failed: {Errors}", string.Join(", ", validationResult.Errors));
            return new UpdateDORateResult
            {
                IsSuccess = false,
                ErrorMessage = validationResult.Errors.FirstOrDefault()?.ErrorMessage
            };
        }

        var doRate = await _dbContext.DORates
            .FirstOrDefaultAsync(d => d.Id == request.DORateId, cancellationToken)
            .ConfigureAwait(false);

        if (doRate == null)
        {
            _logger.LogWarning("DO Rate with ID {Id} not found", request.DORateId);
            return new UpdateDORateResult
            {
                IsSuccess = false,
                ErrorMessage = "DO Rate not found."
            };
        }

        // Validate Source exists
        var source = await _dbContext.SourceDestinations
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == request.SourceId, cancellationToken)
            .ConfigureAwait(false);

        if (source == null)
        {
            _logger.LogWarning("Source with ID {SourceId} not found", request.SourceId);
            return new UpdateDORateResult
            {
                IsSuccess = false,
                ErrorMessage = "Source not found."
            };
        }

        // Validate Destination exists
        var destination = await _dbContext.SourceDestinations
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == request.DestinationId, cancellationToken)
            .ConfigureAwait(false);

        if (destination == null)
        {
            _logger.LogWarning("Destination with ID {DestinationId} not found", request.DestinationId);
            return new UpdateDORateResult
            {
                IsSuccess = false,
                ErrorMessage = "Destination not found."
            };
        }

        // Check for duplicate (excluding current record)
        var duplicateExists = await _dbContext.DORates
            .AnyAsync(d => d.Id != request.DORateId &&
                           d.SourceId == request.SourceId &&
                           d.DestinationId == request.DestinationId &&
                           d.EffectiveDate == request.EffectiveDate &&
                           d.DONumber == request.DONumber &&
                           d.IsActive &&
                           !d.IsDeleted, cancellationToken)
            .ConfigureAwait(false);

        if (duplicateExists)
        {
            _logger.LogWarning("DO Rate with same Source, Destination, Effective Date and DO Number already exists");
            return new UpdateDORateResult
            {
                IsSuccess = false,
                ErrorMessage = "An active DO rate with the same Source, Destination, Effective Date and DO Number already exists."
            };
        }

        doRate.ConsignorId = request.ConsignorId;
        doRate.ConsigneeId = request.ConsigneeId;
        doRate.SourceId = request.SourceId;
        doRate.DestinationId = request.DestinationId;
        doRate.EffectiveDate = request.EffectiveDate;
        doRate.FreightRate = request.FreightRate;
        doRate.UnionRate = request.UnionRate;
        doRate.VendorRate = request.VendorRate;
        doRate.DONumber = request.DONumber;
        doRate.BillingRate = request.BillingRate;
        doRate.AllowedShortage = request.AllowedShortage;
        doRate.RatePerKg = request.RatePerKg;
        doRate.VesselName = request.VesselName;
        doRate.TraderName = request.TraderName;
        doRate.Narration = request.Narration;
        doRate.IsActive = request.IsActive;
        doRate.ModifiedOn = DateTime.UtcNow;
        doRate.ModifiedBy = "System";

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("DO Rate updated successfully");

            return new UpdateDORateResult
            {
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating DO rate");
            return new UpdateDORateResult
            {
                IsSuccess = false,
                ErrorMessage = "An error occurred while updating the DO rate."
            };
        }
    }

    /// <inheritdoc />
    public async Task<UpdateDORateStatusResult> UpdateDORateStatusAsync(UpdateDORateStatusRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating status for DO Rate ID: {Id} to {IsActive}", request.DORateId, request.IsActive);

        var validationResult = _updateStatusValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("DO Rate status update validation failed: {Errors}", string.Join(", ", validationResult.Errors));
            return new UpdateDORateStatusResult
            {
                IsSuccess = false,
                ErrorMessage = validationResult.Errors.FirstOrDefault()?.ErrorMessage
            };
        }

        var doRate = await _dbContext.DORates
            .FirstOrDefaultAsync(d => d.Id == request.DORateId, cancellationToken)
            .ConfigureAwait(false);

        if (doRate == null)
        {
            _logger.LogWarning("DO Rate with ID {Id} not found", request.DORateId);
            return new UpdateDORateStatusResult
            {
                IsSuccess = false,
                ErrorMessage = "DO Rate not found."
            };
        }

        if (doRate.IsActive == request.IsActive)
        {
            _logger.LogWarning("DO Rate already has the requested status");
            return new UpdateDORateStatusResult
            {
                IsSuccess = false,
                ErrorMessage = "DO Rate already has the requested status."
            };
        }

        doRate.IsActive = request.IsActive;
        doRate.ModifiedOn = DateTime.UtcNow;
        doRate.ModifiedBy = "System";

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("DO Rate status updated successfully");

            return new UpdateDORateStatusResult
            {
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating DO rate status");
            return new UpdateDORateStatusResult
            {
                IsSuccess = false,
                ErrorMessage = "An error occurred while updating the DO rate status."
            };
        }
    }

    /// <inheritdoc />
    public async Task<DeleteDORateResult> DeleteDORateAsync(DeleteDORateRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting DO rate with ID: {Id}", request.DORateId);

        var validationResult = _deleteValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("DO Rate deletion validation failed: {Errors}", string.Join(", ", validationResult.Errors));
            return new DeleteDORateResult
            {
                IsSuccess = false,
                ErrorMessage = validationResult.Errors.FirstOrDefault()?.ErrorMessage
            };
        }

        var doRate = await _dbContext.DORates
            .FirstOrDefaultAsync(d => d.Id == request.DORateId, cancellationToken)
            .ConfigureAwait(false);

        if (doRate == null)
        {
            _logger.LogWarning("DO Rate with ID {Id} not found", request.DORateId);
            return new DeleteDORateResult
            {
                IsSuccess = false,
                ErrorMessage = "DO Rate not found."
            };
        }

        // Soft delete
        doRate.IsDeleted = true;
        doRate.DeletedOn = DateTime.UtcNow;
        doRate.ModifiedOn = DateTime.UtcNow;
        doRate.ModifiedBy = "System";

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("DO Rate deleted successfully");

            return new DeleteDORateResult
            {
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting DO rate");
            return new DeleteDORateResult
            {
                IsSuccess = false,
                ErrorMessage = "An error occurred while deleting the DO rate."
            };
        }
    }
}
