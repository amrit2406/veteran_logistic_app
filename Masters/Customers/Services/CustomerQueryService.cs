using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VeteranLogistics.Data.Context;
using CustomerEntity = VeteranLogistics.Data.Entities.Administration.Customer;
using veteran_logistic.Masters.Customers.Contracts;
using veteran_logistic.Masters.Customers.Models;

namespace veteran_logistic.Masters.Customers.Services;

/// <summary>
/// Implementation of the customer query service.
/// </summary>
public sealed class CustomerQueryService : ICustomerQueryService
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<CustomerQueryService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerQueryService"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    public CustomerQueryService(VeteranLogisticsDbContext dbContext, ILogger<CustomerQueryService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<CustomerListItem>> GetAllCustomersAsync(CancellationToken cancellationToken = default)
    {
        return await ProjectToListItem(_dbContext.Customers.AsNoTracking())
            .OrderBy(c => c.CustomerName)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<CustomerListItem>> SearchCustomersAsync(string? search, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Customers.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchPattern = $"%{search}%";
            query = query.Where(c =>
                EF.Functions.Like(c.CustomerCode, searchPattern) ||
                EF.Functions.Like(c.CustomerName, searchPattern) ||
                EF.Functions.Like(c.City, searchPattern) ||
                EF.Functions.Like(c.State, searchPattern));
        }

        return await ProjectToListItem(query)
            .OrderBy(c => c.CustomerName)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<CustomerModel?> GetCustomerForEditAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Customers
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CustomerModel
            {
                Id = c.Id,
                CustomerCode = c.CustomerCode,
                CustomerName = c.CustomerName,
                AddressLine1 = c.AddressLine1,
                AddressLine2 = c.AddressLine2,
                City = c.City,
                State = c.State,
                Country = c.Country,
                PostalCode = c.PostalCode,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                GSTNumber = c.GSTNumber,
                PANNumber = c.PANNumber,
                IsActive = c.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    private static IQueryable<CustomerListItem> ProjectToListItem(IQueryable<CustomerEntity> query)
    {
        return query.Select(c => new CustomerListItem
        {
            Id = c.Id,
            CustomerCode = c.CustomerCode,
            CustomerName = c.CustomerName,
            GSTNumber = c.GSTNumber,
            PANNumber = c.PANNumber,
            City = c.City,
            State = c.State,
            IsActive = c.IsActive
        });
    }
}
