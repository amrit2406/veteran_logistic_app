using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using FinancialYearEntity = VeteranLogistics.Data.Entities.Administration.FinancialYear;
using veteran_logistic.Administration.FinancialYears.Contracts;
using veteran_logistic.Administration.FinancialYears.Models;

namespace veteran_logistic.Administration.FinancialYears.Services;

/// <summary>
/// Implementation of the financial year command service.
/// </summary>
public sealed class FinancialYearCommandService : IFinancialYearCommandService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ICreateFinancialYearValidator _createValidator;
    private readonly IUpdateFinancialYearValidator _updateValidator;
    private readonly ISetCurrentFinancialYearValidator _setCurrentValidator;
    private readonly ICloseFinancialYearValidator _closeValidator;
    private readonly ILogger<FinancialYearCommandService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="FinancialYearCommandService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="createValidator">The financial year creation validator.</param>
    /// <param name="updateValidator">The financial year update validator.</param>
    /// <param name="setCurrentValidator">The set current financial year validator.</param>
    /// <param name="closeValidator">The close financial year validator.</param>
    /// <param name="logger">The logger.</param>
    public FinancialYearCommandService(
        VeteranLogisticsDbContext dbContext,
        ICreateFinancialYearValidator createValidator,
        IUpdateFinancialYearValidator updateValidator,
        ISetCurrentFinancialYearValidator setCurrentValidator,
        ICloseFinancialYearValidator closeValidator,
        ILogger<FinancialYearCommandService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
        _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
        _setCurrentValidator = setCurrentValidator ?? throw new ArgumentNullException(nameof(setCurrentValidator));
        _closeValidator = closeValidator ?? throw new ArgumentNullException(nameof(closeValidator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<CreateFinancialYearResult> CreateFinancialYearAsync(CreateFinancialYearRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _createValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return CreateFinancialYearResult.Failure(errorMessage);
            }

            var trimmedName = request.Name.Trim();

            var existingFinancialYear = await _dbContext.FinancialYears
                .AsNoTracking()
                .FirstOrDefaultAsync(fy => fy.Name == trimmedName, cancellationToken)
                .ConfigureAwait(false);

            if (existingFinancialYear is not null)
            {
                return CreateFinancialYearResult.Failure("A financial year with this name already exists.");
            }

            var financialYear = new FinancialYearEntity
            {
                Name = trimmedName,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsCurrent = false,
                IsClosed = false,
                IsDeleted = false,
                CreatedOn = DateTime.UtcNow
            };

            _dbContext.FinancialYears.Add(financialYear);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return CreateFinancialYearResult.Success(financialYear.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating financial year '{Name}'", request.Name);
            return CreateFinancialYearResult.Failure("An unexpected error occurred while creating the financial year.");
        }
    }

    /// <inheritdoc />
    public async Task<UpdateFinancialYearResult> UpdateFinancialYearAsync(UpdateFinancialYearRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _updateValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdateFinancialYearResult.Failure(errorMessage);
            }

            var financialYear = await _dbContext.FinancialYears
                .FirstOrDefaultAsync(fy => fy.Id == request.Id, cancellationToken)
                .ConfigureAwait(false);

            if (financialYear is null)
            {
                return UpdateFinancialYearResult.Failure("Financial year not found.");
            }

            var trimmedName = request.Name.Trim();

            var existingFinancialYear = await _dbContext.FinancialYears
                .AsNoTracking()
                .FirstOrDefaultAsync(fy => fy.Name == trimmedName && fy.Id != request.Id, cancellationToken)
                .ConfigureAwait(false);

            if (existingFinancialYear is not null)
            {
                return UpdateFinancialYearResult.Failure("A financial year with this name already exists.");
            }

            financialYear.Name = trimmedName;
            financialYear.StartDate = request.StartDate;
            financialYear.EndDate = request.EndDate;
            financialYear.IsCurrent = request.IsCurrent;
            financialYear.IsClosed = request.IsClosed;

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return UpdateFinancialYearResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating financial year '{Id}'", request.Id);
            return UpdateFinancialYearResult.Failure("An unexpected error occurred while updating the financial year.");
        }
    }

    /// <inheritdoc />
    public async Task<SetCurrentFinancialYearResult> SetCurrentFinancialYearAsync(SetCurrentFinancialYearRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _setCurrentValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return SetCurrentFinancialYearResult.Failure(errorMessage);
            }

            var financialYear = await _dbContext.FinancialYears
                .FirstOrDefaultAsync(fy => fy.Id == request.FinancialYearId, cancellationToken)
                .ConfigureAwait(false);

            if (financialYear is null)
            {
                return SetCurrentFinancialYearResult.Failure("Financial year not found.");
            }

            if (financialYear.IsDeleted)
            {
                return SetCurrentFinancialYearResult.Failure("Cannot set a deleted financial year as current.");
            }

            // If already current, return success without updating
            if (financialYear.IsCurrent)
            {
                return SetCurrentFinancialYearResult.Success();
            }

            // Clear the current flag from the existing current financial year (if any)
            var currentFinancialYear = await _dbContext.FinancialYears
                .FirstOrDefaultAsync(fy => fy.IsCurrent && !fy.IsDeleted, cancellationToken)
                .ConfigureAwait(false);

            if (currentFinancialYear is not null)
            {
                currentFinancialYear.IsCurrent = false;
            }

            // Set the requested financial year as current
            financialYear.IsCurrent = true;

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return SetCurrentFinancialYearResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while setting financial year '{FinancialYearId}' as current", request.FinancialYearId);
            return SetCurrentFinancialYearResult.Failure("An unexpected error occurred while setting the financial year as current.");
        }
    }

    /// <inheritdoc />
    public async Task<CloseFinancialYearResult> CloseFinancialYearAsync(CloseFinancialYearRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _closeValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return CloseFinancialYearResult.Failure(errorMessage);
            }

            var financialYear = await _dbContext.FinancialYears
                .FirstOrDefaultAsync(fy => fy.Id == request.FinancialYearId, cancellationToken)
                .ConfigureAwait(false);

            if (financialYear is null)
            {
                return CloseFinancialYearResult.Failure("Financial year not found.");
            }

            // If already closed, return success without updating
            if (financialYear.IsClosed)
            {
                return CloseFinancialYearResult.Success();
            }

            // Cannot close the current financial year
            if (financialYear.IsCurrent)
            {
                return CloseFinancialYearResult.Failure("Current Financial Year cannot be closed.");
            }

            // Set the financial year as closed
            financialYear.IsClosed = true;

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return CloseFinancialYearResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while closing financial year '{FinancialYearId}'", request.FinancialYearId);
            return CloseFinancialYearResult.Failure("An unexpected error occurred while closing the financial year.");
        }
    }
}
