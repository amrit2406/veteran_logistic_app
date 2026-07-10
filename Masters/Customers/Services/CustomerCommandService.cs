using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using CustomerEntity = VeteranLogistics.Data.Entities.Administration.Customer;
using veteran_logistic.Masters.Customers.Contracts;
using veteran_logistic.Masters.Customers.Models;

namespace veteran_logistic.Masters.Customers.Services;

/// <summary>
/// Implementation of the customer command service.
/// </summary>
public sealed class CustomerCommandService : ICustomerCommandService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ICreateCustomerValidator _createValidator;
    private readonly IUpdateCustomerValidator _updateValidator;
    private readonly IUpdateCustomerStatusValidator _updateStatusValidator;
    private readonly IDeleteCustomerValidator _deleteValidator;
    private readonly ILogger<CustomerCommandService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerCommandService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="createValidator">The customer creation validator.</param>
    /// <param name="updateValidator">The customer update validator.</param>
    /// <param name="updateStatusValidator">The customer status update validator.</param>
    /// <param name="deleteValidator">The delete customer validator.</param>
    /// <param name="logger">The logger.</param>
    public CustomerCommandService(
        VeteranLogisticsDbContext dbContext,
        ICreateCustomerValidator createValidator,
        IUpdateCustomerValidator updateValidator,
        IUpdateCustomerStatusValidator updateStatusValidator,
        IDeleteCustomerValidator deleteValidator,
        ILogger<CustomerCommandService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _createValidator = createValidator ?? throw new ArgumentNullException(nameof(createValidator));
        _updateValidator = updateValidator ?? throw new ArgumentNullException(nameof(updateValidator));
        _updateStatusValidator = updateStatusValidator ?? throw new ArgumentNullException(nameof(updateStatusValidator));
        _deleteValidator = deleteValidator ?? throw new ArgumentNullException(nameof(deleteValidator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<CreateCustomerResult> CreateCustomerAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _createValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return CreateCustomerResult.Failure(errorMessage);
            }

            // Check for duplicate CustomerCode
            var existingCustomerByCode = await _dbContext.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CustomerCode == request.CustomerCode, cancellationToken)
                .ConfigureAwait(false);

            if (existingCustomerByCode is not null)
            {
                return CreateCustomerResult.Failure("A customer with this customer code already exists.");
            }

            // Check for duplicate GSTNumber
            var existingCustomerByGST = await _dbContext.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.GSTNumber == request.GSTNumber, cancellationToken)
                .ConfigureAwait(false);

            if (existingCustomerByGST is not null)
            {
                return CreateCustomerResult.Failure("A customer with this GST number already exists.");
            }

            var customer = new CustomerEntity
            {
                CustomerCode = request.CustomerCode,
                CustomerName = request.CustomerName,
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

            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Customer '{CustomerCode}' created successfully with ID {CustomerId}", request.CustomerCode, customer.Id);
            return CreateCustomerResult.Success(customer.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating customer '{CustomerCode}'", request.CustomerCode);
            return CreateCustomerResult.Failure("An unexpected error occurred while creating the customer.");
        }
    }

    /// <inheritdoc />
    public async Task<UpdateCustomerResult> UpdateCustomerAsync(UpdateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _updateValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdateCustomerResult.Failure(errorMessage);
            }

            var customer = await _dbContext.Customers
                .FirstOrDefaultAsync(c => c.Id == request.CustomerId, cancellationToken)
                .ConfigureAwait(false);

            if (customer is null)
            {
                return UpdateCustomerResult.Failure("Customer not found.");
            }

            // Check for duplicate CustomerCode (excluding current customer)
            var existingCustomerByCode = await _dbContext.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CustomerCode == request.CustomerCode && c.Id != request.CustomerId, cancellationToken)
                .ConfigureAwait(false);

            if (existingCustomerByCode is not null)
            {
                return UpdateCustomerResult.Failure("A customer with this customer code already exists.");
            }

            // Check for duplicate GSTNumber (excluding current customer)
            var existingCustomerByGST = await _dbContext.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.GSTNumber == request.GSTNumber && c.Id != request.CustomerId, cancellationToken)
                .ConfigureAwait(false);

            if (existingCustomerByGST is not null)
            {
                return UpdateCustomerResult.Failure("A customer with this GST number already exists.");
            }

            customer.CustomerCode = request.CustomerCode;
            customer.CustomerName = request.CustomerName;
            customer.AddressLine1 = request.AddressLine1;
            customer.AddressLine2 = request.AddressLine2;
            customer.City = request.City;
            customer.State = request.State;
            customer.Country = request.Country;
            customer.PostalCode = request.PostalCode;
            customer.PhoneNumber = request.PhoneNumber;
            customer.Email = request.Email;
            customer.GSTNumber = request.GSTNumber;
            customer.PANNumber = request.PANNumber;
            customer.IsActive = request.IsActive;
            customer.ModifiedOn = DateTime.UtcNow;
            customer.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Customer '{CustomerId}' updated successfully", request.CustomerId);
            return UpdateCustomerResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating customer '{CustomerId}'", request.CustomerId);
            return UpdateCustomerResult.Failure("An unexpected error occurred while updating the customer.");
        }
    }

    /// <inheritdoc />
    public async Task<UpdateCustomerStatusResult> UpdateCustomerStatusAsync(UpdateCustomerStatusRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var customer = await _dbContext.Customers
                .FirstOrDefaultAsync(c => c.Id == request.CustomerId, cancellationToken)
                .ConfigureAwait(false);

            if (customer is null)
            {
                return UpdateCustomerStatusResult.Failure("Customer not found.");
            }

            var validationResult = _updateStatusValidator.Validate(request, customer.IsActive);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return UpdateCustomerStatusResult.Failure(errorMessage);
            }

            customer.IsActive = request.IsActive;
            customer.ModifiedOn = DateTime.UtcNow;
            customer.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Customer '{CustomerId}' status updated to {IsActive}", request.CustomerId, request.IsActive);
            return UpdateCustomerStatusResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating customer status '{CustomerId}'", request.CustomerId);
            return UpdateCustomerStatusResult.Failure("An unexpected error occurred while updating the customer status.");
        }
    }

    /// <inheritdoc />
    public async Task<DeleteCustomerResult> DeleteCustomerAsync(DeleteCustomerRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _deleteValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                var errorMessage = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return DeleteCustomerResult.Failure(errorMessage);
            }

            var customer = await _dbContext.Customers
                .FirstOrDefaultAsync(c => c.Id == request.CustomerId, cancellationToken)
                .ConfigureAwait(false);

            if (customer is null)
            {
                return DeleteCustomerResult.Failure("Customer not found.");
            }

            customer.IsDeleted = true;
            customer.DeletedOn = DateTime.UtcNow;
            customer.ModifiedOn = DateTime.UtcNow;
            customer.ModifiedBy = "System"; // TODO: Replace with actual user from session

            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Customer '{CustomerId}' deleted successfully", request.CustomerId);
            return DeleteCustomerResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting customer '{CustomerId}'", request.CustomerId);
            return DeleteCustomerResult.Failure("An unexpected error occurred while deleting the customer.");
        }
    }
}
