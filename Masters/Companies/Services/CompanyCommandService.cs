using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using CompanyEntity = VeteranLogistics.Data.Entities.Administration.Company;
using veteran_logistic.Masters.Companies.Contracts;
using veteran_logistic.Masters.Companies.Models;

namespace veteran_logistic.Masters.Companies.Services;

/// <summary>
/// Implementation of the company command service.
/// </summary>
public sealed class CompanyCommandService : ICompanyCommandService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ICreateCompanyValidator _createValidator;
    private readonly IUpdateCompanyValidator _updateValidator;
    private readonly IUpdateCompanyStatusValidator _updateStatusValidator;
    private readonly IDeleteCompanyValidator _deleteValidator;
    private readonly ILogger<CompanyCommandService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompanyCommandService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="createValidator">The company creation validator.</param>
    /// <param name="updateValidator">The company update validator.</param>
    /// <param name="updateStatusValidator">The company status update validator.</param>
    /// <param name="deleteValidator">The delete company validator.</param>
    /// <param name="logger">The logger.</param>
    public CompanyCommandService(
        VeteranLogisticsDbContext dbContext,
        ICreateCompanyValidator createValidator,
        IUpdateCompanyValidator updateValidator,
        IUpdateCompanyStatusValidator updateStatusValidator,
        IDeleteCompanyValidator deleteValidator,
        ILogger<CompanyCommandService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
        _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
        _updateStatusValidator = updateStatusValidator ?? throw new ArgumentNullException(nameof(updateStatusValidator));
        _deleteValidator = deleteValidator ?? throw new ArgumentNullException(nameof(deleteValidator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<CreateCompanyResult> CreateCompanyAsync(CreateCompanyRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _createValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return CreateCompanyResult.Failure(errorMessage);
            }

            // Check for duplicate CompanyCode
            var existingCompanyByCode = await _dbContext.Companies
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CompanyCode == request.CompanyCode, cancellationToken)
                .ConfigureAwait(false);

            if (existingCompanyByCode is not null)
            {
                return CreateCompanyResult.Failure("A company with this company code already exists.");
            }

            // Check for duplicate GSTNumber
            var existingCompanyByGST = await _dbContext.Companies
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.GSTNumber == request.GSTNumber, cancellationToken)
                .ConfigureAwait(false);

            if (existingCompanyByGST is not null)
            {
                return CreateCompanyResult.Failure("A company with this GST number already exists.");
            }

            var company = new CompanyEntity
            {
                CompanyCode = request.CompanyCode,
                CompanyName = request.CompanyName,
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                City = request.City,
                State = request.State,
                Country = request.Country,
                PostalCode = request.PostalCode,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                GSTNumber = request.GSTNumber,
                PANNumber = request.PANNumber,
                IsActive = request.IsActive,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System", // TODO: Replace with actual user from session
                ModifiedBy = "System"
            };

            _dbContext.Companies.Add(company);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Company '{CompanyCode}' created successfully with ID {CompanyId}", request.CompanyCode, company.Id);
            return CreateCompanyResult.Success(company.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating company '{CompanyCode}'", request.CompanyCode);
            return CreateCompanyResult.Failure("An unexpected error occurred while creating the company.");
        }
    }

    /// <inheritdoc />
    public async Task<UpdateCompanyResult> UpdateCompanyAsync(UpdateCompanyRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _updateValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdateCompanyResult.Failure(errorMessage);
            }

            var company = await _dbContext.Companies
                .FirstOrDefaultAsync(c => c.Id == request.CompanyId, cancellationToken)
                .ConfigureAwait(false);

            if (company is null)
            {
                return UpdateCompanyResult.Failure("Company not found.");
            }

            // Check for duplicate CompanyCode (excluding current company)
            var existingCompanyByCode = await _dbContext.Companies
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CompanyCode == request.CompanyCode && c.Id != request.CompanyId, cancellationToken)
                .ConfigureAwait(false);

            if (existingCompanyByCode is not null)
            {
                return UpdateCompanyResult.Failure("A company with this company code already exists.");
            }

            // Check for duplicate GSTNumber (excluding current company)
            var existingCompanyByGST = await _dbContext.Companies
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.GSTNumber == request.GSTNumber && c.Id != request.CompanyId, cancellationToken)
                .ConfigureAwait(false);

            if (existingCompanyByGST is not null)
            {
                return UpdateCompanyResult.Failure("A company with this GST number already exists.");
            }

            company.CompanyCode = request.CompanyCode;
            company.CompanyName = request.CompanyName;
            company.AddressLine1 = request.AddressLine1;
            company.AddressLine2 = request.AddressLine2;
            company.City = request.City;
            company.State = request.State;
            company.Country = request.Country;
            company.PostalCode = request.PostalCode;
            company.PhoneNumber = request.PhoneNumber;
            company.Email = request.Email;
            company.GSTNumber = request.GSTNumber;
            company.PANNumber = request.PANNumber;
            company.IsActive = request.IsActive;
            company.ModifiedOn = DateTime.UtcNow;
            company.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Company '{CompanyId}' updated successfully", request.CompanyId);
            return UpdateCompanyResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating company '{CompanyId}'", request.CompanyId);
            return UpdateCompanyResult.Failure("An unexpected error occurred while updating the company.");
        }
    }

    /// <inheritdoc />
    public async Task<UpdateCompanyStatusResult> UpdateCompanyStatusAsync(UpdateCompanyStatusRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var company = await _dbContext.Companies
                .FirstOrDefaultAsync(c => c.Id == request.CompanyId, cancellationToken)
                .ConfigureAwait(false);

            if (company is null)
            {
                return UpdateCompanyStatusResult.Failure("Company not found.");
            }

            var validationResult = _updateStatusValidator.Validate(request, company.IsActive);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdateCompanyStatusResult.Failure(errorMessage);
            }

            company.IsActive = request.IsActive;
            company.ModifiedOn = DateTime.UtcNow;
            company.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Company '{CompanyId}' status updated to {IsActive}", request.CompanyId, request.IsActive);
            return UpdateCompanyStatusResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating company status '{CompanyId}'", request.CompanyId);
            return UpdateCompanyStatusResult.Failure("An unexpected error occurred while updating the company status.");
        }
    }

    /// <inheritdoc />
    public async Task<DeleteCompanyResult> DeleteCompanyAsync(DeleteCompanyRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _deleteValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return DeleteCompanyResult.Failure(errorMessage);
            }

            var company = await _dbContext.Companies
                .FirstOrDefaultAsync(c => c.Id == request.CompanyId, cancellationToken)
                .ConfigureAwait(false);

            if (company is null)
            {
                return DeleteCompanyResult.Failure("Company not found.");
            }

            company.IsDeleted = true;
            company.DeletedOn = DateTime.UtcNow;
            company.ModifiedOn = DateTime.UtcNow;
            company.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Company '{CompanyId}' deleted successfully", request.CompanyId);
            return DeleteCompanyResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting company '{CompanyId}'", request.CompanyId);
            return DeleteCompanyResult.Failure("An unexpected error occurred while deleting the company.");
        }
    }
}
