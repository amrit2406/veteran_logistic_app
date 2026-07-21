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

            // Auto-generate Code
            string generatedCode = await GenerateVendorCodeAsync(cancellationToken).ConfigureAwait(false);

            var vendor = new VendorEntity
            {
                Code = generatedCode,
                Type = request.Type,
                Name = request.Name,
                CorrespondenceAddress = request.CorrespondenceAddress,
                City = request.City,
                BillingAddress = request.BillingAddress,
                Phone = request.Phone,
                Mobile = request.Mobile,
                Fax = request.Fax,
                Email = request.Email,
                ServiceTax = request.ServiceTax,
                CST = request.CST,
                PAN = request.PAN,
                GSTIN = request.GSTIN,
                IsActive = request.IsActive,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System", // TODO: Replace with actual user from session
                ModifiedBy = "System"
            };

            _dbContext.Vendors.Add(vendor);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Vendor '{Code}' created successfully with ID {VendorId}", generatedCode, vendor.Id);
            return CreateVendorResult.Success(vendor.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating vendor");
            return CreateVendorResult.Failure("An unexpected error occurred while creating the vendor.");
        }
    }

    /// <summary>
    /// Generates a unique vendor code.
    /// </summary>
    private async Task<string> GenerateVendorCodeAsync(CancellationToken cancellationToken)
    {
        // Get the highest existing vendor ID to generate a sequential code
        int maxId = await _dbContext.Vendors
            .AsNoTracking()
            .MaxAsync(v => (int?)v.Id, cancellationToken)
            .ConfigureAwait(false) ?? 0;

        // Generate code as VND followed by a 6-digit number (e.g., VND000001)
        int nextId = maxId + 1;
        return $"VND{nextId:D6}";
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

            // Check for duplicate Code (excluding current vendor)
            var existingVendorByCode = await _dbContext.Vendors
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Code == request.Code && v.Id != request.VendorId, cancellationToken)
                .ConfigureAwait(false);

            if (existingVendorByCode is not null)
            {
                return UpdateVendorResult.Failure("A vendor with this code already exists.");
            }

            vendor.Code = request.Code;
            vendor.Type = request.Type;
            vendor.Name = request.Name;
            vendor.CorrespondenceAddress = request.CorrespondenceAddress;
            vendor.City = request.City;
            vendor.BillingAddress = request.BillingAddress;
            vendor.Phone = request.Phone;
            vendor.Mobile = request.Mobile;
            vendor.Fax = request.Fax;
            vendor.Email = request.Email;
            vendor.ServiceTax = request.ServiceTax;
            vendor.CST = request.CST;
            vendor.PAN = request.PAN;
            vendor.GSTIN = request.GSTIN;
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
