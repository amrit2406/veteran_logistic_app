namespace veteran_logistic.Transactions.LoadingRegisters.Models;

/// <summary>
/// Request model for updating loading register status.
/// </summary>
public sealed class UpdateLoadingRegisterStatusRequest
{
    /// <summary>
    /// Gets or sets the loading register ID.
    /// </summary>
    public int LoadingRegisterId { get; set; }

    /// <summary>
    /// Gets or sets whether the loading register should be active.
    /// </summary>
    public bool IsActive { get; set; }
}
