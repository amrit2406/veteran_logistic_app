using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using PaymentRegisterEntity = VeteranLogistics.Data.Entities.Administration.PaymentRegister;
using veteran_logistic.Transactions.PaymentRegisters.Contracts;
using veteran_logistic.Transactions.PaymentRegisters.Models;

namespace veteran_logistic.Transactions.PaymentRegisters.Services;

/// <summary>
/// Implementation of the payment register command service.
/// </summary>
public sealed class PaymentRegisterCommandService : IPaymentRegisterCommandService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ICreatePaymentRegisterValidator _createValidator;
    private readonly IUpdatePaymentRegisterValidator _updateValidator;
    private readonly IUpdatePaymentRegisterStatusValidator _updateStatusValidator;
    private readonly IDeletePaymentRegisterValidator _deleteValidator;
    private readonly ILogger<PaymentRegisterCommandService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentRegisterCommandService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="createValidator">The payment register creation validator.</param>
    /// <param name="updateValidator">The payment register update validator.</param>
    /// <param name="updateStatusValidator">The payment register status update validator.</param>
    /// <param name="deleteValidator">The delete payment register validator.</param>
    /// <param name="logger">The logger.</param>
    public PaymentRegisterCommandService(
        VeteranLogisticsDbContext dbContext,
        ICreatePaymentRegisterValidator createValidator,
        IUpdatePaymentRegisterValidator updateValidator,
        IUpdatePaymentRegisterStatusValidator updateStatusValidator,
        IDeletePaymentRegisterValidator deleteValidator,
        ILogger<PaymentRegisterCommandService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
        _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
        _updateStatusValidator = updateStatusValidator ?? throw new ArgumentNullException(nameof(updateStatusValidator));
        _deleteValidator = deleteValidator ?? throw new ArgumentNullException(nameof(deleteValidator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<CreatePaymentRegisterResult> CreatePaymentRegisterAsync(CreatePaymentRegisterRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _createValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return CreatePaymentRegisterResult.Failure(errorMessage);
            }

            // Get loading register data
            var loadingRegister = await _dbContext.LoadingRegisters
                .AsNoTracking()
                .FirstOrDefaultAsync(lr => lr.ChallanNumber == request.ChallanNumber, cancellationToken)
                .ConfigureAwait(false);

            if (loadingRegister is null)
            {
                return CreatePaymentRegisterResult.Failure("Loading register not found for the given challan number.");
            }

            // Get unloading register data
            var unloadingRegister = await _dbContext.UnloadingRegisters
                .AsNoTracking()
                .FirstOrDefaultAsync(ur => ur.ChallanNumber == request.ChallanNumber, cancellationToken)
                .ConfigureAwait(false);

            if (unloadingRegister is null)
            {
                return CreatePaymentRegisterResult.Failure("Unloading register not found for the given challan number.");
            }

            // Check if payment already exists
            var existingPayment = await _dbContext.PaymentRegisters
                .AsNoTracking()
                .FirstOrDefaultAsync(pr => pr.ChallanNumber == request.ChallanNumber, cancellationToken)
                .ConfigureAwait(false);

            if (existingPayment is not null)
            {
                return CreatePaymentRegisterResult.Failure("Payment register already exists for the given challan number.");
            }

            // Calculate Payable Amount
            // Gross Amount + Challan Money - TDS - Surcharge - Admin Charge
            var grossAmount = loadingRegister.GrossAmount;
            var tdsAmount = grossAmount * (request.TDSPercentage / 100);
            var payableAmount = grossAmount + request.ChallanMoney - tdsAmount - request.Surcharge - request.AdminCharge;

            var paymentRegister = new PaymentRegisterEntity
            {
                ChallanNumber = request.ChallanNumber,
                LoadingRegisterId = loadingRegister.Id,
                UnloadingRegisterId = unloadingRegister.Id,
                TPNumber = loadingRegister.TPNumber,
                VehicleNumber = loadingRegister.Vehicle != null ? loadingRegister.Vehicle.VehicleNumber : null,
                VehicleType = loadingRegister.VehicleType,
                MaterialName = loadingRegister.Material != null ? loadingRegister.Material.MaterialName : null,
                DriverCommission = loadingRegister.DriverCommission,
                LoadingDate = loadingRegister.LoadingDate,
                UnloadingDate = unloadingRegister.UnloadingDate,
                LoadingWeight = loadingRegister.LoadingWeight,
                UnloadingWeight = unloadingRegister.UnloadingWeight,
                PaymentDate = request.PaymentDate,
                PaymentLocationId = request.PaymentLocationId,
                PaymentType = request.PaymentType,
                HSDParty = request.HSDParty,
                Notes = request.Notes,
                Beneficiary = request.Beneficiary,
                PAN = request.PAN,
                UTRNumber = request.UTRNumber,
                MobileNumber = request.MobileNumber,
                AccountNumber = request.AccountNumber,
                IFSCCode = request.IFSCCode,
                BankName = request.BankName,
                TDSPercentage = request.TDSPercentage,
                ChallanMoney = request.ChallanMoney,
                Surcharge = request.Surcharge,
                AdminCharge = request.AdminCharge,
                GrossAmount = grossAmount,
                PayableAmount = payableAmount,
                PaymentStatus = request.PaymentStatus,
                IsActive = request.IsActive,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System", // TODO: Replace with actual user from session
                ModifiedBy = "System"
            };

            _dbContext.PaymentRegisters.Add(paymentRegister);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Payment register '{ChallanNumber}' created successfully with ID {PaymentRegisterId}", request.ChallanNumber, paymentRegister.Id);
            return CreatePaymentRegisterResult.Success(paymentRegister.Id, request.ChallanNumber);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating payment register");
            var errorMessage = $"Error: {ex.Message}";
            if (ex.InnerException != null)
            {
                errorMessage += $" | Inner: {ex.InnerException.Message}";
            }
            return CreatePaymentRegisterResult.Failure(errorMessage);
        }
    }

    /// <inheritdoc />
    public async Task<UpdatePaymentRegisterResult> UpdatePaymentRegisterAsync(UpdatePaymentRegisterRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _updateValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdatePaymentRegisterResult.Failure(errorMessage);
            }

            var paymentRegister = await _dbContext.PaymentRegisters
                .FirstOrDefaultAsync(pr => pr.Id == request.PaymentRegisterId, cancellationToken)
                .ConfigureAwait(false);

            if (paymentRegister is null)
            {
                return UpdatePaymentRegisterResult.Failure("Payment register not found.");
            }

            // Calculate Payable Amount
            var grossAmount = paymentRegister.GrossAmount;
            var tdsAmount = grossAmount * (request.TDSPercentage / 100);
            var payableAmount = grossAmount + request.ChallanMoney - tdsAmount - request.Surcharge - request.AdminCharge;

            paymentRegister.PaymentDate = request.PaymentDate;
            paymentRegister.PaymentLocationId = request.PaymentLocationId;
            paymentRegister.PaymentType = request.PaymentType;
            paymentRegister.HSDParty = request.HSDParty;
            paymentRegister.Notes = request.Notes;
            paymentRegister.Beneficiary = request.Beneficiary;
            paymentRegister.PAN = request.PAN;
            paymentRegister.UTRNumber = request.UTRNumber;
            paymentRegister.MobileNumber = request.MobileNumber;
            paymentRegister.AccountNumber = request.AccountNumber;
            paymentRegister.IFSCCode = request.IFSCCode;
            paymentRegister.BankName = request.BankName;
            paymentRegister.TDSPercentage = request.TDSPercentage;
            paymentRegister.ChallanMoney = request.ChallanMoney;
            paymentRegister.Surcharge = request.Surcharge;
            paymentRegister.AdminCharge = request.AdminCharge;
            paymentRegister.PayableAmount = payableAmount;
            paymentRegister.PaymentStatus = request.PaymentStatus;
            paymentRegister.IsActive = request.IsActive;
            paymentRegister.ModifiedOn = DateTime.UtcNow;
            paymentRegister.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Payment register '{PaymentRegisterId}' updated successfully", request.PaymentRegisterId);
            return UpdatePaymentRegisterResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating payment register '{PaymentRegisterId}'", request.PaymentRegisterId);
            var errorMessage = $"Error: {ex.Message}";
            if (ex.InnerException != null)
            {
                errorMessage += $" | Inner: {ex.InnerException.Message}";
            }
            return UpdatePaymentRegisterResult.Failure(errorMessage);
        }
    }

    /// <inheritdoc />
    public async Task<UpdatePaymentRegisterStatusResult> UpdatePaymentRegisterStatusAsync(UpdatePaymentRegisterStatusRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var paymentRegister = await _dbContext.PaymentRegisters
                .FirstOrDefaultAsync(pr => pr.Id == request.PaymentRegisterId, cancellationToken)
                .ConfigureAwait(false);

            if (paymentRegister is null)
            {
                return UpdatePaymentRegisterStatusResult.Failure("Payment register not found.");
            }

            var validationResult = _updateStatusValidator.Validate(request, paymentRegister.IsActive);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdatePaymentRegisterStatusResult.Failure(errorMessage);
            }

            paymentRegister.IsActive = request.IsActive;
            paymentRegister.ModifiedOn = DateTime.UtcNow;
            paymentRegister.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Payment register '{PaymentRegisterId}' status updated to {IsActive}", request.PaymentRegisterId, request.IsActive);
            return UpdatePaymentRegisterStatusResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating payment register status '{PaymentRegisterId}'", request.PaymentRegisterId);
            return UpdatePaymentRegisterStatusResult.Failure("An unexpected error occurred while updating the payment register status.");
        }
    }

    /// <inheritdoc />
    public async Task<DeletePaymentRegisterResult> DeletePaymentRegisterAsync(DeletePaymentRegisterRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _deleteValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return DeletePaymentRegisterResult.Failure(errorMessage);
            }

            var paymentRegister = await _dbContext.PaymentRegisters
                .FirstOrDefaultAsync(pr => pr.Id == request.PaymentRegisterId, cancellationToken)
                .ConfigureAwait(false);

            if (paymentRegister is null)
            {
                return DeletePaymentRegisterResult.Failure("Payment register not found.");
            }

            paymentRegister.IsDeleted = true;
            paymentRegister.DeletedOn = DateTime.UtcNow;
            paymentRegister.ModifiedOn = DateTime.UtcNow;
            paymentRegister.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Payment register '{PaymentRegisterId}' deleted successfully", request.PaymentRegisterId);
            return DeletePaymentRegisterResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting payment register '{PaymentRegisterId}'", request.PaymentRegisterId);
            return DeletePaymentRegisterResult.Failure("An unexpected error occurred while deleting the payment register.");
        }
    }
}
