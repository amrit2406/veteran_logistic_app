using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using VehicleEntity = VeteranLogistics.Data.Entities.Administration.Vehicle;
using veteran_logistic.Masters.Vehicles.Contracts;
using veteran_logistic.Masters.Vehicles.Models;

namespace veteran_logistic.Masters.Vehicles.Services;

/// <summary>
/// Implementation of the vehicle command service.
/// </summary>
public sealed class VehicleCommandService : IVehicleCommandService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ICreateVehicleValidator _createValidator;
    private readonly IUpdateVehicleValidator _updateValidator;
    private readonly IUpdateVehicleStatusValidator _updateStatusValidator;
    private readonly IDeleteVehicleValidator _deleteValidator;
    private readonly ILogger<VehicleCommandService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="VehicleCommandService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="createValidator">The vehicle creation validator.</param>
    /// <param name="updateValidator">The vehicle update validator.</param>
    /// <param name="updateStatusValidator">The vehicle status update validator.</param>
    /// <param name="deleteValidator">The delete vehicle validator.</param>
    /// <param name="logger">The logger.</param>
    public VehicleCommandService(
        VeteranLogisticsDbContext dbContext,
        ICreateVehicleValidator createValidator,
        IUpdateVehicleValidator updateValidator,
        IUpdateVehicleStatusValidator updateStatusValidator,
        IDeleteVehicleValidator deleteValidator,
        ILogger<VehicleCommandService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
        _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
        _updateStatusValidator = updateStatusValidator ?? throw new ArgumentNullException(nameof(updateStatusValidator));
        _deleteValidator = deleteValidator ?? throw new ArgumentNullException(nameof(deleteValidator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<CreateVehicleResult> CreateVehicleAsync(CreateVehicleRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _createValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return CreateVehicleResult.Failure(errorMessage);
            }

            // Check for duplicate Vehicle Number
            var existingVehicleByNumber = await _dbContext.Vehicles
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.VehicleNumber == request.VehicleNumber, cancellationToken)
                .ConfigureAwait(false);

            if (existingVehicleByNumber is not null)
            {
                return CreateVehicleResult.Failure("A vehicle with this vehicle number already exists.");
            }

            // Validate Vehicle Owner exists
            var vehicleOwner = await _dbContext.VehicleOwners
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == request.VehicleOwnerId, cancellationToken)
                .ConfigureAwait(false);

            if (vehicleOwner is null)
            {
                return CreateVehicleResult.Failure("Vehicle owner not found.");
            }

            var vehicle = new VehicleEntity
            {
                VehicleOwnerId = request.VehicleOwnerId,
                VehicleNumber = request.VehicleNumber,
                VehicleType = request.VehicleType,
                IsActive = request.IsActive,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System", // TODO: Replace with actual user from session
                ModifiedBy = "System"
            };

            _dbContext.Vehicles.Add(vehicle);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Vehicle '{VehicleNumber}' created successfully with ID {VehicleId}", request.VehicleNumber, vehicle.Id);
            return CreateVehicleResult.Success(vehicle.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating vehicle");
            return CreateVehicleResult.Failure("An unexpected error occurred while creating the vehicle.");
        }
    }

    /// <inheritdoc />
    public async Task<UpdateVehicleResult> UpdateVehicleAsync(UpdateVehicleRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _updateValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdateVehicleResult.Failure(errorMessage);
            }

            var vehicle = await _dbContext.Vehicles
                .FirstOrDefaultAsync(v => v.Id == request.VehicleId, cancellationToken)
                .ConfigureAwait(false);

            if (vehicle is null)
            {
                return UpdateVehicleResult.Failure("Vehicle not found.");
            }

            // Check for duplicate Vehicle Number (excluding current vehicle)
            var existingVehicleByNumber = await _dbContext.Vehicles
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.VehicleNumber == request.VehicleNumber && v.Id != request.VehicleId, cancellationToken)
                .ConfigureAwait(false);

            if (existingVehicleByNumber is not null)
            {
                return UpdateVehicleResult.Failure("A vehicle with this vehicle number already exists.");
            }

            // Validate Vehicle Owner exists
            var vehicleOwner = await _dbContext.VehicleOwners
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == request.VehicleOwnerId, cancellationToken)
                .ConfigureAwait(false);

            if (vehicleOwner is null)
            {
                return UpdateVehicleResult.Failure("Vehicle owner not found.");
            }

            vehicle.VehicleOwnerId = request.VehicleOwnerId;
            vehicle.VehicleNumber = request.VehicleNumber;
            vehicle.VehicleType = request.VehicleType;
            vehicle.IsActive = request.IsActive;
            vehicle.ModifiedOn = DateTime.UtcNow;
            vehicle.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Vehicle '{VehicleId}' updated successfully", request.VehicleId);
            return UpdateVehicleResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating vehicle '{VehicleId}'", request.VehicleId);
            return UpdateVehicleResult.Failure("An unexpected error occurred while updating the vehicle.");
        }
    }

    /// <inheritdoc />
    public async Task<UpdateVehicleStatusResult> UpdateVehicleStatusAsync(UpdateVehicleStatusRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var vehicle = await _dbContext.Vehicles
                .FirstOrDefaultAsync(v => v.Id == request.VehicleId, cancellationToken)
                .ConfigureAwait(false);

            if (vehicle is null)
            {
                return UpdateVehicleStatusResult.Failure("Vehicle not found.");
            }

            var validationResult = _updateStatusValidator.Validate(request, vehicle.IsActive);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdateVehicleStatusResult.Failure(errorMessage);
            }

            vehicle.IsActive = request.IsActive;
            vehicle.ModifiedOn = DateTime.UtcNow;
            vehicle.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Vehicle '{VehicleId}' status updated to {IsActive}", request.VehicleId, request.IsActive);
            return UpdateVehicleStatusResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating vehicle status '{VehicleId}'", request.VehicleId);
            return UpdateVehicleStatusResult.Failure("An unexpected error occurred while updating the vehicle status.");
        }
    }

    /// <inheritdoc />
    public async Task<DeleteVehicleResult> DeleteVehicleAsync(DeleteVehicleRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _deleteValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return DeleteVehicleResult.Failure(errorMessage);
            }

            var vehicle = await _dbContext.Vehicles
                .FirstOrDefaultAsync(v => v.Id == request.VehicleId, cancellationToken)
                .ConfigureAwait(false);

            if (vehicle is null)
            {
                return DeleteVehicleResult.Failure("Vehicle not found.");
            }

            vehicle.IsDeleted = true;
            vehicle.DeletedOn = DateTime.UtcNow;
            vehicle.ModifiedOn = DateTime.UtcNow;
            vehicle.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Vehicle '{VehicleId}' deleted successfully", request.VehicleId);
            return DeleteVehicleResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting vehicle '{VehicleId}'", request.VehicleId);
            return DeleteVehicleResult.Failure("An unexpected error occurred while deleting the vehicle.");
        }
    }
}
