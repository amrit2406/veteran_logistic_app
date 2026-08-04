namespace veteran_logistic.Reports.DOStatusReport.DTOs;

/// <summary>
/// Represents the billing status of a Delivery Order.
/// </summary>
public enum BillingStatusType
{
    /// <summary>
    /// Bill has not been generated yet.
    /// </summary>
    NotGenerated,

    /// <summary>
    /// Bill has been generated and is active.
    /// </summary>
    Generated,

    /// <summary>
    /// Bill has been cancelled.
    /// </summary>
    Cancelled,

    /// <summary>
    /// Bill has been closed and settled.
    /// </summary>
    Closed
}
