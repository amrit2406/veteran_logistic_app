namespace veteran_logistic.Reports.PartyBillingReport.DTOs;

/// <summary>
/// Represents a party billing report item for display in the summary grid.
/// </summary>
public sealed class PartyBillingReportItem
{
    /// <summary>
    /// Gets or sets the party bill register ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the bill number.
    /// </summary>
    public string BillNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the bill date.
    /// </summary>
    public DateTime BillDate { get; set; }

    /// <summary>
    /// Gets or sets the customer/party name.
    /// </summary>
    public string Customer { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the third party name.
    /// </summary>
    public string ThirdParty { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the permit number.
    /// </summary>
    public string? PermitNumber { get; set; }

    /// <summary>
    /// Gets or sets the from date filter.
    /// </summary>
    public DateTime? FromDate { get; set; }

    /// <summary>
    /// Gets or sets the to date filter.
    /// </summary>
    public DateTime? ToDate { get; set; }

    /// <summary>
    /// Gets or sets the number of challans in the bill.
    /// </summary>
    public int NumberOfChallans { get; set; }

    /// <summary>
    /// Gets or sets the total loading weight.
    /// </summary>
    public decimal TotalLoadingWeight { get; set; }

    /// <summary>
    /// Gets or sets the total bill amount.
    /// </summary>
    public decimal TotalBillAmount { get; set; }

    /// <summary>
    /// Gets or sets the bill status.
    /// </summary>
    public string Status { get; set; } = string.Empty;
}
