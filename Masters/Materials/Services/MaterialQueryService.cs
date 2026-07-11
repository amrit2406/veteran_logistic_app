using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using MaterialEntity = VeteranLogistics.Data.Entities.Administration.Material;
using veteran_logistic.Masters.Materials.Contracts;
using veteran_logistic.Masters.Materials.Models;

namespace veteran_logistic.Masters.Materials.Services;

/// <summary>
/// Implementation of the material query service.
/// </summary>
public sealed class MaterialQueryService : IMaterialQueryService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<MaterialQueryService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="MaterialQueryService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    public MaterialQueryService(VeteranLogisticsDbContext dbContext, ILogger<MaterialQueryService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<MaterialListItem>> GetAllMaterialsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var materials = await ProjectToListItem(_dbContext.Materials.AsNoTracking())
                .OrderBy(m => m.MaterialName)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            _logger.LogInformation("Retrieved {Count} materials", materials.Count);
            return materials;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all materials");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<MaterialListItem>> SearchMaterialsAsync(string? searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _dbContext.Materials.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchPattern = $"%{searchTerm}%";
                query = query.Where(m => EF.Functions.Like(m.MaterialName, searchPattern));
            }

            var materials = await ProjectToListItem(query)
                .OrderBy(m => m.MaterialName)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            _logger.LogInformation("Retrieved {Count} materials matching search term '{SearchTerm}'", materials.Count, searchTerm);
            return materials;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while searching materials with term '{SearchTerm}'", searchTerm);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<MaterialModel?> GetMaterialForEditAsync(int materialId, CancellationToken cancellationToken = default)
    {
        try
        {
            var material = await _dbContext.Materials
                .AsNoTracking()
                .Where(m => m.Id == materialId)
                .Select(m => new MaterialModel
                {
                    Id = m.Id,
                    MaterialName = m.MaterialName,
                    IsActive = m.IsActive
                })
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);

            if (material is null)
            {
                _logger.LogWarning("Material with ID {MaterialId} not found", materialId);
                return null;
            }

            _logger.LogInformation("Retrieved material with ID {MaterialId} for editing", materialId);
            return material;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving material with ID {MaterialId} for editing", materialId);
            throw;
        }
    }

    private static IQueryable<MaterialListItem> ProjectToListItem(IQueryable<MaterialEntity> query)
    {
        return query.Select(m => new MaterialListItem
        {
            Id = m.Id,
            MaterialName = m.MaterialName,
            IsActive = m.IsActive
        });
    }
}
