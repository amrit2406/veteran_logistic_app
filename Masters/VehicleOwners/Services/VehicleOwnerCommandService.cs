using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using VehicleOwnerEntity = VeteranLogistics.Data.Entities.Administration.VehicleOwner;
using veteran_logistic.Masters.VehicleOwners.Contracts;
using veteran_logistic.Masters.VehicleOwners.Models;

namespace veteran_logistic.Masters.VehicleOwners.Services;

/// <summary>
/// Implementation of the vehicle owner command service.
/// </summary>
public sealed class VehicleOwnerCommandService : IVehicleOwnerCommandService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ICreateVehicleOwnerValidator _createValidator;
    private readonly IUpdateVehicleOwnerValidator _updateValidator;
    private readonly IUpdateVehicleOwnerStatusValidator _updateStatusValidator;
    private readonly IDeleteVehicleOwnerValidator _deleteValidator;
    private readonly ILogger<VehicleOwnerCommandService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="VehicleOwnerCommandService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="createValidator">The vehicle owner creation validator.</param>
    /// <param name="updateValidator">The vehicle owner update validator.</param>
    /// <param name="updateStatusValidator">The vehicle owner status update validator.</param>
    /// <param name="deleteValidator">The delete vehicle owner validator.</param>
    /// <param name="logger">The logger.</param>
    public VehicleOwnerCommandService(
        VeteranLogisticsDbContext dbContext,
        ICreateVehicleOwnerValidator createValidator,
        IUpdateVehicleOwnerValidator updateValidator,
        IUpdateVehicleOwnerStatusValidator updateStatusValidator,
        IDeleteVehicleOwnerValidator deleteValidator,
        ILogger<VehicleOwnerCommandService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
        _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
        _updateStatusValidator = updateStatusValidator ?? throw new ArgumentNullException(nameof(updateStatusValidator));
        _deleteValidator = deleteValidator ?? throw new ArgumentNullException(nameof(deleteValidator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<CreateVehicleOwnerResult> CreateVehicleOwnerAsync(CreateVehicleOwnerRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _createValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return CreateVehicleOwnerResult.Failure(errorMessage);
            }

            // Check for duplicate PAN Number
            var existingOwnerByPAN = await _dbContext.VehicleOwners
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.PANNumber == request.PANNumber, cancellationToken)
                .ConfigureAwait(false);

            if (existingOwnerByPAN is not null)
            {
                return CreateVehicleOwnerResult.Failure("A vehicle owner with this PAN number already exists.");
            }

            // Auto-generate Owner Code
            var ownerCode = await GenerateOwnerCodeAsync(cancellationToken).ConfigureAwait(false);

            var vehicleOwner = new VehicleOwnerEntity
            {
                OwnerCode = ownerCode,
                PANType = request.PANType,
                PANNumber = request.PANNumber,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                CompanyName = request.CompanyName,
                City = request.City,
                State = request.State,
                Address = request.Address,
                Phone = request.Phone,
                Mobile = request.Mobile,
                Email = request.Email,
                Fax = request.Fax,
                IsActive = request.IsActive,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System", // TODO: Replace with actual user from session
                ModifiedBy = "System"
            };

            _dbContext.VehicleOwners.Add(vehicleOwner);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Vehicle owner '{OwnerCode}' created successfully with ID {VehicleOwnerId}", ownerCode, vehicleOwner.Id);
            return CreateVehicleOwnerResult.Success(vehicleOwner.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating vehicle owner");
            return CreateVehicleOwnerResult.Failure("An unexpected error occurred while creating the vehicle owner.");
        }
    }

    /// <inheritdoc />
    public async Task<UpdateVehicleOwnerResult> UpdateVehicleOwnerAsync(UpdateVehicleOwnerRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _updateValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdateVehicleOwnerResult.Failure(errorMessage);
            }

            var vehicleOwner = await _dbContext.VehicleOwners
                .FirstOrDefaultAsync(o => o.Id == request.VehicleOwnerId, cancellationToken)
                .ConfigureAwait(false);

            if (vehicleOwner is null)
            {
                return UpdateVehicleOwnerResult.Failure("Vehicle owner not found.");
            }

            // Check for duplicate PAN Number (excluding current vehicle owner)
            var existingOwnerByPAN = await _dbContext.VehicleOwners
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.PANNumber == request.PANNumber && o.Id != request.VehicleOwnerId, cancellationToken)
                .ConfigureAwait(false);

            if (existingOwnerByPAN is not null)
            {
                return UpdateVehicleOwnerResult.Failure("A vehicle owner with this PAN number already exists.");
            }

            vehicleOwner.PANType = request.PANType;
            vehicleOwner.PANNumber = request.PANNumber;
            vehicleOwner.FirstName = request.FirstName;
            vehicleOwner.MiddleName = request.MiddleName;
            vehicleOwner.LastName = request.LastName;
            vehicleOwner.CompanyName = request.CompanyName;
            vehicleOwner.City = request.City;
            vehicleOwner.State = request.State;
            vehicleOwner.Address = request.Address;
            vehicleOwner.Phone = request.Phone;
            vehicleOwner.Mobile = request.Mobile;
            vehicleOwner.Email = request.Email;
            vehicleOwner.Fax = request.Fax;
            vehicleOwner.IsActive = request.IsActive;
            vehicleOwner.ModifiedOn = DateTime.UtcNow;
            vehicleOwner.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Vehicle owner '{VehicleOwnerId}' updated successfully", request.VehicleOwnerId);
            return UpdateVehicleOwnerResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating vehicle owner '{VehicleOwnerId}'", request.VehicleOwnerId);
            return UpdateVehicleOwnerResult.Failure("An unexpected error occurred while updating the vehicle owner.");
        }
    }

    /// <inheritdoc />
    public async Task<UpdateVehicleOwnerStatusResult> UpdateVehicleOwnerStatusAsync(UpdateVehicleOwnerStatusRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var vehicleOwner = await _dbContext.VehicleOwners
                .FirstOrDefaultAsync(o => o.Id == request.VehicleOwnerId, cancellationToken)
                .ConfigureAwait(false);

            if (vehicleOwner is null)
            {
                return UpdateVehicleOwnerStatusResult.Failure("Vehicle owner not found.");
            }

            var validationResult = _updateStatusValidator.Validate(request, vehicleOwner.IsActive);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdateVehicleOwnerStatusResult.Failure(errorMessage);
            }

            vehicleOwner.IsActive = request.IsActive;
            vehicleOwner.ModifiedOn = DateTime.UtcNow;
            vehicleOwner.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Vehicle owner '{VehicleOwnerId}' status updated to {IsActive}", request.VehicleOwnerId, request.IsActive);
            return UpdateVehicleOwnerStatusResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating vehicle owner status '{VehicleOwnerId}'", request.VehicleOwnerId);
            return UpdateVehicleOwnerStatusResult.Failure("An unexpected error occurred while updating the vehicle owner status.");
        }
    }

    /// <inheritdoc />
    public async Task<DeleteVehicleOwnerResult> DeleteVehicleOwnerAsync(DeleteVehicleOwnerRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _deleteValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return DeleteVehicleOwnerResult.Failure(errorMessage);
            }

            var vehicleOwner = await _dbContext.VehicleOwners
                .FirstOrDefaultAsync(o => o.Id == request.VehicleOwnerId, cancellationToken)
                .ConfigureAwait(false);

            if (vehicleOwner is null)
            {
                return DeleteVehicleOwnerResult.Failure("Vehicle owner not found.");
            }

            vehicleOwner.IsDeleted = true;
            vehicleOwner.DeletedOn = DateTime.UtcNow;
            vehicleOwner.ModifiedOn = DateTime.UtcNow;
            vehicleOwner.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Vehicle owner '{VehicleOwnerId}' deleted successfully", request.VehicleOwnerId);
            return DeleteVehicleOwnerResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting vehicle owner '{VehicleOwnerId}'", request.VehicleOwnerId);
            return DeleteVehicleOwnerResult.Failure("An unexpected error occurred while deleting the vehicle owner.");
        }
    }

    /// <summary>
    /// Generates a unique owner code.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A unique owner code.</returns>
    private async Task<string> GenerateOwnerCodeAsync(CancellationToken cancellationToken)
    {
        // Get the highest existing owner code
        var lastOwnerCode = await _dbContext.VehicleOwners
            .AsNoTracking()
            .Where(o => o.OwnerCode.StartsWith("VO"))
            .OrderByDescending(o => o.OwnerCode)
            .Select(o => o.OwnerCode)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        int nextNumber;
        if (string.IsNullOrWhiteSpace(lastOwnerCode))
        {
            nextNumber = 1;
        }
        else
        {
            // Extract numeric part from owner code (e.g., "VO0001" -> 1)
            var numericPart = lastOwnerCode.Substring(2);
            if (int.TryParse(numericPart, out var lastNumber))
            {
                nextNumber = lastNumber + 1;
            }
            else
            {
                nextNumber = 1;
            }
        }

        // Format as "VO" + 4-digit number (e.g., VO0001, VO0002)
        return $"VO{nextNumber:D4}";
    }
}
