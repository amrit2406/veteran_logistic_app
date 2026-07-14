using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using veteran_logistic.Authorization.Contracts;
using veteran_logistic.Authorization.Models;
using VeteranLogistics.Data.Context;
using VeteranLogistics.Data.Entities.Administration;

namespace veteran_logistic.Authorization.Providers;

/// <summary>
/// Database-based implementation of IPermissionProvider that reads permissions from the RolePermission table.
/// This allows dynamic permission assignment through the Permission Matrix UI.
/// </summary>
public sealed class DatabasePermissionProvider : IPermissionProvider
{
    private readonly VeteranLogisticsDbContext _dbContext;
    private readonly ILogger<DatabasePermissionProvider> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabasePermissionProvider"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="logger">The logger.</param>
    public DatabasePermissionProvider(VeteranLogisticsDbContext dbContext, ILogger<DatabasePermissionProvider> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public IEnumerable<ApplicationPermission> GetPermissions(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            _logger.LogDebug("No role name provided. Returning no permissions.");
            return Enumerable.Empty<ApplicationPermission>();
        }

        // Special case: Administrator role gets all permissions
        if (string.Equals(roleName, "Administrator", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogDebug("Role '{RoleName}' is Administrator. Granting all permissions.", roleName);
            return GetAllPermissions();
        }

        try
        {
            // Get the role by name
            var role = _dbContext.Roles
                .AsNoTracking()
                .FirstOrDefault(r => r.Name == roleName && r.IsActive);

            if (role == null)
            {
                _logger.LogWarning("Role '{RoleName}' not found or inactive. Returning no permissions.", roleName);
                return Enumerable.Empty<ApplicationPermission>();
            }

            // Get the role's permissions from the database
            var rolePermissions = _dbContext.RolePermissions
                .AsNoTracking()
                .Include(rp => rp.Permission)
                .Where(rp => rp.RoleId == role.Id && rp.IsGranted && rp.Permission != null && rp.Permission.IsActive)
                .ToList();

            if (!rolePermissions.Any())
            {
                _logger.LogDebug("Role '{RoleName}' has no granted permissions. Returning no permissions.", roleName);
                return Enumerable.Empty<ApplicationPermission>();
            }

            // Map permission keys to ApplicationPermission enum values
            var permissions = new List<ApplicationPermission>();
            foreach (var rolePermission in rolePermissions)
            {
                var permission = MapPermissionKeyToApplicationPermission(rolePermission.Permission!.PermissionKey);
                if (permission != null)
                {
                    permissions.Add(permission);
                }
            }

            _logger.LogDebug("Role '{RoleName}' has {PermissionCount} permissions from database.", roleName, permissions.Count);
            return permissions;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading permissions for role '{RoleName}'. Returning no permissions.", roleName);
            return Enumerable.Empty<ApplicationPermission>();
        }
    }

    /// <summary>
    /// Gets all available permissions in the system.
    /// </summary>
    private static IEnumerable<ApplicationPermission> GetAllPermissions()
    {
        return new[]
        {
            // Administration - Users
            ApplicationPermission.ViewUsers,
            ApplicationPermission.AddUsers,
            ApplicationPermission.EditUsers,
            ApplicationPermission.ActivateUsers,
            ApplicationPermission.DeleteUsers,
            
            // Administration - Roles
            ApplicationPermission.ViewRoles,
            ApplicationPermission.AddRoles,
            ApplicationPermission.EditRoles,
            ApplicationPermission.ActivateRoles,
            ApplicationPermission.DeleteRoles,
            
            // Administration - Permission Matrix
            ApplicationPermission.ViewPermissionMatrix,
            ApplicationPermission.ManagePermissionMatrix,
            
            // Administration - Financial Years
            ApplicationPermission.ViewFinancialYears,
            ApplicationPermission.AddFinancialYears,
            ApplicationPermission.EditFinancialYears,
            ApplicationPermission.ActivateFinancialYears,
            ApplicationPermission.CloseFinancialYears,
            
            // Masters - Companies
            ApplicationPermission.ViewCompanies,
            ApplicationPermission.AddCompanies,
            ApplicationPermission.EditCompanies,
            ApplicationPermission.DeleteCompanies,
            
            // Masters - Customers
            ApplicationPermission.ViewCustomers,
            ApplicationPermission.AddCustomers,
            ApplicationPermission.EditCustomers,
            ApplicationPermission.DeleteCustomers,
            
            // Masters - Vendors
            ApplicationPermission.ViewVendors,
            ApplicationPermission.AddVendors,
            ApplicationPermission.EditVendors,
            ApplicationPermission.DeleteVendors,
            
            // Masters - Sources
            ApplicationPermission.ViewSources,
            ApplicationPermission.AddSources,
            ApplicationPermission.EditSources,
            ApplicationPermission.DeleteSources,
            
            // Masters - Destinations
            ApplicationPermission.ViewDestinations,
            ApplicationPermission.AddDestinations,
            ApplicationPermission.EditDestinations,
            ApplicationPermission.DeleteDestinations,
            
            // Masters - Materials
            ApplicationPermission.ViewMaterials,
            ApplicationPermission.AddMaterials,
            ApplicationPermission.EditMaterials,
            ApplicationPermission.DeleteMaterials,
            
            // Masters - Fuel Pumps
            ApplicationPermission.ViewFuelPumps,
            ApplicationPermission.AddFuelPumps,
            ApplicationPermission.EditFuelPumps,
            ApplicationPermission.DeleteFuelPumps,
            
            // Masters - HSD Rates
            ApplicationPermission.ViewHsdRates,
            ApplicationPermission.AddHsdRates,
            ApplicationPermission.EditHsdRates,
            ApplicationPermission.DeleteHsdRates,
            
            // Masters - Payment Locations
            ApplicationPermission.ViewPaymentLocations,
            ApplicationPermission.AddPaymentLocations,
            ApplicationPermission.EditPaymentLocations,
            ApplicationPermission.DeletePaymentLocations,
            
            // Masters - Vehicle Owners
            ApplicationPermission.ViewVehicleOwners,
            ApplicationPermission.AddVehicleOwners,
            ApplicationPermission.EditVehicleOwners,
            ApplicationPermission.DeleteVehicleOwners,
            
            // Drivers
            ApplicationPermission.ViewDrivers,
            ApplicationPermission.CreateDrivers,
            ApplicationPermission.EditDrivers,
            ApplicationPermission.DeleteDrivers,
            
            // Vehicles
            ApplicationPermission.ViewVehicles,
            ApplicationPermission.CreateVehicles,
            ApplicationPermission.EditVehicles,
            ApplicationPermission.DeleteVehicles,
            
            // Trips
            ApplicationPermission.ViewTrips,
            ApplicationPermission.CreateTrips,
            ApplicationPermission.EditTrips,
            ApplicationPermission.DeleteTrips,
            
            // Dispatches
            ApplicationPermission.ViewDispatches,
            ApplicationPermission.CreateDispatches,
            ApplicationPermission.EditDispatches,
            ApplicationPermission.DeleteDispatches
        };
    }

    /// <summary>
    /// Maps a permission key from the database to an ApplicationPermission enum value.
    /// </summary>
    /// <param name="permissionKey">The permission key from the database.</param>
    /// <returns>The corresponding ApplicationPermission, or null if no match.</returns>
    private static ApplicationPermission? MapPermissionKeyToApplicationPermission(string permissionKey)
    {
        return permissionKey switch
        {
            // Administration - Users
            "Administration.Users.View" => ApplicationPermission.ViewUsers,
            "Administration.Users.Add" => ApplicationPermission.AddUsers,
            "Administration.Users.Edit" => ApplicationPermission.EditUsers,
            "Administration.Users.Activate" => ApplicationPermission.ActivateUsers,
            "Administration.Users.Delete" => ApplicationPermission.DeleteUsers,
            
            // Administration - Roles
            "Administration.Roles.View" => ApplicationPermission.ViewRoles,
            "Administration.Roles.Add" => ApplicationPermission.AddRoles,
            "Administration.Roles.Edit" => ApplicationPermission.EditRoles,
            "Administration.Roles.Activate" => ApplicationPermission.ActivateRoles,
            "Administration.Roles.Delete" => ApplicationPermission.DeleteRoles,
            
            // Administration - Permission Matrix
            "Administration.PermissionMatrix.View" => ApplicationPermission.ViewPermissionMatrix,
            "Administration.PermissionMatrix.Manage" => ApplicationPermission.ManagePermissionMatrix,
            
            // Administration - Financial Year
            "Administration.FinancialYear.View" => ApplicationPermission.ViewFinancialYears,
            "Administration.FinancialYear.Add" => ApplicationPermission.AddFinancialYears,
            "Administration.FinancialYear.Edit" => ApplicationPermission.EditFinancialYears,
            "Administration.FinancialYear.Activate" => ApplicationPermission.ActivateFinancialYears,
            "Administration.FinancialYear.Close" => ApplicationPermission.CloseFinancialYears,
            
            // Masters - Companies
            "Masters.Companies.View" => ApplicationPermission.ViewCompanies,
            "Masters.Companies.Add" => ApplicationPermission.AddCompanies,
            "Masters.Companies.Edit" => ApplicationPermission.EditCompanies,
            "Masters.Companies.Delete" => ApplicationPermission.DeleteCompanies,
            
            // Masters - Customers
            "Masters.Customers.View" => ApplicationPermission.ViewCustomers,
            "Masters.Customers.Add" => ApplicationPermission.AddCustomers,
            "Masters.Customers.Edit" => ApplicationPermission.EditCustomers,
            "Masters.Customers.Delete" => ApplicationPermission.DeleteCustomers,
            
            // Masters - Vendors
            "Masters.Vendors.View" => ApplicationPermission.ViewVendors,
            "Masters.Vendors.Add" => ApplicationPermission.AddVendors,
            "Masters.Vendors.Edit" => ApplicationPermission.EditVendors,
            "Masters.Vendors.Delete" => ApplicationPermission.DeleteVendors,
            
            // Masters - Sources
            "Masters.Sources.View" => ApplicationPermission.ViewSources,
            "Masters.Sources.Add" => ApplicationPermission.AddSources,
            "Masters.Sources.Edit" => ApplicationPermission.EditSources,
            "Masters.Sources.Delete" => ApplicationPermission.DeleteSources,
            
            // Masters - Destinations
            "Masters.Destinations.View" => ApplicationPermission.ViewDestinations,
            "Masters.Destinations.Add" => ApplicationPermission.AddDestinations,
            "Masters.Destinations.Edit" => ApplicationPermission.EditDestinations,
            "Masters.Destinations.Delete" => ApplicationPermission.DeleteDestinations,
            
            // Masters - Materials
            "Masters.Materials.View" => ApplicationPermission.ViewMaterials,
            "Masters.Materials.Add" => ApplicationPermission.AddMaterials,
            "Masters.Materials.Edit" => ApplicationPermission.EditMaterials,
            "Masters.Materials.Delete" => ApplicationPermission.DeleteMaterials,
            
            // Masters - Fuel Pumps
            "Masters.FuelPumps.View" => ApplicationPermission.ViewFuelPumps,
            "Masters.FuelPumps.Add" => ApplicationPermission.AddFuelPumps,
            "Masters.FuelPumps.Edit" => ApplicationPermission.EditFuelPumps,
            "Masters.FuelPumps.Delete" => ApplicationPermission.DeleteFuelPumps,
            
            // Masters - HSD Rates
            "Masters.HsdRates.View" => ApplicationPermission.ViewHsdRates,
            "Masters.HsdRates.Add" => ApplicationPermission.AddHsdRates,
            "Masters.HsdRates.Edit" => ApplicationPermission.EditHsdRates,
            "Masters.HsdRates.Delete" => ApplicationPermission.DeleteHsdRates,
            
            // Masters - Payment Locations
            "Masters.PaymentLocations.View" => ApplicationPermission.ViewPaymentLocations,
            "Masters.PaymentLocations.Add" => ApplicationPermission.AddPaymentLocations,
            "Masters.PaymentLocations.Edit" => ApplicationPermission.EditPaymentLocations,
            "Masters.PaymentLocations.Delete" => ApplicationPermission.DeletePaymentLocations,
            
            // Masters - Vehicle Owners
            "Masters.VehicleOwners.View" => ApplicationPermission.ViewVehicleOwners,
            "Masters.VehicleOwners.Add" => ApplicationPermission.AddVehicleOwners,
            "Masters.VehicleOwners.Edit" => ApplicationPermission.EditVehicleOwners,
            "Masters.VehicleOwners.Delete" => ApplicationPermission.DeleteVehicleOwners,
            
            // For now, return null for unmapped permissions
            _ => null
        };
    }
}
