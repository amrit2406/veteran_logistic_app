using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using MaterialEntity = VeteranLogistics.Data.Entities.Administration.Material;
using veteran_logistic.Masters.Materials.Contracts;
using veteran_logistic.Masters.Materials.Models;

namespace veteran_logistic.Masters.Materials.Services;

/// <summary>
/// Implementation of the material command service.
/// </summary>
public sealed class MaterialCommandService : IMaterialCommandService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ICreateMaterialValidator _createValidator;
    private readonly IUpdateMaterialValidator _updateValidator;
    private readonly IUpdateMaterialStatusValidator _updateStatusValidator;
    private readonly IDeleteMaterialValidator _deleteValidator;
    private readonly ILogger<MaterialCommandService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="MaterialCommandService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="createValidator">The material creation validator.</param>
    /// <param name="updateValidator">The material update validator.</param>
    /// <param name="updateStatusValidator">The material status update validator.</param>
    /// <param name="deleteValidator">The delete material validator.</param>
    /// <param name="logger">The logger.</param>
    public MaterialCommandService(
        VeteranLogisticsDbContext dbContext,
        ICreateMaterialValidator createValidator,
        IUpdateMaterialValidator updateValidator,
        IUpdateMaterialStatusValidator updateStatusValidator,
        IDeleteMaterialValidator deleteValidator,
        ILogger<MaterialCommandService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
        _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
        _updateStatusValidator = updateStatusValidator ?? throw new ArgumentNullException(nameof(updateStatusValidator));
        _deleteValidator = deleteValidator ?? throw new ArgumentNullException(nameof(deleteValidator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<CreateMaterialResult> CreateMaterialAsync(CreateMaterialRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _createValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return CreateMaterialResult.Failure(errorMessage);
            }

            // Check for duplicate MaterialName
            var existingMaterialByName = await _dbContext.Materials
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.MaterialName == request.MaterialName, cancellationToken)
                .ConfigureAwait(false);

            if (existingMaterialByName is not null)
            {
                return CreateMaterialResult.Failure("A material with this name already exists.");
            }

            var material = new MaterialEntity
            {
                MaterialName = request.MaterialName,
                IsActive = request.IsActive,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System", // TODO: Replace with actual user from session
                ModifiedBy = "System"
            };

            _dbContext.Materials.Add(material);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Material '{MaterialName}' created successfully with ID {MaterialId}", request.MaterialName, material.Id);
            return CreateMaterialResult.Success(material.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating material '{MaterialName}'", request.MaterialName);
            return CreateMaterialResult.Failure("An unexpected error occurred while creating the material.");
        }
    }

    /// <inheritdoc />
    public async Task<UpdateMaterialResult> UpdateMaterialAsync(UpdateMaterialRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _updateValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdateMaterialResult.Failure(errorMessage);
            }

            var material = await _dbContext.Materials
                .FirstOrDefaultAsync(m => m.Id == request.MaterialId, cancellationToken)
                .ConfigureAwait(false);

            if (material is null)
            {
                return UpdateMaterialResult.Failure("Material not found.");
            }

            // Check for duplicate MaterialName (excluding current material)
            var existingMaterialByName = await _dbContext.Materials
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.MaterialName == request.MaterialName && m.Id != request.MaterialId, cancellationToken)
                .ConfigureAwait(false);

            if (existingMaterialByName is not null)
            {
                return UpdateMaterialResult.Failure("A material with this name already exists.");
            }

            material.MaterialName = request.MaterialName;
            material.IsActive = request.IsActive;
            material.ModifiedOn = DateTime.UtcNow;
            material.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Material '{MaterialId}' updated successfully", request.MaterialId);
            return UpdateMaterialResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating material '{MaterialId}'", request.MaterialId);
            return UpdateMaterialResult.Failure("An unexpected error occurred while updating the material.");
        }
    }

    /// <inheritdoc />
    public async Task<UpdateMaterialStatusResult> UpdateMaterialStatusAsync(UpdateMaterialStatusRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var material = await _dbContext.Materials
                .FirstOrDefaultAsync(m => m.Id == request.MaterialId, cancellationToken)
                .ConfigureAwait(false);

            if (material is null)
            {
                return UpdateMaterialStatusResult.Failure("Material not found.");
            }

            var validationResult = _updateStatusValidator.Validate(request, material.IsActive);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdateMaterialStatusResult.Failure(errorMessage);
            }

            material.IsActive = request.IsActive;
            material.ModifiedOn = DateTime.UtcNow;
            material.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Material '{MaterialId}' status updated to {IsActive}", request.MaterialId, request.IsActive);
            return UpdateMaterialStatusResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating material status '{MaterialId}'", request.MaterialId);
            return UpdateMaterialStatusResult.Failure("An unexpected error occurred while updating the material status.");
        }
    }

    /// <inheritdoc />
    public async Task<DeleteMaterialResult> DeleteMaterialAsync(DeleteMaterialRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _deleteValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return DeleteMaterialResult.Failure(errorMessage);
            }

            var material = await _dbContext.Materials
                .FirstOrDefaultAsync(m => m.Id == request.MaterialId, cancellationToken)
                .ConfigureAwait(false);

            if (material is null)
            {
                return DeleteMaterialResult.Failure("Material not found.");
            }

            material.IsDeleted = true;
            material.DeletedOn = DateTime.UtcNow;
            material.ModifiedOn = DateTime.UtcNow;
            material.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Material '{MaterialId}' deleted successfully", request.MaterialId);
            return DeleteMaterialResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting material '{MaterialId}'", request.MaterialId);
            return DeleteMaterialResult.Failure("An unexpected error occurred while deleting the material.");
        }
    }
}
