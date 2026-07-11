using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using SourceEntity = VeteranLogistics.Data.Entities.Administration.Source;
using veteran_logistic.Masters.Sources.Contracts;
using veteran_logistic.Masters.Sources.Models;

namespace veteran_logistic.Masters.Sources.Services;

/// <summary>
/// Implementation of the source command service.
/// </summary>
public sealed class SourceCommandService : ISourceCommandService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ICreateSourceValidator _createValidator;
    private readonly IUpdateSourceValidator _updateValidator;
    private readonly IUpdateSourceStatusValidator _updateStatusValidator;
    private readonly IDeleteSourceValidator _deleteValidator;
    private readonly ILogger<SourceCommandService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SourceCommandService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="createValidator">The source creation validator.</param>
    /// <param name="updateValidator">The source update validator.</param>
    /// <param name="updateStatusValidator">The source status update validator.</param>
    /// <param name="deleteValidator">The delete source validator.</param>
    /// <param name="logger">The logger.</param>
    public SourceCommandService(
        VeteranLogisticsDbContext dbContext,
        ICreateSourceValidator createValidator,
        IUpdateSourceValidator updateValidator,
        IUpdateSourceStatusValidator updateStatusValidator,
        IDeleteSourceValidator deleteValidator,
        ILogger<SourceCommandService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
        _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
        _updateStatusValidator = updateStatusValidator ?? throw new ArgumentNullException(nameof(updateStatusValidator));
        _deleteValidator = deleteValidator ?? throw new ArgumentNullException(nameof(deleteValidator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<CreateSourceResult> CreateSourceAsync(CreateSourceRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _createValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return CreateSourceResult.Failure(errorMessage);
            }

            // Check for duplicate SourceCode
            var existingSourceByCode = await _dbContext.Sources
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SourceCode == request.SourceCode, cancellationToken)
                .ConfigureAwait(false);

            if (existingSourceByCode is not null)
            {
                return CreateSourceResult.Failure("A source with this source code already exists.");
            }

            var source = new SourceEntity
            {
                SourceCode = request.SourceCode,
                SourceName = request.SourceName,
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

            _dbContext.Sources.Add(source);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Source '{SourceCode}' created successfully with ID {SourceId}", request.SourceCode, source.Id);
            return CreateSourceResult.Success(source.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating source '{SourceCode}'", request.SourceCode);
            return CreateSourceResult.Failure("An unexpected error occurred while creating the source.");
        }
    }

    /// <inheritdoc />
    public async Task<UpdateSourceResult> UpdateSourceAsync(UpdateSourceRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _updateValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdateSourceResult.Failure(errorMessage);
            }

            var source = await _dbContext.Sources
                .FirstOrDefaultAsync(s => s.Id == request.SourceId, cancellationToken)
                .ConfigureAwait(false);

            if (source is null)
            {
                return UpdateSourceResult.Failure("Source not found.");
            }

            // Check for duplicate SourceCode (excluding current source)
            var existingSourceByCode = await _dbContext.Sources
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SourceCode == request.SourceCode && s.Id != request.SourceId, cancellationToken)
                .ConfigureAwait(false);

            if (existingSourceByCode is not null)
            {
                return UpdateSourceResult.Failure("A source with this source code already exists.");
            }

            source.SourceCode = request.SourceCode;
            source.SourceName = request.SourceName;
            source.AddressLine1 = request.AddressLine1;
            source.AddressLine2 = request.AddressLine2;
            source.City = request.City;
            source.State = request.State;
            source.Country = request.Country;
            source.PostalCode = request.PostalCode;
            source.ContactPerson = request.ContactPerson;
            source.PhoneNumber = request.PhoneNumber;
            source.Email = request.Email;
            source.GSTNumber = request.GSTNumber;
            source.Latitude = request.Latitude;
            source.Longitude = request.Longitude;
            source.Remarks = request.Remarks;
            source.IsActive = request.IsActive;
            source.ModifiedOn = DateTime.UtcNow;
            source.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Source '{SourceId}' updated successfully", request.SourceId);
            return UpdateSourceResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating source '{SourceId}'", request.SourceId);
            return UpdateSourceResult.Failure("An unexpected error occurred while updating the source.");
        }
    }

    /// <inheritdoc />
    public async Task<UpdateSourceStatusResult> UpdateSourceStatusAsync(UpdateSourceStatusRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var source = await _dbContext.Sources
                .FirstOrDefaultAsync(s => s.Id == request.SourceId, cancellationToken)
                .ConfigureAwait(false);

            if (source is null)
            {
                return UpdateSourceStatusResult.Failure("Source not found.");
            }

            var validationResult = _updateStatusValidator.Validate(request, source.IsActive);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdateSourceStatusResult.Failure(errorMessage);
            }

            source.IsActive = request.IsActive;
            source.ModifiedOn = DateTime.UtcNow;
            source.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Source '{SourceId}' status updated to {IsActive}", request.SourceId, request.IsActive);
            return UpdateSourceStatusResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating source status '{SourceId}'", request.SourceId);
            return UpdateSourceStatusResult.Failure("An unexpected error occurred while updating the source status.");
        }
    }

    /// <inheritdoc />
    public async Task<DeleteSourceResult> DeleteSourceAsync(DeleteSourceRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _deleteValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return DeleteSourceResult.Failure(errorMessage);
            }

            var source = await _dbContext.Sources
                .FirstOrDefaultAsync(s => s.Id == request.SourceId, cancellationToken)
                .ConfigureAwait(false);

            if (source is null)
            {
                return DeleteSourceResult.Failure("Source not found.");
            }

            source.IsDeleted = true;
            source.DeletedOn = DateTime.UtcNow;
            source.ModifiedOn = DateTime.UtcNow;
            source.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Source '{SourceId}' deleted successfully", request.SourceId);
            return DeleteSourceResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting source '{SourceId}'", request.SourceId);
            return DeleteSourceResult.Failure("An unexpected error occurred while deleting the source.");
        }
    }
}
