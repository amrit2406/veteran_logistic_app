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

    // Administration - Users Permissions
    /// <summary>
    /// Permission to view users.
    /// </summary>
    public static readonly ApplicationPermission ViewUsers = new ApplicationPermission("administration.users.view");

    /// <summary>
    /// Permission to add users.
    /// </summary>
    public static readonly ApplicationPermission AddUsers = new ApplicationPermission("administration.users.add");

    /// <summary>
    /// Permission to edit users.
    /// </summary>
    public static readonly ApplicationPermission EditUsers = new ApplicationPermission("administration.users.edit");

    /// <summary>
    /// Permission to activate users.
    /// </summary>
    public static readonly ApplicationPermission ActivateUsers = new ApplicationPermission("administration.users.activate");

    /// <summary>
    /// Permission to delete users.
    /// </summary>
    public static readonly ApplicationPermission DeleteUsers = new ApplicationPermission("administration.users.delete");

    // Administration - Roles Permissions
    /// <summary>
    /// Permission to view roles.
    /// </summary>
    public static readonly ApplicationPermission ViewRoles = new ApplicationPermission("administration.roles.view");

    /// <summary>
    /// Permission to add roles.
    /// </summary>
    public static readonly ApplicationPermission AddRoles = new ApplicationPermission("administration.roles.add");

    /// <summary>
    /// Permission to edit roles.
    /// </summary>
    public static readonly ApplicationPermission EditRoles = new ApplicationPermission("administration.roles.edit");

    /// <summary>
    /// Permission to activate roles.
    /// </summary>
    public static readonly ApplicationPermission ActivateRoles = new ApplicationPermission("administration.roles.activate");

    /// <summary>
    /// Permission to delete roles.
    /// </summary>
    public static readonly ApplicationPermission DeleteRoles = new ApplicationPermission("administration.roles.delete");

    // Administration - Permission Matrix Permissions
    /// <summary>
    /// Permission to view permission matrix.
    /// </summary>
    public static readonly ApplicationPermission ViewPermissionMatrix = new ApplicationPermission("administration.permissionmatrix.view");

    /// <summary>
    /// Permission to manage permission matrix.
    /// </summary>
    public static readonly ApplicationPermission ManagePermissionMatrix = new ApplicationPermission("administration.permissionmatrix.manage");

    // Administration - Financial Years Permissions
    /// <summary>
    /// Permission to view financial years.
    /// </summary>
    public static readonly ApplicationPermission ViewFinancialYears = new ApplicationPermission("administration.financialyears.view");

    /// <summary>
    /// Permission to add financial years.
    /// </summary>
    public static readonly ApplicationPermission AddFinancialYears = new ApplicationPermission("administration.financialyears.add");

    /// <summary>
    /// Permission to edit financial years.
    /// </summary>
    public static readonly ApplicationPermission EditFinancialYears = new ApplicationPermission("administration.financialyears.edit");

    /// <summary>
    /// Permission to activate financial years.
    /// </summary>
    public static readonly ApplicationPermission ActivateFinancialYears = new ApplicationPermission("administration.financialyears.activate");

    /// <summary>
    /// Permission to close financial years.
    /// </summary>
    public static readonly ApplicationPermission CloseFinancialYears = new ApplicationPermission("administration.financialyears.close");

    // Masters - Companies Permissions
    /// <summary>
    /// Permission to view companies.
    /// </summary>
    public static readonly ApplicationPermission ViewCompanies = new ApplicationPermission("masters.companies.view");

    /// <summary>
    /// Permission to add companies.
    /// </summary>
    public static readonly ApplicationPermission AddCompanies = new ApplicationPermission("masters.companies.add");

    /// <summary>
    /// Permission to edit companies.
    /// </summary>
    public static readonly ApplicationPermission EditCompanies = new ApplicationPermission("masters.companies.edit");

    /// <summary>
    /// Permission to delete companies.
    /// </summary>
    public static readonly ApplicationPermission DeleteCompanies = new ApplicationPermission("masters.companies.delete");

    // Masters - Customers Permissions
    /// <summary>
    /// Permission to view customers.
    /// </summary>
    public static readonly ApplicationPermission ViewCustomers = new ApplicationPermission("masters.customers.view");

    /// <summary>
    /// Permission to add customers.
    /// </summary>
    public static readonly ApplicationPermission AddCustomers = new ApplicationPermission("masters.customers.add");

    /// <summary>
    /// Permission to edit customers.
    /// </summary>
    public static readonly ApplicationPermission EditCustomers = new ApplicationPermission("masters.customers.edit");

    /// <summary>
    /// Permission to delete customers.
    /// </summary>
    public static readonly ApplicationPermission DeleteCustomers = new ApplicationPermission("masters.customers.delete");

    // Masters - Vendors Permissions
    /// <summary>
    /// Permission to view vendors.
    /// </summary>
    public static readonly ApplicationPermission ViewVendors = new ApplicationPermission("masters.vendors.view");

    /// <summary>
    /// Permission to add vendors.
    /// </summary>
    public static readonly ApplicationPermission AddVendors = new ApplicationPermission("masters.vendors.add");

    /// <summary>
    /// Permission to edit vendors.
    /// </summary>
    public static readonly ApplicationPermission EditVendors = new ApplicationPermission("masters.vendors.edit");

    /// <summary>
    /// Permission to delete vendors.
    /// </summary>
    public static readonly ApplicationPermission DeleteVendors = new ApplicationPermission("masters.vendors.delete");

    // Masters - Source/Destinations Permissions
    /// <summary>
    /// Permission to view source/destinations.
    /// </summary>
    public static readonly ApplicationPermission ViewSourceDestinations = new ApplicationPermission("masters.sourcedestinations.view");

    /// <summary>
    /// Permission to add source/destinations.
    /// </summary>
    public static readonly ApplicationPermission AddSourceDestinations = new ApplicationPermission("masters.sourcedestinations.add");

    /// <summary>
    /// Permission to edit source/destinations.
    /// </summary>
    public static readonly ApplicationPermission EditSourceDestinations = new ApplicationPermission("masters.sourcedestinations.edit");

    /// <summary>
    /// Permission to delete source/destinations.
    /// </summary>
    public static readonly ApplicationPermission DeleteSourceDestinations = new ApplicationPermission("masters.sourcedestinations.delete");

    // Masters - Materials Permissions
    /// <summary>
    /// Permission to view materials.
    /// </summary>
    public static readonly ApplicationPermission ViewMaterials = new ApplicationPermission("masters.materials.view");

    /// <summary>
    /// Permission to add materials.
    /// </summary>
    public static readonly ApplicationPermission AddMaterials = new ApplicationPermission("masters.materials.add");

    /// <summary>
    /// Permission to edit materials.
    /// </summary>
    public static readonly ApplicationPermission EditMaterials = new ApplicationPermission("masters.materials.edit");

    /// <summary>
    /// Permission to delete materials.
    /// </summary>
    public static readonly ApplicationPermission DeleteMaterials = new ApplicationPermission("masters.materials.delete");

    // Masters - Fuel Pumps Permissions
    /// <summary>
    /// Permission to view fuel pumps.
    /// </summary>
    public static readonly ApplicationPermission ViewFuelPumps = new ApplicationPermission("masters.fuelpumps.view");

    /// <summary>
    /// Permission to add fuel pumps.
    /// </summary>
    public static readonly ApplicationPermission AddFuelPumps = new ApplicationPermission("masters.fuelpumps.add");

    /// <summary>
    /// Permission to edit fuel pumps.
    /// </summary>
    public static readonly ApplicationPermission EditFuelPumps = new ApplicationPermission("masters.fuelpumps.edit");

    /// <summary>
    /// Permission to delete fuel pumps.
    /// </summary>
    public static readonly ApplicationPermission DeleteFuelPumps = new ApplicationPermission("masters.fuelpumps.delete");

    // Masters - HSD Rates Permissions
    /// <summary>
    /// Permission to view HSD rates.
    /// </summary>
    public static readonly ApplicationPermission ViewHsdRates = new ApplicationPermission("masters.hsdrates.view");

    /// <summary>
    /// Permission to add HSD rates.
    /// </summary>
    public static readonly ApplicationPermission AddHsdRates = new ApplicationPermission("masters.hsdrates.add");

    /// <summary>
    /// Permission to edit HSD rates.
    /// </summary>
    public static readonly ApplicationPermission EditHsdRates = new ApplicationPermission("masters.hsdrates.edit");

    /// <summary>
    /// Permission to delete HSD rates.
    /// </summary>
    public static readonly ApplicationPermission DeleteHsdRates = new ApplicationPermission("masters.hsdrates.delete");

    // Masters - Payment Locations Permissions
    /// <summary>
    /// Permission to view payment locations.
    /// </summary>
    public static readonly ApplicationPermission ViewPaymentLocations = new ApplicationPermission("masters.paymentlocations.view");

    /// <summary>
    /// Permission to add payment locations.
    /// </summary>
    public static readonly ApplicationPermission AddPaymentLocations = new ApplicationPermission("masters.paymentlocations.add");

    /// <summary>
    /// Permission to edit payment locations.
    /// </summary>
    public static readonly ApplicationPermission EditPaymentLocations = new ApplicationPermission("masters.paymentlocations.edit");

    /// <summary>
    /// Permission to delete payment locations.
    /// </summary>
    public static readonly ApplicationPermission DeletePaymentLocations = new ApplicationPermission("masters.paymentlocations.delete");

    // Masters - Vehicle Owners Permissions
    /// <summary>
    /// Permission to view vehicle owners.
    /// </summary>
    public static readonly ApplicationPermission ViewVehicleOwners = new ApplicationPermission("masters.vehicleowners.view");

    /// <summary>
    /// Permission to add vehicle owners.
    /// </summary>
    public static readonly ApplicationPermission AddVehicleOwners = new ApplicationPermission("masters.vehicleowners.add");

    /// <summary>
    /// Permission to edit vehicle owners.
    /// </summary>
    public static readonly ApplicationPermission EditVehicleOwners = new ApplicationPermission("masters.vehicleowners.edit");

    /// <summary>
    /// Permission to delete vehicle owners.
    /// </summary>
    public static readonly ApplicationPermission DeleteVehicleOwners = new ApplicationPermission("masters.vehicleowners.delete");

    // Masters - Vehicle Assignments Permissions
    /// <summary>
    /// Permission to view vehicle assignments.
    /// </summary>
    public static readonly ApplicationPermission ViewVehicleAssignments = new ApplicationPermission("masters.vehicleassignments.view");

    /// <summary>
    /// Permission to add vehicle assignments.
    /// </summary>
    public static readonly ApplicationPermission AddVehicleAssignments = new ApplicationPermission("masters.vehicleassignments.add");

    /// <summary>
    /// Permission to edit vehicle assignments.
    /// </summary>
    public static readonly ApplicationPermission EditVehicleAssignments = new ApplicationPermission("masters.vehicleassignments.edit");

    /// <summary>
    /// Permission to delete vehicle assignments.
    /// </summary>
    public static readonly ApplicationPermission DeleteVehicleAssignments = new ApplicationPermission("masters.vehicleassignments.delete");

    /// <summary>
    /// Gets all available application permissions.
    /// </summary>
    /// <returns>A collection of all application permissions.</returns>
    public static IEnumerable<ApplicationPermission> GetAllPermissions()
    {
        var allPermissions = typeof(ApplicationPermission)
            .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
            .Where(f => f.FieldType == typeof(ApplicationPermission))
            .Select(f => (ApplicationPermission)f.GetValue(null)!)
            .ToList();

        // Debug logging
        System.Diagnostics.Debug.WriteLine($"GetAllPermissions() returned {allPermissions.Count} permissions");
        System.Diagnostics.Debug.WriteLine($"All permission IDs: {string.Join(", ", allPermissions.Select(p => p.Id))}");

        return allPermissions;
    }
}
