using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using VendorEntity = VeteranLogistics.Data.Entities.Administration.Vendor;
using veteran_logistic.Masters.Vendors.Contracts;
using veteran_logistic.Masters.Vendors.Models;

namespace veteran_logistic.Masters.Vendors.Services;

/// <summary>
/// Implementation of the vendor command service.
/// </summary>
public sealed class VendorCommandService : IVendorCommandService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ICreateVendorValidator _createValidator;
    private readonly IUpdateVendorValidator _updateValidator;
    private readonly IUpdateVendorStatusValidator _updateStatusValidator;
    private readonly IDeleteVendorValidator _deleteValidator;
    private readonly ILogger<VendorCommandService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="VendorCommandService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="createValidator">The vendor creation validator.</param>
    /// <param name="updateValidator">The vendor update validator.</param>
    /// <param name="updateStatusValidator">The vendor status update validator.</param>
    /// <param name="deleteValidator">The delete vendor validator.</param>
    /// <param name="logger">The logger.</param>
    public VendorCommandService(
        VeteranLogisticsDbContext dbContext,
        ICreateVendorValidator createValidator,
        IUpdateVendorValidator updateValidator,
        IUpdateVendorStatusValidator updateStatusValidator,
        IDeleteVendorValidator deleteValidator,
        ILogger<VendorCommandService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
        _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
        _updateStatusValidator = updateStatusValidator ?? throw new ArgumentNullException(nameof(updateStatusValidator));
        _deleteValidator = deleteValidator ?? throw new ArgumentNullException(nameof(deleteValidator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<CreateVendorResult> CreateVendorAsync(CreateVendorRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _createValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return CreateVendorResult.Failure(errorMessage);
            }

            // Check for duplicate VendorCode
            var existingVendorByCode = await _dbContext.Vendors
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.VendorCode == request.VendorCode, cancellationToken)
                .ConfigureAwait(false);

            if (existingVendorByCode is not null)
            {
                return CreateVendorResult.Failure("A vendor with this vendor code already exists.");
            }

            // Check for duplicate GSTNumber
            var existingVendorByGST = await _dbContext.Vendors
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.GSTNumber == request.GSTNumber, cancellationToken)
                .ConfigureAwait(false);

            if (existingVendorByGST is not null)
            {
                return CreateVendorResult.Failure("A vendor with this GST number already exists.");
            }

            var vendor = new VendorEntity
            {
                VendorCode = request.VendorCode,
                VendorName = request.VendorName,
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
                ContactPerson = request.ContactPerson,
                IsActive = request.IsActive,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System", // TODO: Replace with actual user from session
                ModifiedBy = "System"
            };

            _dbContext.Vendors.Add(vendor);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Vendor '{VendorCode}' created successfully with ID {VendorId}", request.VendorCode, vendor.Id);
            return CreateVendorResult.Success(vendor.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating vendor '{VendorCode}'", request.VendorCode);
            return CreateVendorResult.Failure("An unexpected error occurred while creating the vendor.");
        }
    }

    /// <inheritdoc />
    public async Task<UpdateVendorResult> UpdateVendorAsync(UpdateVendorRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _updateValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdateVendorResult.Failure(errorMessage);
            }

            var vendor = await _dbContext.Vendors
                .FirstOrDefaultAsync(v => v.Id == request.VendorId, cancellationToken)
                .ConfigureAwait(false);

            if (vendor is null)
            {
                return UpdateVendorResult.Failure("Vendor not found.");
            }

            // Check for duplicate VendorCode (excluding current vendor)
            var existingVendorByCode = await _dbContext.Vendors
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.VendorCode == request.VendorCode && v.Id != request.VendorId, cancellationToken)
                .ConfigureAwait(false);

            if (existingVendorByCode is not null)
            {
                return UpdateVendorResult.Failure("A vendor with this vendor code already exists.");
            }

            // Check for duplicate GSTNumber (excluding current vendor)
            var existingVendorByGST = await _dbContext.Vendors
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.GSTNumber == request.GSTNumber && v.Id != request.VendorId, cancellationToken)
                .ConfigureAwait(false);

            if (existingVendorByGST is not null)
            {
                return UpdateVendorResult.Failure("A vendor with this GST number already exists.");
            }

            vendor.VendorCode = request.VendorCode;
            vendor.VendorName = request.VendorName;
            vendor.AddressLine1 = request.AddressLine1;
            vendor.AddressLine2 = request.AddressLine2;
            vendor.City = request.City;
            vendor.State = request.State;
            vendor.Country = request.Country;
            vendor.PostalCode = request.PostalCode;
            vendor.PhoneNumber = request.PhoneNumber;
            vendor.Email = request.Email;
            vendor.GSTNumber = request.GSTNumber;
            vendor.PANNumber = request.PANNumber;
            vendor.ContactPerson = request.ContactPerson;
            vendor.IsActive = request.IsActive;
            vendor.ModifiedOn = DateTime.UtcNow;
            vendor.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Vendor '{VendorId}' updated successfully", request.VendorId);
            return UpdateVendorResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating vendor '{VendorId}'", request.VendorId);
            return UpdateVendorResult.Failure("An unexpected error occurred while updating the vendor.");
        }
    }

    /// <inheritdoc />
    public async Task<UpdateVendorStatusResult> UpdateVendorStatusAsync(UpdateVendorStatusRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var vendor = await _dbContext.Vendors
                .FirstOrDefaultAsync(v => v.Id == request.VendorId, cancellationToken)
                .ConfigureAwait(false);

            if (vendor is null)
            {
                return UpdateVendorStatusResult.Failure("Vendor not found.");
            }

            var validationResult = _updateStatusValidator.Validate(request, vendor.IsActive);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdateVendorStatusResult.Failure(errorMessage);
            }

            vendor.IsActive = request.IsActive;
            vendor.ModifiedOn = DateTime.UtcNow;
            vendor.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Vendor '{VendorId}' status updated to {IsActive}", request.VendorId, request.IsActive);
            return UpdateVendorStatusResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating vendor status '{VendorId}'", request.VendorId);
            return UpdateVendorStatusResult.Failure("An unexpected error occurred while updating the vendor status.");
        }
    }

    /// <inheritdoc />
    public async Task<DeleteVendorResult> DeleteVendorAsync(DeleteVendorRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _deleteValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return DeleteVendorResult.Failure(errorMessage);
            }

            var vendor = await _dbContext.Vendors
                .FirstOrDefaultAsync(v => v.Id == request.VendorId, cancellationToken)
                .ConfigureAwait(false);

            if (vendor is null)
            {
                return DeleteVendorResult.Failure("Vendor not found.");
            }

            vendor.IsDeleted = true;
            vendor.DeletedOn = DateTime.UtcNow;
            vendor.ModifiedOn = DateTime.UtcNow;
            vendor.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Vendor '{VendorId}' deleted successfully", request.VendorId);
            return DeleteVendorResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting vendor '{VendorId}'", request.VendorId);
            return DeleteVendorResult.Failure("An unexpected error occurred while deleting the vendor.");
        }
    }
}
