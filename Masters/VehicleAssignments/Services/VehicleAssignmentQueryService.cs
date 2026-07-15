using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using VehicleAssignmentEntity = VeteranLogistics.Data.Entities.Administration.VehicleAssignment;
using veteran_logistic.Masters.VehicleAssignments.Contracts;
using veteran_logistic.Masters.VehicleAssignments.Models;

namespace veteran_logistic.Masters.VehicleAssignments.Services;

/// <summary>
/// Implementation of the vehicle assignment query service.
/// </summary>
public sealed class VehicleAssignmentQueryService : IVehicleAssignmentQueryService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<VehicleAssignmentQueryService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="VehicleAssignmentQueryService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    public VehicleAssignmentQueryService(VeteranLogisticsDbContext dbContext, ILogger<VehicleAssignmentQueryService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<VehicleAssignmentListItem>> GetAllAssignmentsAsync(CancellationToken cancellationToken = default)
    {
        return await ProjectToListItem(_dbContext.VehicleAssignments.AsNoTracking())
            .OrderBy(va => va.AssignDate)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<VehicleAssignmentListItem>> SearchAssignmentsAsync(string? search, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.VehicleAssignments.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchPattern = $"%{search}%";
            query = query.Where(va =>
                EF.Functions.Like(va.Vehicle.VehicleNumber, searchPattern) ||
                EF.Functions.Like(va.VehicleOwner.FirstName, searchPattern) ||
                EF.Functions.Like(va.VehicleOwner.LastName, searchPattern) ||
                EF.Functions.Like(va.VehicleOwner.PANNumber, searchPattern));
        }

        return await ProjectToListItem(query)
            .OrderBy(va => va.AssignDate)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<VehicleAssignmentModel?> GetAssignmentForEditAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.VehicleAssignments
            .AsNoTracking()
            .Where(va => va.Id == id)
            .Select(va => new VehicleAssignmentModel
            {
                Id = va.Id,
                VehicleId = va.VehicleId,
                VehicleNumber = va.Vehicle.VehicleNumber,
                VehicleType = va.Vehicle.VehicleType,
                VehicleOwnerId = va.VehicleOwnerId,
                PANType = va.VehicleOwner.PANType,
                PANNumber = va.VehicleOwner.PANNumber,
                FirstName = va.VehicleOwner.FirstName,
                MiddleName = va.VehicleOwner.MiddleName,
                LastName = va.VehicleOwner.LastName,
                Address = va.VehicleOwner.Address,
                MobileNumber = va.VehicleOwner.Mobile,
                AssignDate = va.AssignDate,
                ReleaseDate = va.ReleaseDate,
                IsActive = va.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    private static IQueryable<VehicleAssignmentListItem> ProjectToListItem(IQueryable<VehicleAssignmentEntity> query)
    {
        return query.Select(va => new VehicleAssignmentListItem
        {
            Id = va.Id,
            VehicleNumber = va.Vehicle.VehicleNumber,
            VehicleType = va.Vehicle.VehicleType,
            OwnerName = $"{va.VehicleOwner.FirstName} {va.VehicleOwner.MiddleName} {va.VehicleOwner.LastName}".Trim(),
            PANNumber = va.VehicleOwner.PANNumber,
            AssignDate = va.AssignDate,
            ReleaseDate = va.ReleaseDate,
            Status = va.ReleaseDate == null ? "Active" : "Released"
        });
    }
}
