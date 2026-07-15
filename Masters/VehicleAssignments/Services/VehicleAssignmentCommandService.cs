using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using VehicleAssignmentEntity = VeteranLogistics.Data.Entities.Administration.VehicleAssignment;
using VehicleOwnerEntity = VeteranLogistics.Data.Entities.Administration.VehicleOwner;
using veteran_logistic.Masters.VehicleAssignments.Contracts;
using veteran_logistic.Masters.VehicleAssignments.Models;
using veteran_logistic.Masters.VehicleAssignments.Validators;

namespace veteran_logistic.Masters.VehicleAssignments.Services;

/// <summary>
/// Implementation of the vehicle assignment command service.
/// </summary>
public sealed class VehicleAssignmentCommandService : IVehicleAssignmentCommandService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly IAssignVehicleValidator _assignValidator;
    private readonly IUpdateVehicleAssignmentValidator _updateValidator;
    private readonly IReleaseVehicleValidator _releaseValidator;
    private readonly IDeleteVehicleAssignmentValidator _deleteValidator;
    private readonly ILogger<VehicleAssignmentCommandService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="VehicleAssignmentCommandService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="assignValidator">The assign vehicle validator.</param>
    /// <param name="updateValidator">The update vehicle assignment validator.</param>
    /// <param name="releaseValidator">The release vehicle validator.</param>
    /// <param name="deleteValidator">The delete vehicle assignment validator.</param>
    /// <param name="logger">The logger.</param>
    public VehicleAssignmentCommandService(
        VeteranLogisticsDbContext dbContext,
        IAssignVehicleValidator assignValidator,
        IUpdateVehicleAssignmentValidator updateValidator,
        IReleaseVehicleValidator releaseValidator,
        IDeleteVehicleAssignmentValidator deleteValidator,
        ILogger<VehicleAssignmentCommandService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _assignValidator = assignValidator ?? throw new ArgumentNullException(nameof(assignValidator));
        _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
        _releaseValidator = releaseValidator ?? throw new ArgumentNullException(nameof(releaseValidator));
        _deleteValidator = deleteValidator ?? throw new ArgumentNullException(nameof(deleteValidator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<AssignVehicleResult> AssignVehicleAsync(AssignVehicleRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _assignValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return AssignVehicleResult.Failure(errorMessage);
            }

            // Business rule: A vehicle cannot have two active assignments
            var existingActiveAssignment = await _dbContext.VehicleAssignments
                .AsNoTracking()
                .FirstOrDefaultAsync(va => va.VehicleId == request.VehicleId && va.ReleaseDate == null, cancellationToken)
                .ConfigureAwait(false);

            if (existingActiveAssignment is not null)
            {
                return AssignVehicleResult.Failure("The vehicle already has an active assignment. Please release the current assignment before assigning to a new owner.");
            }

            // Create or find VehicleOwner from provided details
            var vehicleOwner = new VehicleOwnerEntity
            {
                PANType = request.OwnerPanType,
                PANNumber = request.OwnerPanNumber,
                FirstName = request.OwnerFirstName,
                MiddleName = request.OwnerMiddleName,
                LastName = request.OwnerLastName,
                Address = request.OwnerAddress,
                Mobile = request.OwnerMobileNumber,
                IsActive = true,
                IsDeleted = false,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System",
                ModifiedBy = "System"
            };

            _dbContext.VehicleOwners.Add(vehicleOwner);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            var assignment = new VehicleAssignmentEntity
            {
                VehicleId = request.VehicleId,
                VehicleOwnerId = vehicleOwner.Id,
                AssignDate = request.AssignDate,
                ReleaseDate = null,
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System", // TODO: Replace with actual user from session
                ModifiedBy = "System"
            };

            _dbContext.VehicleAssignments.Add(assignment);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Vehicle assignment created successfully with ID {AssignmentId} for VehicleId {VehicleId}", assignment.Id, request.VehicleId);
            return AssignVehicleResult.Success(assignment.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while assigning vehicle {VehicleId}", request.VehicleId);
            return AssignVehicleResult.Failure("An unexpected error occurred while assigning the vehicle.");
        }
    }

    /// <inheritdoc />
    public async Task<UpdateVehicleAssignmentResult> UpdateAssignmentAsync(UpdateVehicleAssignmentRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _updateValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdateVehicleAssignmentResult.Failure(errorMessage);
            }

            var assignment = await _dbContext.VehicleAssignments
                .FirstOrDefaultAsync(va => va.Id == request.AssignmentId, cancellationToken)
                .ConfigureAwait(false);

            if (assignment is null)
            {
                return UpdateVehicleAssignmentResult.Failure("Vehicle assignment not found.");
            }

            // Business rule: If updating to active assignment, check for other active assignments
            if (request.ReleaseDate == null)
            {
                var existingActiveAssignment = await _dbContext.VehicleAssignments
                    .AsNoTracking()
                    .FirstOrDefaultAsync(va => va.VehicleId == request.VehicleId && va.ReleaseDate == null && va.Id != request.AssignmentId, cancellationToken)
                    .ConfigureAwait(false);

                if (existingActiveAssignment is not null)
                {
                    return UpdateVehicleAssignmentResult.Failure("The vehicle already has another active assignment. Please release the current assignment before updating.");
                }
            }

            assignment.VehicleId = request.VehicleId;
            assignment.VehicleOwnerId = request.VehicleOwnerId;
            assignment.AssignDate = request.AssignDate;
            assignment.ReleaseDate = request.ReleaseDate;
            assignment.IsActive = request.IsActive;
            assignment.ModifiedOn = DateTime.UtcNow;
            assignment.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Vehicle assignment '{AssignmentId}' updated successfully", request.AssignmentId);
            return UpdateVehicleAssignmentResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating vehicle assignment '{AssignmentId}'", request.AssignmentId);
            return UpdateVehicleAssignmentResult.Failure("An unexpected error occurred while updating the vehicle assignment.");
        }
    }

    /// <inheritdoc />
    public async Task<ReleaseVehicleResult> ReleaseVehicleAsync(ReleaseVehicleRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _releaseValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return ReleaseVehicleResult.Failure(errorMessage);
            }

            var assignment = await _dbContext.VehicleAssignments
                .FirstOrDefaultAsync(va => va.Id == request.AssignmentId, cancellationToken)
                .ConfigureAwait(false);

            if (assignment is null)
            {
                return ReleaseVehicleResult.Failure("Vehicle assignment not found.");
            }

            if (assignment.ReleaseDate != null)
            {
                return ReleaseVehicleResult.Failure("The vehicle has already been released.");
            }

            assignment.ReleaseDate = request.ReleaseDate;
            assignment.ModifiedOn = DateTime.UtcNow;
            assignment.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Vehicle assignment '{AssignmentId}' released successfully", request.AssignmentId);
            return ReleaseVehicleResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while releasing vehicle assignment '{AssignmentId}'", request.AssignmentId);
            return ReleaseVehicleResult.Failure("An unexpected error occurred while releasing the vehicle.");
        }
    }

    /// <inheritdoc />
    public async Task<DeleteVehicleAssignmentResult> DeleteAssignmentAsync(DeleteVehicleAssignmentRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _deleteValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return DeleteVehicleAssignmentResult.Failure(errorMessage);
            }

            var assignment = await _dbContext.VehicleAssignments
                .FirstOrDefaultAsync(va => va.Id == request.AssignmentId, cancellationToken)
                .ConfigureAwait(false);

            if (assignment is null)
            {
                return DeleteVehicleAssignmentResult.Failure("Vehicle assignment not found.");
            }

            assignment.IsDeleted = true;
            assignment.DeletedOn = DateTime.UtcNow;
            assignment.ModifiedOn = DateTime.UtcNow;
            assignment.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Vehicle assignment '{AssignmentId}' deleted successfully", request.AssignmentId);
            return DeleteVehicleAssignmentResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting vehicle assignment '{AssignmentId}'", request.AssignmentId);
            return DeleteVehicleAssignmentResult.Failure("An unexpected error occurred while deleting the vehicle assignment.");
        }
    }
}
