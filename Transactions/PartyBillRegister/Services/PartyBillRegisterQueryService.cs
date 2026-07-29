using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using VeteranLogistics.Data.Entities.Administration;
using veteran_logistic.Transactions.PartyBillRegister.Contracts;
using veteran_logistic.Transactions.PartyBillRegister.Models;

namespace veteran_logistic.Transactions.PartyBillRegister.Services;

/// <summary>
/// Service for querying party bill register data.
/// </summary>
public sealed class PartyBillRegisterQueryService : IPartyBillRegisterQueryService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<PartyBillRegisterQueryService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PartyBillRegisterQueryService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    public PartyBillRegisterQueryService(VeteranLogisticsDbContext dbContext, ILogger<PartyBillRegisterQueryService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<PartyBillRegisterListItem>> GetAllPartyBillRegistersAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving all party bill registers");

        var query = _dbContext.PartyBillRegisters
            .AsNoTracking()
            .Include(pbr => pbr.Party)
            .OrderByDescending(pbr => pbr.BillDate)
            .ThenByDescending(pbr => pbr.CreatedOn);

        var partyBillRegisters = await query
            .Select(pbr => new PartyBillRegisterListItem
            {
                Id = pbr.Id,
                BillNumber = pbr.BillNumber,
                BillDate = pbr.BillDate,
                PartyName = pbr.Party != null ? pbr.Party.CustomerName : string.Empty,
                ThirdPartyName = pbr.ThirdPartyName,
                PermitNumber = pbr.PermitNumber,
                GrandTotal = pbr.GrandTotal,
                IsActive = pbr.IsActive
            })
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        _logger.LogInformation("Retrieved {Count} party bill registers", partyBillRegisters.Count);
        return partyBillRegisters.AsReadOnly();
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<PartyBillRegisterListItem>> SearchPartyBillRegistersAsync(string? search, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Searching party bill registers with search term: {SearchTerm}", search ?? "all");

        var query = _dbContext.PartyBillRegisters
            .AsNoTracking()
            .Include(pbr => pbr.Party)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTerm = search.Trim();
            query = query.Where(pbr =>
                pbr.BillNumber.Contains(searchTerm) ||
                (pbr.Party != null && pbr.Party.CustomerName.Contains(searchTerm)) ||
                pbr.ThirdPartyName.Contains(searchTerm) ||
                (pbr.PermitNumber != null && pbr.PermitNumber.Contains(searchTerm)));
        }

        query = query.OrderByDescending(pbr => pbr.BillDate)
            .ThenByDescending(pbr => pbr.CreatedOn);

        var partyBillRegisters = await query
            .Select(pbr => new PartyBillRegisterListItem
            {
                Id = pbr.Id,
                BillNumber = pbr.BillNumber,
                BillDate = pbr.BillDate,
                PartyName = pbr.Party != null ? pbr.Party.CustomerName : string.Empty,
                ThirdPartyName = pbr.ThirdPartyName,
                PermitNumber = pbr.PermitNumber,
                GrandTotal = pbr.GrandTotal,
                IsActive = pbr.IsActive
            })
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        _logger.LogInformation("Found {Count} party bill registers matching search criteria", partyBillRegisters.Count);
        return partyBillRegisters.AsReadOnly();
    }

    /// <inheritdoc />
    public async Task<PartyBillRegisterModel?> GetPartyBillRegisterForEditAsync(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving party bill register for edit with ID: {PartyBillRegisterId}", id);

        var partyBillRegister = await _dbContext.PartyBillRegisters
            .AsNoTracking()
            .Include(pbr => pbr.Party)
            .Include(pbr => pbr.Consignor)
            .Include(pbr => pbr.Destination)
            .Include(pbr => pbr.PartyBillRegisterDetails)
                .ThenInclude(pbrd => pbrd.LoadingRegister)
            .FirstOrDefaultAsync(pbr => pbr.Id == id, cancellationToken)
            .ConfigureAwait(false);

        if (partyBillRegister is null)
        {
            _logger.LogWarning("Party bill register with ID {PartyBillRegisterId} not found", id);
            return null;
        }

        var model = new PartyBillRegisterModel
        {
            Id = partyBillRegister.Id,
            BillNumber = partyBillRegister.BillNumber,
            BillDate = partyBillRegister.BillDate,
            PartyId = partyBillRegister.PartyId,
            PartyName = partyBillRegister.Party != null ? partyBillRegister.Party.CustomerName : string.Empty,
            ThirdPartyName = partyBillRegister.ThirdPartyName,
            PermitNumber = partyBillRegister.PermitNumber,
            ConsignorId = partyBillRegister.ConsignorId,
            ConsignorName = partyBillRegister.Consignor != null ? partyBillRegister.Consignor.CustomerName : null,
            DestinationId = partyBillRegister.DestinationId,
            DestinationName = partyBillRegister.Destination != null ? partyBillRegister.Destination.LocationName : null,
            FromDate = partyBillRegister.FromDate,
            ToDate = partyBillRegister.ToDate,
            TotalRecords = partyBillRegister.TotalRecords,
            TotalMaterialWeight = partyBillRegister.TotalMaterialWeight,
            TotalAmount = partyBillRegister.TotalAmount,
            ChargeHead1 = partyBillRegister.ChargeHead1,
            ChargeType1 = partyBillRegister.ChargeType1,
            ChargeAmount1 = partyBillRegister.ChargeAmount1,
            ChargeHead2 = partyBillRegister.ChargeHead2,
            ChargeType2 = partyBillRegister.ChargeType2,
            ChargeAmount2 = partyBillRegister.ChargeAmount2,
            GrandTotal = partyBillRegister.GrandTotal,
            Remarks = partyBillRegister.Remarks,
            IsActive = partyBillRegister.IsActive,
            PartyBillRegisterDetails = partyBillRegister.PartyBillRegisterDetails.Select(pbrd => new PartyBillRegisterDetailModel
            {
                Id = pbrd.Id,
                PartyBillRegisterId = pbrd.PartyBillRegisterId,
                LoadingRegisterId = pbrd.LoadingRegisterId,
                TPNumber = pbrd.TPNumber,
                ChallanNumber = pbrd.ChallanNumber,
                VehicleNumber = pbrd.VehicleNumber,
                LoadingDate = pbrd.LoadingDate,
                MaterialWeight = pbrd.MaterialWeight,
                BillingRate = pbrd.BillingRate,
                DriverCommission = pbrd.DriverCommission,
                Amount = pbrd.Amount
            }).ToList()
        };

        _logger.LogInformation("Retrieved party bill register for edit with ID: {PartyBillRegisterId}", id);
        return model;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<EligibleLoadingRegisterModel>> GetEligibleLoadingRegistersAsync(int? consignorId, int? destinationId, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving eligible loading registers for bill generation with filters - ConsignorId: {ConsignorId}, DestinationId: {DestinationId}, FromDate: {FromDate}, ToDate: {ToDate}",
            consignorId, destinationId, fromDate, toDate);

        var query = _dbContext.LoadingRegisters
            .AsNoTracking()
            .Include(lr => lr.Vehicle)
            .Where(lr => lr.IsActive)
            .AsQueryable();

        if (consignorId.HasValue && consignorId.Value > 0)
        {
            query = query.Where(lr => lr.ConsignorId == consignorId.Value);
        }

        if (destinationId.HasValue && destinationId.Value > 0)
        {
            query = query.Where(lr => lr.DestinationId == destinationId.Value);
        }

        if (fromDate.HasValue)
        {
            query = query.Where(lr => lr.LoadingDate >= fromDate.Value);
        }

        if (toDate.HasValue)
        {
            query = query.Where(lr => lr.LoadingDate <= toDate.Value);
        }

        // Exclude loading registers that are already in active party bills
        var existingLoadingRegisterIds = await _dbContext.PartyBillRegisterDetails
            .AsNoTracking()
            .Include(pbrd => pbrd.PartyBillRegister)
            .Where(pbrd => pbrd.PartyBillRegister.IsActive)
            .Select(pbrd => pbrd.LoadingRegisterId)
            .Distinct()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (existingLoadingRegisterIds.Any())
        {
            query = query.Where(lr => !existingLoadingRegisterIds.Contains(lr.Id));
        }

        query = query.OrderBy(lr => lr.LoadingDate)
            .ThenBy(lr => lr.ChallanNumber);

        var eligibleLoadingRegisters = await query
            .Select(lr => new EligibleLoadingRegisterModel
            {
                Id = lr.Id,
                TPNumber = lr.TPNumber,
                ChallanNumber = lr.ChallanNumber,
                VehicleNumber = lr.Vehicle != null ? lr.Vehicle.VehicleNumber : null,
                LoadingDate = lr.LoadingDate,
                MaterialWeight = lr.LoadingWeight,
                BillingRate = lr.Rate,
                DriverCommission = lr.DriverCommission,
                Amount = lr.GrossAmount,
                IsSelected = false
            })
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        _logger.LogInformation("Retrieved {Count} eligible loading registers for bill generation", eligibleLoadingRegisters.Count);
        return eligibleLoadingRegisters.AsReadOnly();
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<PartyBillRegisterDetailModel>> GetPartyBillRegisterDetailsAsync(int partyBillRegisterId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving party bill register details for party bill register ID: {PartyBillRegisterId}", partyBillRegisterId);

        var details = await _dbContext.PartyBillRegisterDetails
            .AsNoTracking()
            .Include(pbrd => pbrd.LoadingRegister)
            .Where(pbrd => pbrd.PartyBillRegisterId == partyBillRegisterId)
            .OrderBy(pbrd => pbrd.LoadingDate)
            .ThenBy(pbrd => pbrd.ChallanNumber)
            .Select(pbrd => new PartyBillRegisterDetailModel
            {
                Id = pbrd.Id,
                PartyBillRegisterId = pbrd.PartyBillRegisterId,
                LoadingRegisterId = pbrd.LoadingRegisterId,
                TPNumber = pbrd.TPNumber,
                ChallanNumber = pbrd.ChallanNumber,
                VehicleNumber = pbrd.VehicleNumber,
                LoadingDate = pbrd.LoadingDate,
                MaterialWeight = pbrd.MaterialWeight,
                BillingRate = pbrd.BillingRate,
                DriverCommission = pbrd.DriverCommission,
                Amount = pbrd.Amount
            })
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        _logger.LogInformation("Retrieved {Count} party bill register details", details.Count);
        return details.AsReadOnly();
    }
}
