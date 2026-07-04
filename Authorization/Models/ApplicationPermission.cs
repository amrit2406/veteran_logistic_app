namespace veteran_logistic.Authorization.Models;

/// <summary>
/// Defines the application permissions for permission-based authorization.
/// </summary>
public sealed record ApplicationPermission
{
    private readonly string _id;

    /// <summary>
    /// Gets the unique identifier for the permission.
    /// </summary>
    public string Id => _id;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationPermission"/> class.
    /// </summary>
    /// <param name="id">The unique identifier for the permission.</param>
    private ApplicationPermission(string id)
    {
        _id = id;
    }

    /// <summary>
    /// No permission assigned.
    /// </summary>
    public static readonly ApplicationPermission None = new ApplicationPermission("none");

    // Driver Management Permissions
    /// <summary>
    /// Permission to view drivers.
    /// </summary>
    public static readonly ApplicationPermission ViewDrivers = new ApplicationPermission("drivers.view");

    /// <summary>
    /// Permission to create drivers.
    /// </summary>
    public static readonly ApplicationPermission CreateDrivers = new ApplicationPermission("drivers.create");

    /// <summary>
    /// Permission to edit drivers.
    /// </summary>
    public static readonly ApplicationPermission EditDrivers = new ApplicationPermission("drivers.edit");

    /// <summary>
    /// Permission to delete drivers.
    /// </summary>
    public static readonly ApplicationPermission DeleteDrivers = new ApplicationPermission("drivers.delete");

    // Vehicle Management Permissions
    /// <summary>
    /// Permission to view vehicles.
    /// </summary>
    public static readonly ApplicationPermission ViewVehicles = new ApplicationPermission("vehicles.view");

    /// <summary>
    /// Permission to create vehicles.
    /// </summary>
    public static readonly ApplicationPermission CreateVehicles = new ApplicationPermission("vehicles.create");

    /// <summary>
    /// Permission to edit vehicles.
    /// </summary>
    public static readonly ApplicationPermission EditVehicles = new ApplicationPermission("vehicles.edit");

    /// <summary>
    /// Permission to delete vehicles.
    /// </summary>
    public static readonly ApplicationPermission DeleteVehicles = new ApplicationPermission("vehicles.delete");

    // Trip Management Permissions
    /// <summary>
    /// Permission to view trips.
    /// </summary>
    public static readonly ApplicationPermission ViewTrips = new ApplicationPermission("trips.view");

    /// <summary>
    /// Permission to create trips.
    /// </summary>
    public static readonly ApplicationPermission CreateTrips = new ApplicationPermission("trips.create");

    /// <summary>
    /// Permission to edit trips.
    /// </summary>
    public static readonly ApplicationPermission EditTrips = new ApplicationPermission("trips.edit");

    /// <summary>
    /// Permission to delete trips.
    /// </summary>
    public static readonly ApplicationPermission DeleteTrips = new ApplicationPermission("trips.delete");

    // Dispatch Management Permissions
    /// <summary>
    /// Permission to view dispatches.
    /// </summary>
    public static readonly ApplicationPermission ViewDispatches = new ApplicationPermission("dispatches.view");

    /// <summary>
    /// Permission to create dispatches.
    /// </summary>
    public static readonly ApplicationPermission CreateDispatches = new ApplicationPermission("dispatches.create");

    /// <summary>
    /// Permission to edit dispatches.
    /// </summary>
    public static readonly ApplicationPermission EditDispatches = new ApplicationPermission("dispatches.edit");

    /// <summary>
    /// Permission to delete dispatches.
    /// </summary>
    public static readonly ApplicationPermission DeleteDispatches = new ApplicationPermission("dispatches.delete");
}
