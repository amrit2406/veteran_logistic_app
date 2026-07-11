using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using DestinationEntity = VeteranLogistics.Data.Entities.Administration.Destination;
using veteran_logistic.Masters.Destinations.Contracts;
using veteran_logistic.Masters.Destinations.Models;

namespace veteran_logistic.Masters.Destinations.Services;

/// <summary>
/// Implementation of the destination command service.
/// </summary>
public sealed class DestinationCommandService : IDestinationCommandService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ICreateDestinationValidator _createValidator;
    private readonly IUpdateDestinationValidator _updateValidator;
    private readonly IUpdateDestinationStatusValidator _updateStatusValidator;
    private readonly IDeleteDestinationValidator _deleteValidator;
    private readonly ILogger<DestinationCommandService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DestinationCommandService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="createValidator">The destination creation validator.</param>
    /// <param name="updateValidator">The destination update validator.</param>
    /// <param name="updateStatusValidator">The destination status update validator.</param>
    /// <param name="deleteValidator">The delete destination validator.</param>
    /// <param name="logger">The logger.</param>
    public DestinationCommandService(
        VeteranLogisticsDbContext dbContext,
        ICreateDestinationValidator createValidator,
        IUpdateDestinationValidator updateValidator,
        IUpdateDestinationStatusValidator updateStatusValidator,
        IDeleteDestinationValidator deleteValidator,
        ILogger<DestinationCommandService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
        _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
        _updateStatusValidator = updateStatusValidator ?? throw new ArgumentNullException(nameof(updateStatusValidator));
        _deleteValidator = deleteValidator ?? throw new ArgumentNullException(nameof(deleteValidator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<CreateDestinationResult> CreateDestinationAsync(CreateDestinationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _createValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return CreateDestinationResult.Failure(errorMessage);
            }

            // Check for duplicate DestinationCode
            var existingDestinationByCode = await _dbContext.Destinations
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.DestinationCode == request.DestinationCode, cancellationToken)
                .ConfigureAwait(false);

            if (existingDestinationByCode is not null)
            {
                return CreateDestinationResult.Failure("A destination with this destination code already exists.");
            }

            var destination = new DestinationEntity
            {
                DestinationCode = request.DestinationCode,
                DestinationName = request.DestinationName,
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                City = request.City,
                State = request.State,
                Country = request.Country,
                PostalCode = request.PostalCode,
                ContactPerson = request.ContactPerson,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                GSTNumber = request.GSTNumber,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Remarks = request.Remarks,
                IsActive = request.IsActive,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System", // TODO: Replace with actual user from session
                ModifiedBy = "System"
            };

            _dbContext.Destinations.Add(destination);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Destination '{DestinationCode}' created successfully with ID {DestinationId}", request.DestinationCode, destination.Id);
            return CreateDestinationResult.Success(destination.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating destination '{DestinationCode}'", request.DestinationCode);
            return CreateDestinationResult.Failure("An unexpected error occurred while creating the destination.");
        }
    }

    /// <inheritdoc />
    public async Task<UpdateDestinationResult> UpdateDestinationAsync(UpdateDestinationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _updateValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdateDestinationResult.Failure(errorMessage);
            }

            var destination = await _dbContext.Destinations
                .FirstOrDefaultAsync(d => d.Id == request.DestinationId, cancellationToken)
                .ConfigureAwait(false);

            if (destination is null)
            {
                return UpdateDestinationResult.Failure("Destination not found.");
            }

            // Check for duplicate DestinationCode (excluding current destination)
            var existingDestinationByCode = await _dbContext.Destinations
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.DestinationCode == request.DestinationCode && d.Id != request.DestinationId, cancellationToken)
                .ConfigureAwait(false);

            if (existingDestinationByCode is not null)
            {
                return UpdateDestinationResult.Failure("A destination with this destination code already exists.");
            }

            destination.DestinationCode = request.DestinationCode;
            destination.DestinationName = request.DestinationName;
            destination.AddressLine1 = request.AddressLine1;
            destination.AddressLine2 = request.AddressLine2;
            destination.City = request.City;
            destination.State = request.State;
            destination.Country = request.Country;
            destination.PostalCode = request.PostalCode;
            destination.ContactPerson = request.ContactPerson;
            destination.PhoneNumber = request.PhoneNumber;
            destination.Email = request.Email;
            destination.GSTNumber = request.GSTNumber;
            destination.Latitude = request.Latitude;
            destination.Longitude = request.Longitude;
            destination.Remarks = request.Remarks;
            destination.IsActive = request.IsActive;
            destination.ModifiedOn = DateTime.UtcNow;
            destination.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Destination '{DestinationId}' updated successfully", request.DestinationId);
            return UpdateDestinationResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating destination '{DestinationId}'", request.DestinationId);
            return UpdateDestinationResult.Failure("An unexpected error occurred while updating the destination.");
        }
    }

    /// <inheritdoc />
    public async Task<UpdateDestinationStatusResult> UpdateDestinationStatusAsync(UpdateDestinationStatusRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var destination = await _dbContext.Destinations
                .FirstOrDefaultAsync(d => d.Id == request.DestinationId, cancellationToken)
                .ConfigureAwait(false);

            if (destination is null)
            {
                return UpdateDestinationStatusResult.Failure("Destination not found.");
            }

            var validationResult = _updateStatusValidator.Validate(request, destination.IsActive);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdateDestinationStatusResult.Failure(errorMessage);
            }

            destination.IsActive = request.IsActive;
            destination.ModifiedOn = DateTime.UtcNow;
            destination.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Destination '{DestinationId}' status updated to {IsActive}", request.DestinationId, request.IsActive);
            return UpdateDestinationStatusResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating destination status '{DestinationId}'", request.DestinationId);
            return UpdateDestinationStatusResult.Failure("An unexpected error occurred while updating the destination status.");
        }
    }

    /// <inheritdoc />
    public async Task<DeleteDestinationResult> DeleteDestinationAsync(DeleteDestinationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _deleteValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return DeleteDestinationResult.Failure(errorMessage);
            }

            var destination = await _dbContext.Destinations
                .FirstOrDefaultAsync(d => d.Id == request.DestinationId, cancellationToken)
                .ConfigureAwait(false);

            if (destination is null)
            {
                return DeleteDestinationResult.Failure("Destination not found.");
            }

            destination.IsDeleted = true;
            destination.DeletedOn = DateTime.UtcNow;
            destination.ModifiedOn = DateTime.UtcNow;
            destination.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Destination '{DestinationId}' deleted successfully", request.DestinationId);
            return DeleteDestinationResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting destination '{DestinationId}'", request.DestinationId);
            return DeleteDestinationResult.Failure("An unexpected error occurred while deleting the destination.");
        }
    }
}
