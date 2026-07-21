using Microsoft.Extensions.Logging;
using veteran_logistic.Masters.Customers.Contracts;
using veteran_logistic.Masters.DORates.Contracts;
using veteran_logistic.Masters.DORates.Models;

namespace veteran_logistic.Masters.DORates.Services;

/// <summary>
/// Implementation of the lookup service for Consignors and Consignees.
/// This service uses the Customer entity to provide real customer data.
/// </summary>
public sealed class DummyLookupService : IDummyLookupService
{
    private readonly ICustomerQueryService _customerQueryService;
    private readonly ILogger<DummyLookupService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DummyLookupService"/> class.
    /// </summary>
    /// <param name="customerQueryService">The customer query service.</param>
    /// <param name="logger">The logger.</param>
    public DummyLookupService(ICustomerQueryService customerQueryService, ILogger<DummyLookupService> logger)
    {
        _customerQueryService = customerQueryService ?? throw new ArgumentNullException(nameof(customerQueryService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IEnumerable<LookupItem>> GetConsignorsAsync(CancellationToken cancellationToken = default)
    {
        var customers = await _customerQueryService.GetAllCustomersAsync(cancellationToken).ConfigureAwait(false);
        _logger.LogInformation("Retrieved {Count} customers for Consignor lookup", customers.Count);
        
        return customers
            .Where(c => c.IsActive)
            .Select(c => new LookupItem { Id = c.Id, Name = c.CustomerName })
            .OrderBy(c => c.Name);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<LookupItem>> GetConsigneesAsync(CancellationToken cancellationToken = default)
    {
        var customers = await _customerQueryService.GetAllCustomersAsync(cancellationToken).ConfigureAwait(false);
        _logger.LogInformation("Retrieved {Count} customers for Consignee lookup", customers.Count);
        
        return customers
            .Where(c => c.IsActive)
            .Select(c => new LookupItem { Id = c.Id, Name = c.CustomerName })
            .OrderBy(c => c.Name);
    }

    /// <inheritdoc />
    public async Task<string> GetConsignorNameAsync(int consignorId, CancellationToken cancellationToken = default)
    {
        var customers = await _customerQueryService.GetAllCustomersAsync(cancellationToken).ConfigureAwait(false);
        var customer = customers.FirstOrDefault(c => c.Id == consignorId);
        return customer?.CustomerName ?? string.Empty;
    }

    /// <inheritdoc />
    public async Task<string> GetConsigneeNameAsync(int consigneeId, CancellationToken cancellationToken = default)
    {
        var customers = await _customerQueryService.GetAllCustomersAsync(cancellationToken).ConfigureAwait(false);
        var customer = customers.FirstOrDefault(c => c.Id == consigneeId);
        return customer?.CustomerName ?? string.Empty;
    }
}
