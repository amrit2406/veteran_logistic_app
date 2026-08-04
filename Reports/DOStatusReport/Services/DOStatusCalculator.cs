using VeteranLogistics.Data.Entities.Administration;
using veteran_logistic.Reports.DOStatusReport.DTOs;

namespace veteran_logistic.Reports.DOStatusReport.Services;

/// <summary>
/// Centralized business logic for calculating DO status, payment status, billing status, and exceptions.
/// </summary>
public static class DOStatusCalculator
{
    private const int DefaultDelayThresholdDays = 3;
    private const decimal WeightTolerancePercentage = 0.05m; // 5% tolerance

    /// <summary>
    /// Calculates the DO status based on the presence of unloading, payment, and billing records.
    /// </summary>
    /// <param name="hasUnloading">Whether unloading record exists.</param>
    /// <param name="hasPayment">Whether payment record exists.</param>
    /// <param name="hasBill">Whether bill record exists.</param>
    /// <returns>The calculated DO status.</returns>
    public static DOStatus CalculateDOStatus(bool hasUnloading, bool hasPayment, bool hasBill)
    {
        // Status calculation follows the workflow: Loading -> Unloading -> Payment -> Billing -> Completed
        if (hasBill)
        {
            return DOStatus.Completed;
        }

        if (hasPayment)
        {
            return DOStatus.BillPending;
        }

        if (hasUnloading)
        {
            return DOStatus.PaymentPending;
        }

        return DOStatus.InTransit;
    }

    /// <summary>
    /// Converts the entity PaymentStatus string to the strongly typed PaymentStatusType enum.
    /// </summary>
    /// <param name="entityPaymentStatus">The payment status from the entity.</param>
    /// <returns>The strongly typed payment status.</returns>
    public static PaymentStatusType ConvertPaymentStatus(string? entityPaymentStatus)
    {
        if (string.IsNullOrWhiteSpace(entityPaymentStatus))
        {
            return PaymentStatusType.Pending;
        }

        return entityPaymentStatus.ToLowerInvariant() switch
        {
            "paid" => PaymentStatusType.Paid,
            "partially paid" or "partial" => PaymentStatusType.PartiallyPaid,
            "cancelled" => PaymentStatusType.Cancelled,
            _ => PaymentStatusType.Pending
        };
    }

    /// <summary>
    /// Converts the entity billing status to the strongly typed BillingStatusType enum.
    /// </summary>
    /// <param name="hasBill">Whether a bill record exists.</param>
    /// <param name="billStatus">The bill status from the entity (if available).</param>
    /// <returns>The strongly typed billing status.</returns>
    public static BillingStatusType ConvertBillingStatus(bool hasBill, string? billStatus = null)
    {
        if (!hasBill)
        {
            return BillingStatusType.NotGenerated;
        }

        if (string.IsNullOrWhiteSpace(billStatus))
        {
            return BillingStatusType.Generated;
        }

        return billStatus.ToLowerInvariant() switch
        {
            "cancelled" => BillingStatusType.Cancelled,
            "closed" => BillingStatusType.Closed,
            _ => BillingStatusType.Generated
        };
    }

    /// <summary>
    /// Detects exceptions in the DO data.
    /// </summary>
    /// <param name="loadingDate">The loading date.</param>
    /// <param name="hasUnloading">Whether unloading record exists.</param>
    /// <param name="unloadingDate">The unloading date (if exists).</param>
    /// <param name="loadingWeight">The loading weight.</param>
    /// <param name="unloadingWeight">The unloading weight.</param>
    /// <param name="shortageWeight">The shortage weight.</param>
    /// <param name="grossAmount">The gross amount.</param>
    /// <param name="payableAmount">The payable amount from payment (if exists).</param>
    /// <param name="hasPayment">Whether payment record exists.</param>
    /// <param name="hasBill">Whether bill record exists.</param>
    /// <param name="delayThresholdDays">The delay threshold in days (default: 3).</param>
    /// <returns>The detected exception type.</returns>
    public static DOExceptionType DetectException(
        DateTime loadingDate,
        bool hasUnloading,
        DateTime? unloadingDate,
        decimal loadingWeight,
        decimal unloadingWeight,
        decimal shortageWeight,
        decimal grossAmount,
        decimal? payableAmount,
        bool hasPayment,
        bool hasBill,
        int delayThresholdDays = DefaultDelayThresholdDays)
    {
        // Check for missing unloading beyond threshold
        if (!hasUnloading)
        {
            var daysSinceLoading = (DateTime.Today - loadingDate.Date).Days;
            if (daysSinceLoading > delayThresholdDays)
            {
                return DOExceptionType.MissingUnloading;
            }
        }

        // Check for negative shortage
        if (shortageWeight < 0)
        {
            return DOExceptionType.NegativeShortage;
        }

        // Check for weight mismatch
        if (hasUnloading && loadingWeight > 0)
        {
            var expectedUnloading = loadingWeight - shortageWeight;
            var tolerance = loadingWeight * WeightTolerancePercentage;
            if (Math.Abs(unloadingWeight - expectedUnloading) > tolerance)
            {
                return DOExceptionType.WeightMismatch;
            }
        }

        // Check for payment mismatch
        if (hasPayment && payableAmount.HasValue)
        {
            var tolerance = grossAmount * WeightTolerancePercentage;
            if (Math.Abs(payableAmount.Value - grossAmount) > tolerance && payableAmount.Value > 0)
            {
                return DOExceptionType.PaymentMismatch;
            }
        }

        // Check for missing bill after payment
        if (hasPayment && !hasBill)
        {
            var daysSincePayment = (DateTime.Today - (unloadingDate ?? loadingDate)).Days;
            if (daysSinceLoading > delayThresholdDays)
            {
                return DOExceptionType.MissingBill;
            }
        }

        return DOExceptionType.None;
    }

    /// <summary>
    /// Calculates the age of the DO in days since loading.
    /// </summary>
    /// <param name="loadingDate">The loading date.</param>
    /// <returns>The age in days.</returns>
    public static int CalculateAgeInDays(DateTime loadingDate)
    {
        return (DateTime.Today - loadingDate.Date).Days;
    }

    /// <summary>
    /// Determines if the DO is delayed beyond the threshold.
    /// </summary>
    /// <param name="loadingDate">The loading date.</param>
    /// <param name="hasBill">Whether the DO is fully billed (completed).</param>
    /// <param name="delayThresholdDays">The delay threshold in days (default: 3).</param>
    /// <returns>A tuple indicating if delayed and the delay days.</returns>
    public static (bool IsDelayed, int DelayDays) CalculateDelay(
        DateTime loadingDate,
        bool hasBill,
        int delayThresholdDays = DefaultDelayThresholdDays)
    {
        if (hasBill)
        {
            return (false, 0);
        }

        var ageInDays = CalculateAgeInDays(loadingDate);
        var isDelayed = ageInDays > delayThresholdDays;
        var delayDays = isDelayed ? ageInDays - delayThresholdDays : 0;

        return (isDelayed, delayDays);
    }
}
