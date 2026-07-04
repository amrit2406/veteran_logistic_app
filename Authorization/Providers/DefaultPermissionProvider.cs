using veteran_logistic.Authorization.Contracts;
using veteran_logistic.Authorization.Models;

namespace veteran_logistic.Authorization.Providers;

/// <summary>
/// Default implementation of IPermissionProvider that maps roles to permissions.
/// </summary>
public sealed class DefaultPermissionProvider : IPermissionProvider
{
    /// <inheritdoc />
    public IEnumerable<ApplicationPermission> GetPermissions(ApplicationRole role)
    {
        return role switch
        {
            ApplicationRole.Administrator => GetAdministratorPermissions(),
            ApplicationRole.Manager => GetManagerPermissions(),
            ApplicationRole.User => GetUserPermissions(),
            ApplicationRole.Viewer => GetViewerPermissions(),
            _ => Enumerable.Empty<ApplicationPermission>()
        };
    }

    private static IEnumerable<ApplicationPermission> GetAdministratorPermissions()
    {
        // Administrators have all permissions
        return new[]
        {
            ApplicationPermission.ViewDrivers,
            ApplicationPermission.CreateDrivers,
            ApplicationPermission.EditDrivers,
            ApplicationPermission.DeleteDrivers,
            ApplicationPermission.ViewVehicles,
            ApplicationPermission.CreateVehicles,
            ApplicationPermission.EditVehicles,
            ApplicationPermission.DeleteVehicles,
            ApplicationPermission.ViewTrips,
            ApplicationPermission.CreateTrips,
            ApplicationPermission.EditTrips,
            ApplicationPermission.DeleteTrips,
            ApplicationPermission.ViewDispatches,
            ApplicationPermission.CreateDispatches,
            ApplicationPermission.EditDispatches,
            ApplicationPermission.DeleteDispatches
        };
    }

    private static IEnumerable<ApplicationPermission> GetManagerPermissions()
    {
        // Managers can view and create/edit most resources, but cannot delete
        return new[]
        {
            ApplicationPermission.ViewDrivers,
            ApplicationPermission.CreateDrivers,
            ApplicationPermission.EditDrivers,
            ApplicationPermission.ViewVehicles,
            ApplicationPermission.CreateVehicles,
            ApplicationPermission.EditVehicles,
            ApplicationPermission.ViewTrips,
            ApplicationPermission.CreateTrips,
            ApplicationPermission.EditTrips,
            ApplicationPermission.ViewDispatches,
            ApplicationPermission.CreateDispatches,
            ApplicationPermission.EditDispatches
        };
    }

    private static IEnumerable<ApplicationPermission> GetUserPermissions()
    {
        // Users can view and create resources, but cannot edit or delete
        return new[]
        {
            ApplicationPermission.ViewDrivers,
            ApplicationPermission.CreateDrivers,
            ApplicationPermission.ViewVehicles,
            ApplicationPermission.CreateVehicles,
            ApplicationPermission.ViewTrips,
            ApplicationPermission.CreateTrips,
            ApplicationPermission.ViewDispatches,
            ApplicationPermission.CreateDispatches
        };
    }

    private static IEnumerable<ApplicationPermission> GetViewerPermissions()
    {
        // Viewers can only view resources
        return new[]
        {
            ApplicationPermission.ViewDrivers,
            ApplicationPermission.ViewVehicles,
            ApplicationPermission.ViewTrips,
            ApplicationPermission.ViewDispatches
        };
    }
}
