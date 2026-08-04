namespace veteran_logistic.Reports.DOStatusReport.DTOs;

/// <summary>
/// Represents the operational status of a Delivery Order (DO).
/// </summary>
public enum DOStatus
{
    /// <summary>
    /// DO has been loaded but not yet unloaded.
    /// </summary>
    Loaded,

    /// <summary>
    /// DO is in transit (loaded but unloading not yet recorded).
    /// </summary>
    InTransit,

    /// <summary>
    /// DO has been unloaded.
    /// </summary>
    Unloaded,

    /// <summary>
    /// Payment is pending for the DO.
    /// </summary>
    PaymentPending,

    /// <summary>
    /// Payment has been completed.
    /// </summary>
    PaymentCompleted,

    /// <summary>
    /// Party bill is pending.
    /// </summary>
    BillPending,

    /// <summary>
    /// DO is fully completed (loaded, unloaded, paid, and billed).
    /// </summary>
    Completed
}
