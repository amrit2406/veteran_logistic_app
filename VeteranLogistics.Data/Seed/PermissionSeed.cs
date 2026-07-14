using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VeteranLogistics.Data.Context;
using VeteranLogistics.Data.Entities.Administration;

namespace VeteranLogistics.Data.Seed;

/// <summary>
/// Seeds the Permission table with administration and masters module permissions.
/// </summary>
public static class PermissionSeed
{
    private static readonly List<Permission> Permissions = new()
    {
        // Administration - Users screen
        new Permission { Module = "Administration", Screen = "Users", PermissionKey = "Administration.Users.View", DisplayName = "View Users", Description = "View list of users", SortOrder = 1, IsActive = true },
        new Permission { Module = "Administration", Screen = "Users", PermissionKey = "Administration.Users.Add", DisplayName = "Add User", Description = "Create a new user", SortOrder = 2, IsActive = true },
        new Permission { Module = "Administration", Screen = "Users", PermissionKey = "Administration.Users.Edit", DisplayName = "Edit User", Description = "Edit existing user", SortOrder = 3, IsActive = true },
        new Permission { Module = "Administration", Screen = "Users", PermissionKey = "Administration.Users.Activate", DisplayName = "Activate User", Description = "Activate a user account", SortOrder = 4, IsActive = true },
        new Permission { Module = "Administration", Screen = "Users", PermissionKey = "Administration.Users.Delete", DisplayName = "Delete User", Description = "Delete a user account", SortOrder = 5, IsActive = true },
        
        // Administration - Roles screen
        new Permission { Module = "Administration", Screen = "Roles", PermissionKey = "Administration.Roles.View", DisplayName = "View Roles", Description = "View list of roles", SortOrder = 10, IsActive = true },
        new Permission { Module = "Administration", Screen = "Roles", PermissionKey = "Administration.Roles.Add", DisplayName = "Add Role", Description = "Create a new role", SortOrder = 11, IsActive = true },
        new Permission { Module = "Administration", Screen = "Roles", PermissionKey = "Administration.Roles.Edit", DisplayName = "Edit Role", Description = "Edit existing role", SortOrder = 12, IsActive = true },
        new Permission { Module = "Administration", Screen = "Roles", PermissionKey = "Administration.Roles.Activate", DisplayName = "Activate Role", Description = "Activate a role", SortOrder = 13, IsActive = true },
        new Permission { Module = "Administration", Screen = "Roles", PermissionKey = "Administration.Roles.Delete", DisplayName = "Delete Role", Description = "Delete a role", SortOrder = 14, IsActive = true },
        
        // Administration - Permission Matrix screen
        new Permission { Module = "Administration", Screen = "PermissionMatrix", PermissionKey = "Administration.PermissionMatrix.View", DisplayName = "View Permission Matrix", Description = "View permission matrix", SortOrder = 20, IsActive = true },
        new Permission { Module = "Administration", Screen = "PermissionMatrix", PermissionKey = "Administration.PermissionMatrix.Manage", DisplayName = "Manage Permission Matrix", Description = "Edit permission assignments", SortOrder = 21, IsActive = true },
        
        // Administration - Financial Year screen
        new Permission { Module = "Administration", Screen = "FinancialYear", PermissionKey = "Administration.FinancialYear.View", DisplayName = "View Financial Years", Description = "View financial year list", SortOrder = 30, IsActive = true },
        new Permission { Module = "Administration", Screen = "FinancialYear", PermissionKey = "Administration.FinancialYear.Add", DisplayName = "Add Financial Year", Description = "Create a new financial year", SortOrder = 31, IsActive = true },
        new Permission { Module = "Administration", Screen = "FinancialYear", PermissionKey = "Administration.FinancialYear.Edit", DisplayName = "Edit Financial Year", Description = "Edit existing financial year", SortOrder = 32, IsActive = true },
        new Permission { Module = "Administration", Screen = "FinancialYear", PermissionKey = "Administration.FinancialYear.Activate", DisplayName = "Activate Financial Year", Description = "Activate a financial year", SortOrder = 33, IsActive = true },
        new Permission { Module = "Administration", Screen = "FinancialYear", PermissionKey = "Administration.FinancialYear.Close", DisplayName = "Close Financial Year", Description = "Close a financial year", SortOrder = 34, IsActive = true },
        
        // Masters - Companies screen
        new Permission { Module = "Masters", Screen = "Companies", PermissionKey = "Masters.Companies.View", DisplayName = "View Companies", Description = "View list of companies", SortOrder = 100, IsActive = true },
        new Permission { Module = "Masters", Screen = "Companies", PermissionKey = "Masters.Companies.Add", DisplayName = "Add Company", Description = "Create a new company", SortOrder = 101, IsActive = true },
        new Permission { Module = "Masters", Screen = "Companies", PermissionKey = "Masters.Companies.Edit", DisplayName = "Edit Company", Description = "Edit existing company", SortOrder = 102, IsActive = true },
        new Permission { Module = "Masters", Screen = "Companies", PermissionKey = "Masters.Companies.Delete", DisplayName = "Delete Company", Description = "Delete a company", SortOrder = 103, IsActive = true },
        
        // Masters - Customers screen
        new Permission { Module = "Masters", Screen = "Customers", PermissionKey = "Masters.Customers.View", DisplayName = "View Customers", Description = "View list of customers", SortOrder = 110, IsActive = true },
        new Permission { Module = "Masters", Screen = "Customers", PermissionKey = "Masters.Customers.Add", DisplayName = "Add Customer", Description = "Create a new customer", SortOrder = 111, IsActive = true },
        new Permission { Module = "Masters", Screen = "Customers", PermissionKey = "Masters.Customers.Edit", DisplayName = "Edit Customer", Description = "Edit existing customer", SortOrder = 112, IsActive = true },
        new Permission { Module = "Masters", Screen = "Customers", PermissionKey = "Masters.Customers.Delete", DisplayName = "Delete Customer", Description = "Delete a customer", SortOrder = 113, IsActive = true },
        
        // Masters - Vendors screen
        new Permission { Module = "Masters", Screen = "Vendors", PermissionKey = "Masters.Vendors.View", DisplayName = "View Vendors", Description = "View list of vendors", SortOrder = 120, IsActive = true },
        new Permission { Module = "Masters", Screen = "Vendors", PermissionKey = "Masters.Vendors.Add", DisplayName = "Add Vendor", Description = "Create a new vendor", SortOrder = 121, IsActive = true },
        new Permission { Module = "Masters", Screen = "Vendors", PermissionKey = "Masters.Vendors.Edit", DisplayName = "Edit Vendor", Description = "Edit existing vendor", SortOrder = 122, IsActive = true },
        new Permission { Module = "Masters", Screen = "Vendors", PermissionKey = "Masters.Vendors.Delete", DisplayName = "Delete Vendor", Description = "Delete a vendor", SortOrder = 123, IsActive = true },
        
        // Masters - Sources screen
        new Permission { Module = "Masters", Screen = "Sources", PermissionKey = "Masters.Sources.View", DisplayName = "View Sources", Description = "View list of sources", SortOrder = 130, IsActive = true },
        new Permission { Module = "Masters", Screen = "Sources", PermissionKey = "Masters.Sources.Add", DisplayName = "Add Source", Description = "Create a new source", SortOrder = 131, IsActive = true },
        new Permission { Module = "Masters", Screen = "Sources", PermissionKey = "Masters.Sources.Edit", DisplayName = "Edit Source", Description = "Edit existing source", SortOrder = 132, IsActive = true },
        new Permission { Module = "Masters", Screen = "Sources", PermissionKey = "Masters.Sources.Delete", DisplayName = "Delete Source", Description = "Delete a source", SortOrder = 133, IsActive = true },
        
        // Masters - Destinations screen
        new Permission { Module = "Masters", Screen = "Destinations", PermissionKey = "Masters.Destinations.View", DisplayName = "View Destinations", Description = "View list of destinations", SortOrder = 140, IsActive = true },
        new Permission { Module = "Masters", Screen = "Destinations", PermissionKey = "Masters.Destinations.Add", DisplayName = "Add Destination", Description = "Create a new destination", SortOrder = 141, IsActive = true },
        new Permission { Module = "Masters", Screen = "Destinations", PermissionKey = "Masters.Destinations.Edit", DisplayName = "Edit Destination", Description = "Edit existing destination", SortOrder = 142, IsActive = true },
        new Permission { Module = "Masters", Screen = "Destinations", PermissionKey = "Masters.Destinations.Delete", DisplayName = "Delete Destination", Description = "Delete a destination", SortOrder = 143, IsActive = true },
        
        // Masters - Materials screen
        new Permission { Module = "Masters", Screen = "Materials", PermissionKey = "Masters.Materials.View", DisplayName = "View Materials", Description = "View list of materials", SortOrder = 150, IsActive = true },
        new Permission { Module = "Masters", Screen = "Materials", PermissionKey = "Masters.Materials.Add", DisplayName = "Add Material", Description = "Create a new material", SortOrder = 151, IsActive = true },
        new Permission { Module = "Masters", Screen = "Materials", PermissionKey = "Masters.Materials.Edit", DisplayName = "Edit Material", Description = "Edit existing material", SortOrder = 152, IsActive = true },
        new Permission { Module = "Masters", Screen = "Materials", PermissionKey = "Masters.Materials.Delete", DisplayName = "Delete Material", Description = "Delete a material", SortOrder = 153, IsActive = true },
        
        // Masters - Fuel Pumps screen
        new Permission { Module = "Masters", Screen = "FuelPumps", PermissionKey = "Masters.FuelPumps.View", DisplayName = "View Fuel Pumps", Description = "View list of fuel pumps", SortOrder = 160, IsActive = true },
        new Permission { Module = "Masters", Screen = "FuelPumps", PermissionKey = "Masters.FuelPumps.Add", DisplayName = "Add Fuel Pump", Description = "Create a new fuel pump", SortOrder = 161, IsActive = true },
        new Permission { Module = "Masters", Screen = "FuelPumps", PermissionKey = "Masters.FuelPumps.Edit", DisplayName = "Edit Fuel Pump", Description = "Edit existing fuel pump", SortOrder = 162, IsActive = true },
        new Permission { Module = "Masters", Screen = "FuelPumps", PermissionKey = "Masters.FuelPumps.Delete", DisplayName = "Delete Fuel Pump", Description = "Delete a fuel pump", SortOrder = 163, IsActive = true },
        
        // Masters - HSD Rates screen
        new Permission { Module = "Masters", Screen = "HsdRates", PermissionKey = "Masters.HsdRates.View", DisplayName = "View HSD Rates", Description = "View list of HSD rates", SortOrder = 170, IsActive = true },
        new Permission { Module = "Masters", Screen = "HsdRates", PermissionKey = "Masters.HsdRates.Add", DisplayName = "Add HSD Rate", Description = "Create a new HSD rate", SortOrder = 171, IsActive = true },
        new Permission { Module = "Masters", Screen = "HsdRates", PermissionKey = "Masters.HsdRates.Edit", DisplayName = "Edit HSD Rate", Description = "Edit existing HSD rate", SortOrder = 172, IsActive = true },
        new Permission { Module = "Masters", Screen = "HsdRates", PermissionKey = "Masters.HsdRates.Delete", DisplayName = "Delete HSD Rate", Description = "Delete an HSD rate", SortOrder = 173, IsActive = true },
        
        // Masters - Payment Locations screen
        new Permission { Module = "Masters", Screen = "PaymentLocations", PermissionKey = "Masters.PaymentLocations.View", DisplayName = "View Payment Locations", Description = "View list of payment locations", SortOrder = 180, IsActive = true },
        new Permission { Module = "Masters", Screen = "PaymentLocations", PermissionKey = "Masters.PaymentLocations.Add", DisplayName = "Add Payment Location", Description = "Create a new payment location", SortOrder = 181, IsActive = true },
        new Permission { Module = "Masters", Screen = "PaymentLocations", PermissionKey = "Masters.PaymentLocations.Edit", DisplayName = "Edit Payment Location", Description = "Edit existing payment location", SortOrder = 182, IsActive = true },
        new Permission { Module = "Masters", Screen = "PaymentLocations", PermissionKey = "Masters.PaymentLocations.Delete", DisplayName = "Delete Payment Location", Description = "Delete a payment location", SortOrder = 183, IsActive = true },
        
        // Masters - Vehicle Owners screen
        new Permission { Module = "Masters", Screen = "VehicleOwners", PermissionKey = "Masters.VehicleOwners.View", DisplayName = "View Vehicle Owners", Description = "View list of vehicle owners", SortOrder = 190, IsActive = true },
        new Permission { Module = "Masters", Screen = "VehicleOwners", PermissionKey = "Masters.VehicleOwners.Add", DisplayName = "Add Vehicle Owner", Description = "Create a new vehicle owner", SortOrder = 191, IsActive = true },
        new Permission { Module = "Masters", Screen = "VehicleOwners", PermissionKey = "Masters.VehicleOwners.Edit", DisplayName = "Edit Vehicle Owner", Description = "Edit existing vehicle owner", SortOrder = 192, IsActive = true },
        new Permission { Module = "Masters", Screen = "VehicleOwners", PermissionKey = "Masters.VehicleOwners.Delete", DisplayName = "Delete Vehicle Owner", Description = "Delete a vehicle owner", SortOrder = 193, IsActive = true }
    };

    /// <summary>
    /// Ensures that all listed permissions exist in the database. Idempotent.
    /// </summary>
    public static async Task EnsurePermissionsAsync(VeteranLogisticsDbContext dbContext, CancellationToken cancellationToken = default)
    {
        if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));

        var existingKeys = await dbContext.Permissions
            .AsNoTracking()
            .Select(p => p.PermissionKey)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var toAdd = Permissions.Where(p => !existingKeys.Contains(p.PermissionKey)).ToList();
        if (!toAdd.Any()) return;

        await dbContext.Permissions.AddRangeAsync(toAdd, cancellationToken).ConfigureAwait(false);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
