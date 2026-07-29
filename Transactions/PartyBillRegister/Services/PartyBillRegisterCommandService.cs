using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using VeteranLogistics.Data.Entities.Administration;
using veteran_logistic.Transactions.PartyBillRegister.Contracts;
using veteran_logistic.Transactions.PartyBillRegister.Models;

namespace veteran_logistic.Transactions.PartyBillRegister.Services;

/// <summary>
/// Service for party bill register command operations.
/// </summary>
public sealed class PartyBillRegisterCommandService : IPartyBillRegisterCommandService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<PartyBillRegisterCommandService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PartyBillRegisterCommandService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    public PartyBillRegisterCommandService(VeteranLogisticsDbContext dbContext, ILogger<PartyBillRegisterCommandService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<CreatePartyBillRegisterResult> CreatePartyBillRegisterAsync(CreatePartyBillRegisterRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating party bill register for party ID: {PartyId}", request.PartyId);

        try
        {
            // Generate bill number
            var billNumber = await GenerateBillNumberAsync(cancellationToken).ConfigureAwait(false);

            // Retrieve selected loading registers
            var loadingRegisters = await _dbContext.LoadingRegisters
                .AsNoTracking()
                .Include(lr => lr.Vehicle)
                .Where(lr => request.SelectedLoadingRegisterIds.Contains(lr.Id))
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            if (loadingRegisters.Count != request.SelectedLoadingRegisterIds.Count)
            {
                _logger.LogWarning("Some selected loading registers were not found");
                return new CreatePartyBillRegisterResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Some selected loading registers were not found."
                };
            }

            // Calculate totals
            var totalRecords = loadingRegisters.Count;
            var totalMaterialWeight = loadingRegisters.Sum(lr => lr.LoadingWeight);
            var totalAmount = loadingRegisters.Sum(lr => lr.GrossAmount);
            var grandTotal = totalAmount + request.ChargeAmount1 + request.ChargeAmount2;

            // Create party bill register
            var partyBillRegister = new VeteranLogistics.Data.Entities.Administration.PartyBillRegister
            {
                BillNumber = billNumber,
                BillDate = request.BillDate,
                PartyId = request.PartyId,
                ThirdPartyName = request.ThirdPartyName,
                PermitNumber = request.PermitNumber,
                ConsignorId = request.ConsignorId,
                DestinationId = request.DestinationId,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                TotalRecords = totalRecords,
                TotalMaterialWeight = totalMaterialWeight,
                TotalAmount = totalAmount,
                ChargeHead1 = request.ChargeHead1,
                ChargeType1 = request.ChargeType1,
                ChargeAmount1 = request.ChargeAmount1,
                ChargeHead2 = request.ChargeHead2,
                ChargeType2 = request.ChargeType2,
                ChargeAmount2 = request.ChargeAmount2,
                GrandTotal = grandTotal,
                Remarks = request.Remarks,
                IsActive = true,
                CreatedBy = request.CreatedBy,
                CreatedOn = DateTime.UtcNow
            };

            _dbContext.PartyBillRegisters.Add(partyBillRegister);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            // Create party bill register details
            foreach (var loadingRegister in loadingRegisters)
            {
                var detail = new PartyBillRegisterDetail
                {
                    PartyBillRegisterId = partyBillRegister.Id,
                    LoadingRegisterId = loadingRegister.Id,
                    TPNumber = loadingRegister.TPNumber,
                    ChallanNumber = loadingRegister.ChallanNumber,
                    VehicleNumber = loadingRegister.Vehicle?.VehicleNumber,
                    LoadingDate = loadingRegister.LoadingDate,
                    MaterialWeight = loadingRegister.LoadingWeight,
                    BillingRate = loadingRegister.Rate,
                    DriverCommission = loadingRegister.DriverCommission,
                    Amount = loadingRegister.GrossAmount,
                    CreatedOn = DateTime.UtcNow
                };

                _dbContext.PartyBillRegisterDetails.Add(detail);
            }

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Successfully created party bill register with ID: {PartyBillRegisterId} and Bill Number: {BillNumber}", partyBillRegister.Id, billNumber);

            return new CreatePartyBillRegisterResult
            {
                PartyBillRegisterId = partyBillRegister.Id,
                BillNumber = billNumber,
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating party bill register for party ID: {PartyId}", request.PartyId);
            return new CreatePartyBillRegisterResult
            {
                IsSuccess = false,
                ErrorMessage = "An error occurred while creating the party bill register."
            };
        }
    }

    /// <inheritdoc />
    public async Task<UpdatePartyBillRegisterResult> UpdatePartyBillRegisterAsync(UpdatePartyBillRegisterRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating party bill register with ID: {PartyBillRegisterId}", request.PartyBillRegisterId);

        try
        {
            var partyBillRegister = await _dbContext.PartyBillRegisters
                .Include(pbr => pbr.PartyBillRegisterDetails)
                .FirstOrDefaultAsync(pbr => pbr.Id == request.PartyBillRegisterId, cancellationToken)
                .ConfigureAwait(false);

            if (partyBillRegister is null)
            {
                _logger.LogWarning("Party bill register with ID {PartyBillRegisterId} not found", request.PartyBillRegisterId);
                return new UpdatePartyBillRegisterResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Party bill register not found."
                };
            }

            // Update header fields
            partyBillRegister.BillDate = request.BillDate;
            partyBillRegister.PartyId = request.PartyId;
            partyBillRegister.ThirdPartyName = request.ThirdPartyName;
            partyBillRegister.PermitNumber = request.PermitNumber;
            partyBillRegister.ChargeHead1 = request.ChargeHead1;
            partyBillRegister.ChargeType1 = request.ChargeType1;
            partyBillRegister.ChargeAmount1 = request.ChargeAmount1;
            partyBillRegister.ChargeHead2 = request.ChargeHead2;
            partyBillRegister.ChargeType2 = request.ChargeType2;
            partyBillRegister.ChargeAmount2 = request.ChargeAmount2;
            partyBillRegister.Remarks = request.Remarks;
            partyBillRegister.ModifiedBy = request.ModifiedBy;
            partyBillRegister.ModifiedOn = DateTime.UtcNow;

            // Recalculate grand total
            partyBillRegister.GrandTotal = partyBillRegister.TotalAmount + request.ChargeAmount1 + request.ChargeAmount2;

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Successfully updated party bill register with ID: {PartyBillRegisterId}", request.PartyBillRegisterId);

            return new UpdatePartyBillRegisterResult
            {
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating party bill register with ID: {PartyBillRegisterId}", request.PartyBillRegisterId);
            return new UpdatePartyBillRegisterResult
            {
                IsSuccess = false,
                ErrorMessage = "An error occurred while updating the party bill register."
            };
        }
    }

    /// <inheritdoc />
    public async Task<UpdatePartyBillRegisterStatusResult> UpdatePartyBillRegisterStatusAsync(UpdatePartyBillRegisterStatusRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating party bill register status for ID: {PartyBillRegisterId} to IsActive: {IsActive}", request.PartyBillRegisterId, request.IsActive);

        try
        {
            var partyBillRegister = await _dbContext.PartyBillRegisters
                .FirstOrDefaultAsync(pbr => pbr.Id == request.PartyBillRegisterId, cancellationToken)
                .ConfigureAwait(false);

            if (partyBillRegister is null)
            {
                _logger.LogWarning("Party bill register with ID {PartyBillRegisterId} not found", request.PartyBillRegisterId);
                return new UpdatePartyBillRegisterStatusResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Party bill register not found."
                };
            }

            partyBillRegister.IsActive = request.IsActive;
            partyBillRegister.ModifiedBy = request.ModifiedBy;
            partyBillRegister.ModifiedOn = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Successfully updated party bill register status for ID: {PartyBillRegisterId}", request.PartyBillRegisterId);

            return new UpdatePartyBillRegisterStatusResult
            {
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating party bill register status for ID: {PartyBillRegisterId}", request.PartyBillRegisterId);
            return new UpdatePartyBillRegisterStatusResult
            {
                IsSuccess = false,
                ErrorMessage = "An error occurred while updating the party bill register status."
            };
        }
    }

    /// <inheritdoc />
    public async Task<DeletePartyBillRegisterResult> DeletePartyBillRegisterAsync(DeletePartyBillRegisterRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Soft deleting party bill register with ID: {PartyBillRegisterId}", request.PartyBillRegisterId);

        try
        {
            var partyBillRegister = await _dbContext.PartyBillRegisters
                .Include(pbr => pbr.PartyBillRegisterDetails)
                .FirstOrDefaultAsync(pbr => pbr.Id == request.PartyBillRegisterId, cancellationToken)
                .ConfigureAwait(false);

            if (partyBillRegister is null)
            {
                _logger.LogWarning("Party bill register with ID {PartyBillRegisterId} not found", request.PartyBillRegisterId);
                return new DeletePartyBillRegisterResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Party bill register not found."
                };
            }

            // Soft delete the party bill register
            partyBillRegister.IsDeleted = true;
            partyBillRegister.DeletedOn = DateTime.UtcNow;

            // Soft delete the details
            foreach (var detail in partyBillRegister.PartyBillRegisterDetails)
            {
                detail.IsDeleted = true;
                detail.DeletedOn = DateTime.UtcNow;
            }

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Successfully soft deleted party bill register with ID: {PartyBillRegisterId}", request.PartyBillRegisterId);

            return new DeletePartyBillRegisterResult
            {
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting party bill register with ID: {PartyBillRegisterId}", request.PartyBillRegisterId);
            return new DeletePartyBillRegisterResult
            {
                IsSuccess = false,
                ErrorMessage = "An error occurred while deleting the party bill register."
            };
        }
    }

    /// <summary>
    /// Generates a unique bill number.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A unique bill number.</returns>
    private async Task<string> GenerateBillNumberAsync(CancellationToken cancellationToken = default)
    {
        var prefix = "PBR";
        var year = DateTime.UtcNow.Year;
        var month = DateTime.UtcNow.Month.ToString("D2");

        // Get the last bill number for this month
        var lastBillNumber = await _dbContext.PartyBillRegisters
            .AsNoTracking()
            .Where(pbr => pbr.BillNumber.StartsWith($"{prefix}/{year}/{month}"))
            .OrderByDescending(pbr => pbr.BillNumber)
            .Select(pbr => pbr.BillNumber)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        int sequence = 1;

        if (!string.IsNullOrEmpty(lastBillNumber))
        {
            var parts = lastBillNumber.Split('/');
            if (parts.Length == 4 && int.TryParse(parts[3], out var lastSequence))
            {
                sequence = lastSequence + 1;
            }
        }

        return $"{prefix}/{year}/{month}/{sequence:D5}";
    }
}
