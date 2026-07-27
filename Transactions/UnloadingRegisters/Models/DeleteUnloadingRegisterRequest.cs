namespace veteran_logistic.Transactions.UnloadingRegisters.Models;

/// <summary>
/// Request model for deleting an unloading register.
/// </summary>
public sealed class DeleteUnloadingRegisterRequest
{
    /// <summary>
    /// Gets or sets the unloading register ID.
    /// </summary>
    public int UnloadingRegisterId { get; set; }
}
