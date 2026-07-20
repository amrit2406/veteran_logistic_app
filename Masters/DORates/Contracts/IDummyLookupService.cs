using veteran_logistic.Masters.DORates.Models;

namespace veteran_logistic.Masters.DORates.Contracts;

/// <summary>
/// Service for providing dummy lookup data for Consignors and Consignees.
/// This is a temporary service that can be replaced with real Master lookups later.
/// </summary>
public interface IDummyLookupService
{
    /// <summary>
    /// Gets the collection of dummy Consignors.
    /// </summary>
    /// <returns>The collection of Consignor lookup items.</returns>
    IEnumerable<LookupItem> GetConsignors();

    /// <summary>
    /// Gets the collection of dummy Consignees.
    /// </summary>
    /// <returns>The collection of Consignee lookup items.</returns>
    IEnumerable<LookupItem> GetConsignees();

    /// <summary>
    /// Gets the name of a Consignor by ID.
    /// </summary>
    /// <param name="consignorId">The Consignor ID.</param>
    /// <returns>The Consignor name, or empty string if not found.</returns>
    string GetConsignorName(int consignorId);

    /// <summary>
    /// Gets the name of a Consignee by ID.
    /// </summary>
    /// <param name="consigneeId">The Consignee ID.</param>
    /// <returns>The Consignee name, or empty string if not found.</returns>
    string GetConsigneeName(int consigneeId);
}
