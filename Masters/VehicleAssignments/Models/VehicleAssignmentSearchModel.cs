namespace veteran_logistic.Masters.VehicleAssignments.Models;

/// <summary>
/// Represents a search model for filtering vehicle assignments.
/// </summary>
public sealed class VehicleAssignmentSearchModel
{
    /// <summary>
    /// Gets or sets the search text to filter vehicles by vehicle number.
    /// </summary>
    public string SearchText { get; set; } = string.Empty;
}
