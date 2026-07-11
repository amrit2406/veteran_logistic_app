using veteran_logistic.Masters.FuelPumps.Models;

namespace veteran_logistic.Masters.FuelPumps.Contracts;

/// <summary>
/// Service contract for fuel pump command operations.
/// </summary>
public interface IFuelPumpCommandService
{
    /// <summary>
    /// Creates a new fuel pump.
    /// </summary>
    /// <param name="request">The fuel pump creation request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result containing the created fuel pump ID.</returns>
    Task<CreateFuelPumpResult> CreateFuelPumpAsync(CreateFuelPumpRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing fuel pump.
    /// </summary>
    /// <param name="request">The fuel pump update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdateFuelPumpResult> UpdateFuelPumpAsync(UpdateFuelPumpRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a fuel pump's active status.
    /// </summary>
    /// <param name="request">The fuel pump status update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<UpdateFuelPumpStatusResult> UpdateFuelPumpStatusAsync(UpdateFuelPumpStatusRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a fuel pump (soft delete).
    /// </summary>
    /// <param name="request">The delete fuel pump request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<DeleteFuelPumpResult> DeleteFuelPumpAsync(DeleteFuelPumpRequest request, CancellationToken cancellationToken = default);
}
