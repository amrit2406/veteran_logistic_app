namespace veteran_logistic.Reports.DOStatusReport.DTOs;

/// <summary>
/// Represents types of exceptions detected in a Delivery Order.
/// </summary>
public enum DOExceptionType
{
    /// <summary>
    /// No exception detected.
    /// </summary>
    None,

    /// <summary>
    /// Unloading is missing beyond the configured delay period.
    /// </summary>
    MissingUnloading,

    /// <summary>
    /// Payment amount does not match expected amount.
    /// </summary>
    PaymentMismatch,

    /// <summary>
    /// Negative shortage weight detected.
    /// </summary>
    NegativeShortage,

    /// <summary>
    /// Weight mismatch between loading and unloading beyond tolerance.
    /// </summary>
    WeightMismatch,

    /// <summary>
    /// Bill is missing after payment completion.
    /// </summary>
    MissingBill,

    /// <summary>
    /// Invalid or broken reference to related records.
    /// </summary>
    InvalidReference
}
