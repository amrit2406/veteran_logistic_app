namespace veteran_logistic.Reports.PaymentReport.DTOs;

/// <summary>
/// Represents a payment report item for display in the payment report grid.
/// </summary>
public sealed class PaymentReportItem
{
    /// <summary>
    /// Gets or sets the payment register ID.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the payment date.
    /// </summary>
    public DateTime PaymentDate { get; set; }

    /// <summary>
    /// Gets or sets the challan number.
    /// </summary>
    public string ChallanNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the TP number.
    /// </summary>
    public string TPNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the vehicle number.
    /// </summary>
    public string? VehicleNumber { get; set; }

    /// <summary>
    /// Gets or sets the loading date.
    /// </summary>
    public DateTime? LoadingDate { get; set; }

    /// <summary>
    /// Gets or sets the unloading date.
    /// </summary>
    public DateTime? UnloadingDate { get; set; }

    /// <summary>
    /// Gets or sets the customer name.
    /// </summary>
    public string? CustomerName { get; set; }

    /// <summary>
    /// Gets or sets the material name.
    /// </summary>
    public string? MaterialName { get; set; }

    /// <summary>
    /// Gets or sets the driver name.
    /// </summary>
    public string Driver { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the vehicle owner name.
    /// </summary>
    public string? VehicleOwner { get; set; }

    /// <summary>
    /// Gets or sets the payment location name.
    /// </summary>
    public string? PaymentLocationName { get; set; }

    /// <summary>
    /// Gets or sets the payment type.
    /// </summary>
    public string PaymentType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the beneficiary name.
    /// </summary>
    public string Beneficiary { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the PAN number.
    /// </summary>
    public string PAN { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the bank name.
    /// </summary>
    public string BankName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the account number.
    /// </summary>
    public string AccountNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the IFSC code.
    /// </summary>
    public string IFSCCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the UTR number.
    /// </summary>
    public string UTRNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the mobile number.
    /// </summary>
    public string MobileNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the loading weight.
    /// </summary>
    public decimal LoadingWeight { get; set; }

    /// <summary>
    /// Gets or sets the unloading weight.
    /// </summary>
    public decimal UnloadingWeight { get; set; }

    /// <summary>
    /// Gets or sets the driver commission.
    /// </summary>
    public decimal DriverCommission { get; set; }

    /// <summary>
    /// Gets or sets the challan amount.
    /// </summary>
    public decimal ChallanAmount { get; set; }

    /// <summary>
    /// Gets or sets the TDS amount.
    /// </summary>
    public decimal TDSAmount { get; set; }

    /// <summary>
    /// Gets or sets the surcharge amount.
    /// </summary>
    public decimal SurchargeAmount { get; set; }

    /// <summary>
    /// Gets or sets the admin charge.
    /// </summary>
    public decimal AdminCharge { get; set; }

    /// <summary>
    /// Gets or sets the net payment.
    /// </summary>
    public decimal NetPayment { get; set; }

    /// <summary>
    /// Gets or sets the notes.
    /// </summary>
    public string Notes { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the payment status.
    /// </summary>
    public string PaymentStatus { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the status (active/inactive).
    /// </summary>
    public bool IsActive { get; set; }
}
