using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using PaymentLocationEntity = VeteranLogistics.Data.Entities.Administration.PaymentLocation;
using veteran_logistic.Masters.PaymentLocations.Contracts;
using veteran_logistic.Masters.PaymentLocations.Models;

namespace veteran_logistic.Masters.PaymentLocations.Services;

/// <summary>
/// Implementation of the payment location command service.
/// </summary>
public sealed class PaymentLocationCommandService : IPaymentLocationCommandService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ICreatePaymentLocationValidator _createValidator;
    private readonly IUpdatePaymentLocationValidator _updateValidator;
    private readonly IUpdatePaymentLocationStatusValidator _updateStatusValidator;
    private readonly IDeletePaymentLocationValidator _deleteValidator;
    private readonly ILogger<PaymentLocationCommandService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentLocationCommandService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="createValidator">The payment location creation validator.</param>
    /// <param name="updateValidator">The payment location update validator.</param>
    /// <param name="updateStatusValidator">The payment location status update validator.</param>
    /// <param name="deleteValidator">The delete payment location validator.</param>
    /// <param name="logger">The logger.</param>
    public PaymentLocationCommandService(
        VeteranLogisticsDbContext dbContext,
        ICreatePaymentLocationValidator createValidator,
        IUpdatePaymentLocationValidator updateValidator,
        IUpdatePaymentLocationStatusValidator updateStatusValidator,
        IDeletePaymentLocationValidator deleteValidator,
        ILogger<PaymentLocationCommandService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
        _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
        _updateStatusValidator = updateStatusValidator ?? throw new ArgumentNullException(nameof(updateStatusValidator));
        _deleteValidator = deleteValidator ?? throw new ArgumentNullException(nameof(deleteValidator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<CreatePaymentLocationResult> CreatePaymentLocationAsync(CreatePaymentLocationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _createValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return CreatePaymentLocationResult.Failure(errorMessage);
            }

            // Check for duplicate PaymentLocationName
            var existingPaymentLocationByName = await _dbContext.PaymentLocations
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PaymentLocationName == request.PaymentLocationName, cancellationToken)
                .ConfigureAwait(false);

            if (existingPaymentLocationByName is not null)
            {
                return CreatePaymentLocationResult.Failure("A payment location with this name already exists.");
            }

            var paymentLocation = new PaymentLocationEntity
            {
                PaymentLocationName = request.PaymentLocationName,
                IsActive = request.IsActive,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System", // TODO: Replace with actual user from session
                ModifiedBy = "System"
            };

            _dbContext.PaymentLocations.Add(paymentLocation);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Payment location '{PaymentLocationName}' created successfully with ID {PaymentLocationId}", request.PaymentLocationName, paymentLocation.Id);
            return CreatePaymentLocationResult.Success(paymentLocation.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating payment location '{PaymentLocationName}'", request.PaymentLocationName);
            return CreatePaymentLocationResult.Failure("An unexpected error occurred while creating the payment location.");
        }
    }

    /// <inheritdoc />
    public async Task<UpdatePaymentLocationResult> UpdatePaymentLocationAsync(UpdatePaymentLocationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _updateValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdatePaymentLocationResult.Failure(errorMessage);
            }

            var paymentLocation = await _dbContext.PaymentLocations
                .FirstOrDefaultAsync(p => p.Id == request.PaymentLocationId, cancellationToken)
                .ConfigureAwait(false);

            if (paymentLocation is null)
            {
                return UpdatePaymentLocationResult.Failure("Payment location not found.");
            }

            // Check for duplicate PaymentLocationName (excluding current payment location)
            var existingPaymentLocationByName = await _dbContext.PaymentLocations
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PaymentLocationName == request.PaymentLocationName && p.Id != request.PaymentLocationId, cancellationToken)
                .ConfigureAwait(false);

            if (existingPaymentLocationByName is not null)
            {
                return UpdatePaymentLocationResult.Failure("A payment location with this name already exists.");
            }

            paymentLocation.PaymentLocationName = request.PaymentLocationName;
            paymentLocation.IsActive = request.IsActive;
            paymentLocation.ModifiedOn = DateTime.UtcNow;
            paymentLocation.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Payment location '{PaymentLocationId}' updated successfully", request.PaymentLocationId);
            return UpdatePaymentLocationResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating payment location '{PaymentLocationId}'", request.PaymentLocationId);
            return UpdatePaymentLocationResult.Failure("An unexpected error occurred while updating the payment location.");
        }
    }

    /// <inheritdoc />
    public async Task<UpdatePaymentLocationStatusResult> UpdatePaymentLocationStatusAsync(UpdatePaymentLocationStatusRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var paymentLocation = await _dbContext.PaymentLocations
                .FirstOrDefaultAsync(p => p.Id == request.PaymentLocationId, cancellationToken)
                .ConfigureAwait(false);

            if (paymentLocation is null)
            {
                return UpdatePaymentLocationStatusResult.Failure("Payment location not found.");
            }

            var validationResult = _updateStatusValidator.Validate(request, paymentLocation.IsActive);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdatePaymentLocationStatusResult.Failure(errorMessage);
            }

            paymentLocation.IsActive = request.IsActive;
            paymentLocation.ModifiedOn = DateTime.UtcNow;
            paymentLocation.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Payment location '{PaymentLocationId}' status updated to {IsActive}", request.PaymentLocationId, request.IsActive);
            return UpdatePaymentLocationStatusResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating payment location status '{PaymentLocationId}'", request.PaymentLocationId);
            return UpdatePaymentLocationStatusResult.Failure("An unexpected error occurred while updating the payment location status.");
        }
    }

    /// <inheritdoc />
    public async Task<DeletePaymentLocationResult> DeletePaymentLocationAsync(DeletePaymentLocationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _deleteValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return DeletePaymentLocationResult.Failure(errorMessage);
            }

            var paymentLocation = await _dbContext.PaymentLocations
                .FirstOrDefaultAsync(p => p.Id == request.PaymentLocationId, cancellationToken)
                .ConfigureAwait(false);

            if (paymentLocation is null)
            {
                return DeletePaymentLocationResult.Failure("Payment location not found.");
            }

            paymentLocation.IsDeleted = true;
            paymentLocation.DeletedOn = DateTime.UtcNow;
            paymentLocation.ModifiedOn = DateTime.UtcNow;
            paymentLocation.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Payment location '{PaymentLocationId}' deleted successfully", request.PaymentLocationId);
            return DeletePaymentLocationResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting payment location '{PaymentLocationId}'", request.PaymentLocationId);
            return DeletePaymentLocationResult.Failure("An unexpected error occurred while deleting the payment location.");
        }
    }
}
