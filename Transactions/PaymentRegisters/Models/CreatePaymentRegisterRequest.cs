namespace veteran_logistic.Transactions.PaymentRegisters.Models;

/// <summary>
/// Request model for creating a new payment register.
/// </summary>
public sealed class CreatePaymentRegisterRequest
{
    /// <summary>
    /// Gets or sets the challan number (from Loading/Unloading Register).
    /// </summary>
    public string ChallanNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the loading register ID.
    /// </summary>
    public int? LoadingRegisterId { get; set; }

    /// <summary>
    /// Gets or sets the unloading register ID.
    /// </summary>
    public int? UnloadingRegisterId { get; set; }

    /// <summary>
    /// Gets or sets the payment date.
    /// </summary>
    public DateTime PaymentDate { get; set; }

    /// <summary>
    /// Gets or sets the payment location ID.
    /// </summary>
    public int? PaymentLocationId { get; set; }

    /// <summary>
    /// Gets or sets the payment type.
    /// </summary>
    public string PaymentType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the HSD party.
    /// </summary>
    public string? HSDParty { get; set; }

    /// <summary>
    /// Gets or sets the notes.
    /// </summary>
    public string Notes { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the beneficiary name.
    /// </summary>
    public string Beneficiary { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the PAN number.
    /// </summary>
    public string PAN { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the UTR number.
    /// </summary>
    public string UTRNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the mobile number.
    /// </summary>
    public string MobileNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the account number.
    /// </summary>
    public string AccountNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the IFSC code.
    /// </summary>
    public string IFSCCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the bank name.
    /// </summary>
    public string BankName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the TDS percentage.
    /// </summary>
    public decimal TDSPercentage { get; set; }

    /// <summary>
    /// Gets or sets the challan money.
    /// </summary>
    public decimal ChallanMoney { get; set; }

    /// <summary>
    /// Gets or sets the surcharge at 2%.
    /// </summary>
    public decimal Surcharge { get; set; }

    /// <summary>
    /// Gets or sets the admin charge.
    /// </summary>
    public decimal AdminCharge { get; set; }

    /// <summary>
    /// Gets or sets whether the payment register is active.
    /// </summary>
    public bool IsActive { get; set; } = true;
}
