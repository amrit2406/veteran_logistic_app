using veteran_logistic.Masters.DORates.Contracts;
using veteran_logistic.Masters.DORates.Models;

namespace veteran_logistic.Masters.DORates.Services;

/// <summary>
/// Implementation of the dummy lookup service for Consignors and Consignees.
/// This provides in-memory dummy data that can be replaced with real Master lookups later.
/// </summary>
public sealed class DummyLookupService : IDummyLookupService
{
    private readonly List<LookupItem> _consignors = new()
    {
        new LookupItem { Id = 1, Name = "ABC Industries" },
        new LookupItem { Id = 2, Name = "Reliance" },
        new LookupItem { Id = 3, Name = "Tata Steel" },
        new LookupItem { Id = 4, Name = "JSW" }
    };

    private readonly List<LookupItem> _consignees = new()
    {
        new LookupItem { Id = 1, Name = "Veteran Logistics" },
        new LookupItem { Id = 2, Name = "XYZ Traders" },
        new LookupItem { Id = 3, Name = "Eastern Minerals" },
        new LookupItem { Id = 4, Name = "Ultra Cement" }
    };

    /// <inheritdoc />
    public IEnumerable<LookupItem> GetConsignors()
    {
        return _consignors;
    }

    /// <inheritdoc />
    public IEnumerable<LookupItem> GetConsignees()
    {
        return _consignees;
    }

    /// <inheritdoc />
    public string GetConsignorName(int consignorId)
    {
        var consignor = _consignors.FirstOrDefault(c => c.Id == consignorId);
        return consignor?.Name ?? string.Empty;
    }

    /// <inheritdoc />
    public string GetConsigneeName(int consigneeId)
    {
        var consignee = _consignees.FirstOrDefault(c => c.Id == consigneeId);
        return consignee?.Name ?? string.Empty;
    }
}
