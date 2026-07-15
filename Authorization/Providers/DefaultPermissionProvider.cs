using Microsoft.Extensions.Logging;
using veteran_logistic.Authorization.Contracts;
using veteran_logistic.Authorization.Models;

namespace veteran_logistic.Authorization.Providers;

/// <summary>
/// Default implementation of IPermissionProvider that maps roles to permissions.
/// </summary>
public sealed class DefaultPermissionProvider : IPermissionProvider
{
    private readonly ILogger<DefaultPermissionProvider> _logger;

    public DefaultPermissionProvider(ILogger<DefaultPermissionProvider> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public IEnumerable<ApplicationPermission> GetPermissions(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            return Enumerable.Empty<ApplicationPermission>();
        }

        _logger.LogInformation("Getting permissions for role: {RoleName}", roleName);

        // Try to parse the role name to ApplicationRole enum for backward compatibility
        if (Enum.TryParse<ApplicationRole>(roleName, out var role))
        {
            var permissions = role switch
            {
                ApplicationRole.Administrator => GetAdministratorPermissions(),
                ApplicationRole.Manager => GetManagerPermissions(),
                ApplicationRole.User => GetUserPermissions(),
                ApplicationRole.Viewer => GetViewerPermissions(),
                _ => Enumerable.Empty<ApplicationPermission>()
            };

            _logger.LogInformation("Role {RoleName} resolved to {Role}, returning {PermissionCount} permissions",
                roleName, role, permissions.Count());

            return permissions;
        }

        // If role name doesn't match enum, return no permissions
        _logger.LogWarning("Role name '{RoleName}' does not match any ApplicationRole enum value", roleName);
        return Enumerable.Empty<ApplicationPermission>();
    }

    private IEnumerable<ApplicationPermission> GetAdministratorPermissions()
    {
        // Administrators have all permissions
        // return new[]
        // {
        //     ApplicationPermission.ViewDrivers,
        //     ApplicationPermission.CreateDrivers,
        //     ApplicationPermission.EditDrivers,
        //     ApplicationPermission.DeleteDrivers,
        //     ApplicationPermission.ViewVehicles,
        //     ApplicationPermission.CreateVehicles,
        //     ApplicationPermission.EditVehicles,
        //     ApplicationPermission.DeleteVehicles,
        //     ApplicationPermission.ViewTrips,
        //     ApplicationPermission.CreateTrips,
        //     ApplicationPermission.EditTrips,
        //     ApplicationPermission.DeleteTrips,
        //     ApplicationPermission.ViewDispatches,
        //     ApplicationPermission.CreateDispatches,
        //     ApplicationPermission.EditDispatches,
        //     ApplicationPermission.DeleteDispatches
        // };
        // Administrators have all permissions - automatically includes any new permissions added
        var allPermissions = ApplicationPermission.GetAllPermissions().ToList();
        _logger.LogWarning("GetAdministratorPermissions() returned {PermissionCount} permissions", allPermissions.Count);
        _logger.LogWarning("GetAdministratorPermissions() permissions: {Permissions}",
            string.Join(", ", allPermissions.Select(p => p.Id)));
        return allPermissions;
    }

    private static IEnumerable<ApplicationPermission> GetManagerPermissions()
    {
        // Managers can view and create/edit most resources, but cannot delete
        return new[]
        {
            // Driver Management
            ApplicationPermission.ViewDrivers,
            ApplicationPermission.CreateDrivers,
            ApplicationPermission.EditDrivers,
            // Vehicle Management
            ApplicationPermission.ViewVehicles,
            ApplicationPermission.CreateVehicles,
            ApplicationPermission.EditVehicles,
            // Trip Management
            ApplicationPermission.ViewTrips,
            ApplicationPermission.CreateTrips,
            ApplicationPermission.EditTrips,
            // Dispatch Management
            ApplicationPermission.ViewDispatches,
            ApplicationPermission.CreateDispatches,
            ApplicationPermission.EditDispatches,
            // Masters - Companies
            ApplicationPermission.ViewCompanies,
            ApplicationPermission.AddCompanies,
            ApplicationPermission.EditCompanies,
            // Masters - Customers
            ApplicationPermission.ViewCustomers,
            ApplicationPermission.AddCustomers,
            ApplicationPermission.EditCustomers,
            // Masters - Vendors
            ApplicationPermission.ViewVendors,
            ApplicationPermission.AddVendors,
            ApplicationPermission.EditVendors,
            // Masters - Sources
            ApplicationPermission.ViewSources,
            ApplicationPermission.AddSources,
            ApplicationPermission.EditSources,
            // Masters - Destinations
            ApplicationPermission.ViewDestinations,
            ApplicationPermission.AddDestinations,
            ApplicationPermission.EditDestinations,
            // Masters - Materials
            ApplicationPermission.ViewMaterials,
            ApplicationPermission.AddMaterials,
            ApplicationPermission.EditMaterials,
            // Masters - Fuel Pumps
            ApplicationPermission.ViewFuelPumps,
            ApplicationPermission.AddFuelPumps,
            ApplicationPermission.EditFuelPumps,
            // Masters - HSD Rates
            ApplicationPermission.ViewHsdRates,
            ApplicationPermission.AddHsdRates,
            ApplicationPermission.EditHsdRates,
            // Masters - Payment Locations
            ApplicationPermission.ViewPaymentLocations,
            ApplicationPermission.AddPaymentLocations,
            ApplicationPermission.EditPaymentLocations,
            // Masters - Vehicle Owners
            ApplicationPermission.ViewVehicleOwners,
            ApplicationPermission.AddVehicleOwners,
            ApplicationPermission.EditVehicleOwners,
            // Masters - Vehicles
            ApplicationPermission.ViewVehicles,
            ApplicationPermission.CreateVehicles,
            ApplicationPermission.EditVehicles
        };
    }

