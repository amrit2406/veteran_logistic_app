namespace veteran_logistic.Transactions.UnloadingRegisters.Models;

/// <summary>
/// Request model for updating unloading register status.
/// </summary>
public sealed class UpdateUnloadingRegisterStatusRequest
{
    /// <summary>
    /// Gets or sets the unloading register ID.
    /// </summary>
    public int UnloadingRegisterId { get; set; }

    /// <summary>
    /// Gets or sets whether the unloading register should be active.
    /// </summary>
    public bool IsActive { get; set; }
}
