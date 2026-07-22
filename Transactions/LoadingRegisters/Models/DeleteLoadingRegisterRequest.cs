namespace veteran_logistic.Transactions.LoadingRegisters.Models;

/// <summary>
/// Request model for deleting a loading register.
/// </summary>
public sealed class DeleteLoadingRegisterRequest
{
    /// <summary>
    /// Gets or sets the loading register ID.
    /// </summary>
    public int LoadingRegisterId { get; set; }
}
