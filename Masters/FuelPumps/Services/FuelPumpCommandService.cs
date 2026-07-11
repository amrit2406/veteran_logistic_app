using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using FuelPumpEntity = VeteranLogistics.Data.Entities.Administration.FuelPump;
using veteran_logistic.Masters.FuelPumps.Contracts;
using veteran_logistic.Masters.FuelPumps.Models;

namespace veteran_logistic.Masters.FuelPumps.Services;

/// <summary>
/// Implementation of the fuel pump command service.
/// </summary>
public sealed class FuelPumpCommandService : IFuelPumpCommandService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ICreateFuelPumpValidator _createValidator;
    private readonly IUpdateFuelPumpValidator _updateValidator;
    private readonly IUpdateFuelPumpStatusValidator _updateStatusValidator;
    private readonly IDeleteFuelPumpValidator _deleteValidator;
    private readonly ILogger<FuelPumpCommandService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="FuelPumpCommandService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="createValidator">The fuel pump creation validator.</param>
    /// <param name="updateValidator">The fuel pump update validator.</param>
    /// <param name="updateStatusValidator">The fuel pump status update validator.</param>
    /// <param name="deleteValidator">The delete fuel pump validator.</param>
    /// <param name="logger">The logger.</param>
    public FuelPumpCommandService(
        VeteranLogisticsDbContext dbContext,
        ICreateFuelPumpValidator createValidator,
        IUpdateFuelPumpValidator updateValidator,
        IUpdateFuelPumpStatusValidator updateStatusValidator,
        IDeleteFuelPumpValidator deleteValidator,
        ILogger<FuelPumpCommandService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
        _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
        _updateStatusValidator = updateStatusValidator ?? throw new ArgumentNullException(nameof(updateStatusValidator));
        _deleteValidator = deleteValidator ?? throw new ArgumentNullException(nameof(deleteValidator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<CreateFuelPumpResult> CreateFuelPumpAsync(CreateFuelPumpRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _createValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return CreateFuelPumpResult.Failure(errorMessage);
            }

            // Check for duplicate FuelPumpName
            var existingFuelPumpByName = await _dbContext.FuelPumps
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.FuelPumpName == request.FuelPumpName, cancellationToken)
                .ConfigureAwait(false);

            if (existingFuelPumpByName is not null)
            {
                return CreateFuelPumpResult.Failure("A fuel pump with this name already exists.");
            }

            var fuelPump = new FuelPumpEntity
            {
                FuelPumpName = request.FuelPumpName,
                IsActive = request.IsActive,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System", // TODO: Replace with actual user from session
                ModifiedBy = "System"
            };

            _dbContext.FuelPumps.Add(fuelPump);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Fuel pump '{FuelPumpName}' created successfully with ID {FuelPumpId}", request.FuelPumpName, fuelPump.Id);
            return CreateFuelPumpResult.Success(fuelPump.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating fuel pump '{FuelPumpName}'", request.FuelPumpName);
            return CreateFuelPumpResult.Failure("An unexpected error occurred while creating the fuel pump.");
        }
    }

    /// <inheritdoc />
    public async Task<UpdateFuelPumpResult> UpdateFuelPumpAsync(UpdateFuelPumpRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _updateValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdateFuelPumpResult.Failure(errorMessage);
            }

            var fuelPump = await _dbContext.FuelPumps
                .FirstOrDefaultAsync(f => f.Id == request.FuelPumpId, cancellationToken)
                .ConfigureAwait(false);

            if (fuelPump is null)
            {
                return UpdateFuelPumpResult.Failure("Fuel pump not found.");
            }

            // Check for duplicate FuelPumpName (excluding current fuel pump)
            var existingFuelPumpByName = await _dbContext.FuelPumps
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.FuelPumpName == request.FuelPumpName && f.Id != request.FuelPumpId, cancellationToken)
                .ConfigureAwait(false);

            if (existingFuelPumpByName is not null)
            {
                return UpdateFuelPumpResult.Failure("A fuel pump with this name already exists.");
            }

            fuelPump.FuelPumpName = request.FuelPumpName;
            fuelPump.IsActive = request.IsActive;
            fuelPump.ModifiedOn = DateTime.UtcNow;
            fuelPump.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Fuel pump '{FuelPumpId}' updated successfully", request.FuelPumpId);
            return UpdateFuelPumpResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating fuel pump '{FuelPumpId}'", request.FuelPumpId);
            return UpdateFuelPumpResult.Failure("An unexpected error occurred while updating the fuel pump.");
        }
    }

    /// <inheritdoc />
    public async Task<UpdateFuelPumpStatusResult> UpdateFuelPumpStatusAsync(UpdateFuelPumpStatusRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var fuelPump = await _dbContext.FuelPumps
                .FirstOrDefaultAsync(f => f.Id == request.FuelPumpId, cancellationToken)
                .ConfigureAwait(false);

            if (fuelPump is null)
            {
                return UpdateFuelPumpStatusResult.Failure("Fuel pump not found.");
            }

            var validationResult = _updateStatusValidator.Validate(request, fuelPump.IsActive);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdateFuelPumpStatusResult.Failure(errorMessage);
            }

            fuelPump.IsActive = request.IsActive;
            fuelPump.ModifiedOn = DateTime.UtcNow;
            fuelPump.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Fuel pump '{FuelPumpId}' status updated to {IsActive}", request.FuelPumpId, request.IsActive);
            return UpdateFuelPumpStatusResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating fuel pump status '{FuelPumpId}'", request.FuelPumpId);
            return UpdateFuelPumpStatusResult.Failure("An unexpected error occurred while updating the fuel pump status.");
        }
    }

    /// <inheritdoc />
    public async Task<DeleteFuelPumpResult> DeleteFuelPumpAsync(DeleteFuelPumpRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _deleteValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return DeleteFuelPumpResult.Failure(errorMessage);
            }

            var fuelPump = await _dbContext.FuelPumps
                .FirstOrDefaultAsync(f => f.Id == request.FuelPumpId, cancellationToken)
                .ConfigureAwait(false);

            if (fuelPump is null)
            {
                return DeleteFuelPumpResult.Failure("Fuel pump not found.");
            }

            fuelPump.IsDeleted = true;
            fuelPump.DeletedOn = DateTime.UtcNow;
            fuelPump.ModifiedOn = DateTime.UtcNow;
            fuelPump.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Fuel pump '{FuelPumpId}' deleted successfully", request.FuelPumpId);
            return DeleteFuelPumpResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting fuel pump '{FuelPumpId}'", request.FuelPumpId);
            return DeleteFuelPumpResult.Failure("An unexpected error occurred while deleting the fuel pump.");
        }
    }
}
