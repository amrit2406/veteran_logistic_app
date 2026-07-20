using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using VeteranLogistics.Data.Entities.Administration;
using veteran_logistic.Masters.SourceDestinations.Contracts;
using veteran_logistic.Masters.SourceDestinations.Models;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Masters.SourceDestinations.Services;

/// <summary>
/// Service for source/destination command operations.
/// </summary>
public sealed class SourceDestinationCommandService : ISourceDestinationCommandService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<SourceDestinationCommandService> _logger;
    private readonly ICreateSourceDestinationValidator _createValidator;
    private readonly IUpdateSourceDestinationValidator _updateValidator;
    private readonly IUpdateSourceDestinationStatusValidator _updateStatusValidator;
    private readonly IDeleteSourceDestinationValidator _deleteValidator;

    /// <summary>
    /// Initializes a new instance of the <see cref="SourceDestinationCommandService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="createValidator">The create validator.</param>
    /// <param name="updateValidator">The update validator.</param>
    /// <param name="updateStatusValidator">The update status validator.</param>
    /// <param name="deleteValidator">The delete validator.</param>
    public SourceDestinationCommandService(
        VeteranLogisticsDbContext dbContext,
        ILogger<SourceDestinationCommandService> logger,
        ICreateSourceDestinationValidator createValidator,
        IUpdateSourceDestinationValidator updateValidator,
        IUpdateSourceDestinationStatusValidator updateStatusValidator,
        IDeleteSourceDestinationValidator deleteValidator)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
        _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
        _updateStatusValidator = updateStatusValidator ?? throw new ArgumentNullException(nameof(updateStatusValidator));
        _deleteValidator = deleteValidator ?? throw new ArgumentNullException(nameof(deleteValidator));
    }

    /// <inheritdoc />
    public async Task<CreateSourceDestinationResult> CreateSourceDestinationAsync(CreateSourceDestinationRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating source/destination with name: {LocationName}", request.LocationName);

        var validationResult = _createValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Source/Destination creation validation failed: {Errors}", string.Join(", ", validationResult.Errors));
            return new CreateSourceDestinationResult
            {
                IsSuccess = false,
                ErrorMessage = validationResult.Errors.FirstOrDefault()?.ErrorMessage
            };
        }

        // Check for duplicate
        var exists = await _dbContext.SourceDestinations
            .AnyAsync(sd => sd.LocationName == request.LocationName, cancellationToken)
            .ConfigureAwait(false);

        if (exists)
        {
            _logger.LogWarning("Source/Destination with name {LocationName} already exists", request.LocationName);
            return new CreateSourceDestinationResult
            {
                IsSuccess = false,
                ErrorMessage = "A source/destination with this name already exists."
            };
        }

        var sourceDestination = new SourceDestination
        {
            LocationName = request.LocationName,
            IsActive = request.IsActive,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = "System", // TODO: Get from current user
            ModifiedOn = DateTime.UtcNow,
            ModifiedBy = "System"
        };

        _dbContext.SourceDestinations.Add(sourceDestination);

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Source/Destination created successfully with ID: {Id}", sourceDestination.Id);

            return new CreateSourceDestinationResult
            {
                IsSuccess = true,
                CreatedId = sourceDestination.Id
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating source/destination");
            return new CreateSourceDestinationResult
            {
                IsSuccess = false,
                ErrorMessage = "An error occurred while creating the source/destination."
            };
        }
    }

    /// <inheritdoc />
    public async Task<UpdateSourceDestinationResult> UpdateSourceDestinationAsync(UpdateSourceDestinationRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating source/destination with ID: {Id}", request.SourceDestinationId);

        var validationResult = _updateValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Source/Destination update validation failed: {Errors}", string.Join(", ", validationResult.Errors));
            return new UpdateSourceDestinationResult
            {
                IsSuccess = false,
                ErrorMessage = validationResult.Errors.FirstOrDefault()?.ErrorMessage
            };
        }

        var sourceDestination = await _dbContext.SourceDestinations
            .FirstOrDefaultAsync(sd => sd.Id == request.SourceDestinationId, cancellationToken)
            .ConfigureAwait(false);

        if (sourceDestination == null)
        {
            _logger.LogWarning("Source/Destination with ID {Id} not found", request.SourceDestinationId);
            return new UpdateSourceDestinationResult
            {
                IsSuccess = false,
                ErrorMessage = "Source/Destination not found."
            };
        }

        // Check for duplicate (excluding current record)
        var duplicateExists = await _dbContext.SourceDestinations
            .AnyAsync(sd => sd.Id != request.SourceDestinationId && sd.LocationName == request.LocationName, cancellationToken)
            .ConfigureAwait(false);

        if (duplicateExists)
        {
            _logger.LogWarning("Source/Destination with name {LocationName} already exists", request.LocationName);
            return new UpdateSourceDestinationResult
            {
                IsSuccess = false,
                ErrorMessage = "A source/destination with this name already exists."
            };
        }

        sourceDestination.LocationName = request.LocationName;
        sourceDestination.IsActive = request.IsActive;
        sourceDestination.ModifiedOn = DateTime.UtcNow;
        sourceDestination.ModifiedBy = "System"; // TODO: Get from current user

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Source/Destination updated successfully");

            return new UpdateSourceDestinationResult
            {
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating source/destination");
            return new UpdateSourceDestinationResult
            {
                IsSuccess = false,
                ErrorMessage = "An error occurred while updating the source/destination."
            };
        }
    }

    /// <inheritdoc />
    public async Task<UpdateSourceDestinationStatusResult> UpdateSourceDestinationStatusAsync(UpdateSourceDestinationStatusRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating status for source/destination ID: {Id} to {IsActive}", request.SourceDestinationId, request.IsActive);

        var validationResult = _updateStatusValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Source/Destination status update validation failed: {Errors}", string.Join(", ", validationResult.Errors));
            return new UpdateSourceDestinationStatusResult
            {
                IsSuccess = false,
                ErrorMessage = validationResult.Errors.FirstOrDefault()?.ErrorMessage
            };
        }

        var sourceDestination = await _dbContext.SourceDestinations
            .FirstOrDefaultAsync(sd => sd.Id == request.SourceDestinationId, cancellationToken)
            .ConfigureAwait(false);

        if (sourceDestination == null)
        {
            _logger.LogWarning("Source/Destination with ID {Id} not found", request.SourceDestinationId);
            return new UpdateSourceDestinationStatusResult
            {
                IsSuccess = false,
                ErrorMessage = "Source/Destination not found."
            };
        }

        if (sourceDestination.IsActive == request.IsActive)
        {
            _logger.LogWarning("Source/Destination already has the requested status");
            return new UpdateSourceDestinationStatusResult
            {
                IsSuccess = false,
                ErrorMessage = "Source/Destination already has the requested status."
            };
        }

        sourceDestination.IsActive = request.IsActive;
        sourceDestination.ModifiedOn = DateTime.UtcNow;
        sourceDestination.ModifiedBy = "System"; // TODO: Get from current user

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Source/Destination status updated successfully");

            return new UpdateSourceDestinationStatusResult
            {
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating source/destination status");
            return new UpdateSourceDestinationStatusResult
            {
                IsSuccess = false,
                ErrorMessage = "An error occurred while updating the source/destination status."
            };
        }
    }

    /// <inheritdoc />
    public async Task<DeleteSourceDestinationResult> DeleteSourceDestinationAsync(DeleteSourceDestinationRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting source/destination with ID: {Id}", request.SourceDestinationId);

        var validationResult = _deleteValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Source/Destination deletion validation failed: {Errors}", string.Join(", ", validationResult.Errors));
            return new DeleteSourceDestinationResult
            {
                IsSuccess = false,
                ErrorMessage = validationResult.Errors.FirstOrDefault()?.ErrorMessage
            };
        }

        var sourceDestination = await _dbContext.SourceDestinations
            .FirstOrDefaultAsync(sd => sd.Id == request.SourceDestinationId, cancellationToken)
            .ConfigureAwait(false);

        if (sourceDestination == null)
        {
            _logger.LogWarning("Source/Destination with ID {Id} not found", request.SourceDestinationId);
            return new DeleteSourceDestinationResult
            {
                IsSuccess = false,
                ErrorMessage = "Source/Destination not found."
            };
        }

        // Soft delete
        sourceDestination.IsDeleted = true;
        sourceDestination.DeletedOn = DateTime.UtcNow;
        sourceDestination.ModifiedOn = DateTime.UtcNow;
        sourceDestination.ModifiedBy = "System"; // TODO: Get from current user

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Source/Destination deleted successfully");

            return new DeleteSourceDestinationResult
            {
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting source/destination");
            return new DeleteSourceDestinationResult
            {
                IsSuccess = false,
                ErrorMessage = "An error occurred while deleting the source/destination."
            };
        }
    }
}
