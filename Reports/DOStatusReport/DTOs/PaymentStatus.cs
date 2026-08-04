namespace veteran_logistic.Reports.DOStatusReport.DTOs;

/// <summary>
/// Represents the payment status of a Delivery Order.
/// </summary>
public enum PaymentStatusType
{
    /// <summary>
    /// Payment is pending and has not been initiated.
    /// </summary>
    Pending,

    /// <summary>
    /// Payment has been partially made.
    /// </summary>
    PartiallyPaid,

    /// <summary>
    /// Payment has been completed in full.
    /// </summary>
    Paid,

    /// <summary>
    /// Payment has been cancelled.
    /// </summary>
    Cancelled
}