    private static IEnumerable<ApplicationPermission> GetUserPermissions()
    {
        // Users can view and create resources, but cannot edit or delete
        return new[]
        {
            // Driver Management
            ApplicationPermission.ViewDrivers,
            ApplicationPermission.CreateDrivers,
            // Vehicle Management
            ApplicationPermission.ViewVehicles,
            ApplicationPermission.CreateVehicles,
            // Trip Management
            ApplicationPermission.ViewTrips,
            ApplicationPermission.CreateTrips,
            // Dispatch Management
            ApplicationPermission.ViewDispatches,
            ApplicationPermission.CreateDispatches,
            // Masters - Companies
            ApplicationPermission.ViewCompanies,
            ApplicationPermission.AddCompanies,
            // Masters - Customers
            ApplicationPermission.ViewCustomers,
            ApplicationPermission.AddCustomers,
            // Masters - Vendors
            ApplicationPermission.ViewVendors,
            ApplicationPermission.AddVendors,
            // Masters - Sources
            ApplicationPermission.ViewSources,
            ApplicationPermission.AddSources,
            // Masters - Destinations
            ApplicationPermission.ViewDestinations,
            ApplicationPermission.AddDestinations,
            // Masters - Materials
            ApplicationPermission.ViewMaterials,
            ApplicationPermission.AddMaterials,
            // Masters - Fuel Pumps
            ApplicationPermission.ViewFuelPumps,
            ApplicationPermission.AddFuelPumps,
            // Masters - HSD Rates
            ApplicationPermission.ViewHsdRates,
            ApplicationPermission.AddHsdRates,
            // Masters - Payment Locations
            ApplicationPermission.ViewPaymentLocations,
            ApplicationPermission.AddPaymentLocations,
            // Masters - Vehicle Owners
            ApplicationPermission.ViewVehicleOwners,
            ApplicationPermission.AddVehicleOwners,
            // Masters - Vehicles
            ApplicationPermission.ViewVehicles,
            ApplicationPermission.CreateVehicles
        };
    }

    private static IEnumerable<ApplicationPermission> GetViewerPermissions()
    {
        // Viewers can only view resources
        return new[]
        {
            // Driver Management
            ApplicationPermission.ViewDrivers,
            // Vehicle Management
            ApplicationPermission.ViewVehicles,
            // Trip Management
            ApplicationPermission.ViewTrips,
            // Dispatch Management
            ApplicationPermission.ViewDispatches,
            // Masters - Companies
            ApplicationPermission.ViewCompanies,
            // Masters - Customers
            ApplicationPermission.ViewCustomers,
            // Masters - Vendors
            ApplicationPermission.ViewVendors,
            // Masters - Sources
            ApplicationPermission.ViewSources,
            // Masters - Destinations
            ApplicationPermission.ViewDestinations,
            // Masters - Materials
            ApplicationPermission.ViewMaterials,
            // Masters - Fuel Pumps
            ApplicationPermission.ViewFuelPumps,
            // Masters - HSD Rates
            ApplicationPermission.ViewHsdRates,
            // Masters - Payment Locations
            ApplicationPermission.ViewPaymentLocations,
            // Masters - Vehicle Owners
            ApplicationPermission.ViewVehicleOwners,
            // Masters - Vehicles
            ApplicationPermission.ViewVehicles
        };
    }
}
