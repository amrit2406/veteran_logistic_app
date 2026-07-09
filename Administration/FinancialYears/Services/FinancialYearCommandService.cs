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
    private readonly ILogger<FinancialYearCommandService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="FinancialYearCommandService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="createValidator">The financial year creation validator.</param>
    /// <param name="updateValidator">The financial year update validator.</param>
    /// <param name="logger">The logger.</param>
    public FinancialYearCommandService(
        VeteranLogisticsDbContext dbContext,
        ICreateFinancialYearValidator createValidator,
        IUpdateFinancialYearValidator updateValidator,
        ILogger<FinancialYearCommandService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
        _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
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
}
