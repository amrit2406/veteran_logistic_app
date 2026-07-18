using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using VehicleEntity = VeteranLogistics.Data.Entities.Administration.Vehicle;
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
    public async Task<VehicleAssignmentModel?> GetVehicleAssignmentAsync(string vehicleNumber, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Getting vehicle assignment for vehicle number: {VehicleNumber}", vehicleNumber);

        var result = await _dbContext.Vehicles
            .AsNoTracking()
            .Include(v => v.VehicleOwner)
            .Where(v => v.VehicleNumber == vehicleNumber && !v.IsDeleted)
            .Select(v => new VehicleAssignmentModel
            {
                VehicleNumber = v.VehicleNumber,
                VehicleType = v.VehicleType,
                AssignDate = v.CreatedOn,
                ReleaseDate = v.DeletedOn,
                PANType = v.VehicleOwner != null ? v.VehicleOwner.PANType : string.Empty,
                PANNumber = v.VehicleOwner != null ? v.VehicleOwner.PANNumber : string.Empty,
                FirstName = v.VehicleOwner != null ? v.VehicleOwner.FirstName : string.Empty,
                MiddleName = v.VehicleOwner != null ? v.VehicleOwner.MiddleName : string.Empty,
                LastName = v.VehicleOwner != null ? v.VehicleOwner.LastName : string.Empty,
                Address = v.VehicleOwner != null ? v.VehicleOwner.Address : string.Empty,
                MobileNumber = v.VehicleOwner != null ? v.VehicleOwner.Mobile : string.Empty
            })
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        if (result != null)
        {
            _logger.LogInformation("Vehicle assignment found for vehicle number: {VehicleNumber}", vehicleNumber);
        }
        else
        {
            _logger.LogWarning("Vehicle assignment not found for vehicle number: {VehicleNumber}", vehicleNumber);
        }

        return result;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<VehicleAssignmentModel>> SearchVehiclesAsync(string? search, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Searching vehicles with search term: {Search}", search ?? "all");

        var query = _dbContext.Vehicles
            .AsNoTracking()
            .Include(v => v.VehicleOwner)
            .Where(v => !v.IsDeleted);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchPattern = $"%{search}%";
            query = query.Where(v => EF.Functions.Like(v.VehicleNumber, searchPattern));
        }

        var results = await query
            .Select(v => new VehicleAssignmentModel
            {
                VehicleNumber = v.VehicleNumber,
                VehicleType = v.VehicleType,
                AssignDate = v.CreatedOn,
                ReleaseDate = v.DeletedOn,
                PANType = v.VehicleOwner != null ? v.VehicleOwner.PANType : string.Empty,
                PANNumber = v.VehicleOwner != null ? v.VehicleOwner.PANNumber : string.Empty,
                FirstName = v.VehicleOwner != null ? v.VehicleOwner.FirstName : string.Empty,
                MiddleName = v.VehicleOwner != null ? v.VehicleOwner.MiddleName : string.Empty,
                LastName = v.VehicleOwner != null ? v.VehicleOwner.LastName : string.Empty,
                Address = v.VehicleOwner != null ? v.VehicleOwner.Address : string.Empty,
                MobileNumber = v.VehicleOwner != null ? v.VehicleOwner.Mobile : string.Empty
            })
            .OrderBy(v => v.VehicleNumber)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        _logger.LogInformation("Found {Count} vehicles matching search criteria", results.Count);

        return results;
    }
}
