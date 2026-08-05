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
        new Permission { Module = "Administration", Screen = "Users", PermissionKey = "Administration.Users.View", DisplayName = "View Users", Description = "View list of users", SortOrder = 10, IsActive = true },
        new Permission { Module = "Administration", Screen = "Users", PermissionKey = "Administration.Users.Add", DisplayName = "Add User", Description = "Create a new user", SortOrder = 11, IsActive = true },
        new Permission { Module = "Administration", Screen = "Users", PermissionKey = "Administration.Users.Edit", DisplayName = "Edit User", Description = "Edit existing user", SortOrder = 12, IsActive = true },
        new Permission { Module = "Administration", Screen = "Users", PermissionKey = "Administration.Users.Activate", DisplayName = "Activate User", Description = "Activate a user account", SortOrder = 13, IsActive = true },
        new Permission { Module = "Administration", Screen = "Users", PermissionKey = "Administration.Users.Delete", DisplayName = "Delete User", Description = "Delete a user account", SortOrder = 14, IsActive = true },
        
        // Administration - Roles screen
        new Permission { Module = "Administration", Screen = "Roles", PermissionKey = "Administration.Roles.View", DisplayName = "View Roles", Description = "View list of roles", SortOrder = 20, IsActive = true },
        new Permission { Module = "Administration", Screen = "Roles", PermissionKey = "Administration.Roles.Add", DisplayName = "Add Role", Description = "Create a new role", SortOrder = 21, IsActive = true },
        new Permission { Module = "Administration", Screen = "Roles", PermissionKey = "Administration.Roles.Edit", DisplayName = "Edit Role", Description = "Edit existing role", SortOrder = 22, IsActive = true },
        new Permission { Module = "Administration", Screen = "Roles", PermissionKey = "Administration.Roles.Activate", DisplayName = "Activate Role", Description = "Activate a role", SortOrder = 23, IsActive = true },
        new Permission { Module = "Administration", Screen = "Roles", PermissionKey = "Administration.Roles.Delete", DisplayName = "Delete Role", Description = "Delete a role", SortOrder = 24, IsActive = true },
        
        // Administration - Permission Matrix screen
        new Permission { Module = "Administration", Screen = "PermissionMatrix", PermissionKey = "Administration.PermissionMatrix.View", DisplayName = "View Permission Matrix", Description = "View permission matrix", SortOrder = 30, IsActive = true },
        new Permission { Module = "Administration", Screen = "PermissionMatrix", PermissionKey = "Administration.PermissionMatrix.Manage", DisplayName = "Manage Permission Matrix", Description = "Edit permission assignments", SortOrder = 31, IsActive = true },
        
        // Administration - Financial Year screen
        new Permission { Module = "Administration", Screen = "FinancialYear", PermissionKey = "Administration.FinancialYear.View", DisplayName = "View Financial Years", Description = "View financial year list", SortOrder = 40, IsActive = true },
        new Permission { Module = "Administration", Screen = "FinancialYear", PermissionKey = "Administration.FinancialYear.Add", DisplayName = "Add Financial Year", Description = "Create a new financial year", SortOrder = 41, IsActive = true },
        new Permission { Module = "Administration", Screen = "FinancialYear", PermissionKey = "Administration.FinancialYear.Edit", DisplayName = "Edit Financial Year", Description = "Edit existing financial year", SortOrder = 42, IsActive = true },
        new Permission { Module = "Administration", Screen = "FinancialYear", PermissionKey = "Administration.FinancialYear.Activate", DisplayName = "Activate Financial Year", Description = "Activate a financial year", SortOrder = 43, IsActive = true },
        new Permission { Module = "Administration", Screen = "FinancialYear", PermissionKey = "Administration.FinancialYear.Close", DisplayName = "Close Financial Year", Description = "Close a financial year", SortOrder = 44, IsActive = true },
        
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
        new Permission { Module = "Masters", Screen = "VehicleOwners", PermissionKey = "Masters.VehicleOwners.Delete", DisplayName = "Delete Vehicle Owner", Description = "Delete a vehicle owner", SortOrder = 193, IsActive = true }, 
        
        // Masters - Vehicles screen
        new Permission { Module = "Masters", Screen = "Vehicles", PermissionKey = "vehicles.view", DisplayName = "View Vehicles", Description = "View list of vehicles", SortOrder = 1, IsActive = true },
        new Permission { Module = "Masters", Screen = "Vehicles", PermissionKey = "vehicles.create", DisplayName = "Create Vehicles", Description = "Create a new vehicle", SortOrder = 2, IsActive = true },
        new Permission { Module = "Masters", Screen = "Vehicles", PermissionKey = "vehicles.edit", DisplayName = "Edit Vehicles", Description = "Edit existing vehicle", SortOrder = 3, IsActive = true },
        new Permission { Module = "Masters", Screen = "Vehicles", PermissionKey = "vehicles.delete", DisplayName = "Delete Vehicles", Description = "Delete a vehicle", SortOrder = 4, IsActive = true },

        // Masters - DO Rates screen
        new Permission { Module = "Masters", Screen = "DORates", PermissionKey = "masters.dorates.view", DisplayName = "View DO Rates", Description = "View list of DO rates", SortOrder = 5, IsActive = true },
        new Permission { Module = "Masters", Screen = "DORates", PermissionKey = "masters.dorates.add", DisplayName = "Add DO Rate", Description = "Create a new DO rate", SortOrder = 6, IsActive = true },
        new Permission { Module = "Masters", Screen = "DORates", PermissionKey = "masters.dorates.edit", DisplayName = "Edit DO Rate", Description = "Edit existing DO rate", SortOrder = 7, IsActive = true },
        new Permission { Module = "Masters", Screen = "DORates", PermissionKey = "masters.dorates.delete", DisplayName = "Delete DO Rate", Description = "Delete a DO rate", SortOrder = 8, IsActive = true },

        // Transactions - Loading Registers screen
        new Permission { Module = "Transactions", Screen = "LoadingRegisters", PermissionKey = "transactions.loadingregisters.view", DisplayName = "View Loading Registers", Description = "View list of loading registers", SortOrder = 200, IsActive = true },
        new Permission { Module = "Transactions", Screen = "LoadingRegisters", PermissionKey = "transactions.loadingregisters.add", DisplayName = "Add Loading Register", Description = "Create a new loading register", SortOrder = 201, IsActive = true },
        new Permission { Module = "Transactions", Screen = "LoadingRegisters", PermissionKey = "transactions.loadingregisters.edit", DisplayName = "Edit Loading Register", Description = "Edit existing loading register", SortOrder = 202, IsActive = true },
        new Permission { Module = "Transactions", Screen = "LoadingRegisters", PermissionKey = "transactions.loadingregisters.delete", DisplayName = "Delete Loading Register", Description = "Delete a loading register", SortOrder = 203, IsActive = true },
        new Permission { Module = "Transactions", Screen = "LoadingRegisters", PermissionKey = "transactions.loadingregisters.updatestatus", DisplayName = "Update Loading Register Status", Description = "Update loading register status", SortOrder = 204, IsActive = true },

        // Transactions - Payment Registers screen
        new Permission { Module = "Transactions", Screen = "PaymentRegisters", PermissionKey = "transactions.paymentregisters.view", DisplayName = "View Payment Registers", Description = "View list of payment registers", SortOrder = 210, IsActive = true },
        new Permission { Module = "Transactions", Screen = "PaymentRegisters", PermissionKey = "transactions.paymentregisters.add", DisplayName = "Add Payment Register", Description = "Create a new payment register", SortOrder = 211, IsActive = true },
        new Permission { Module = "Transactions", Screen = "PaymentRegisters", PermissionKey = "transactions.paymentregisters.edit", DisplayName = "Edit Payment Register", Description = "Edit existing payment register", SortOrder = 212, IsActive = true },
        new Permission { Module = "Transactions", Screen = "PaymentRegisters", PermissionKey = "transactions.paymentregisters.delete", DisplayName = "Delete Payment Register", Description = "Delete a payment register", SortOrder = 213, IsActive = true },

        // Transactions - Unloading Registers screen
        new Permission { Module = "Transactions", Screen = "UnloadingRegisters", PermissionKey = "transactions.unloadingregisters.view", DisplayName = "View Unloading Registers", Description = "View list of unloading registers", SortOrder = 220, IsActive = true },
        new Permission { Module = "Transactions", Screen = "UnloadingRegisters", PermissionKey = "transactions.unloadingregisters.add", DisplayName = "Add Unloading Register", Description = "Create a new unloading register", SortOrder = 221, IsActive = true },
        new Permission { Module = "Transactions", Screen = "UnloadingRegisters", PermissionKey = "transactions.unloadingregisters.edit", DisplayName = "Edit Unloading Register", Description = "Edit existing unloading register", SortOrder = 222, IsActive = true },
        new Permission { Module = "Transactions", Screen = "UnloadingRegisters", PermissionKey = "transactions.unloadingregisters.delete", DisplayName = "Delete Unloading Register", Description = "Delete an unloading register", SortOrder = 223, IsActive = true },

        // Transactions - Party Bill Register screen
        new Permission { Module = "Transactions", Screen = "PartyBillRegisters", PermissionKey = "transactions.partybillregisters.view", DisplayName = "View Party Bill Registers", Description = "View list of party bill registers", SortOrder = 230, IsActive = true },
        new Permission { Module = "Transactions", Screen = "PartyBillRegisters", PermissionKey = "transactions.partybillregisters.add", DisplayName = "Add Party Bill Register", Description = "Create a new party bill register", SortOrder = 231, IsActive = true },
        new Permission { Module = "Transactions", Screen = "PartyBillRegisters", PermissionKey = "transactions.partybillregisters.edit", DisplayName = "Edit Party Bill Register", Description = "Edit existing party bill register", SortOrder = 232, IsActive = true },
        new Permission { Module = "Transactions", Screen = "PartyBillRegisters", PermissionKey = "transactions.partybillregisters.delete", DisplayName = "Delete Party Bill Register", Description = "Delete a party bill register", SortOrder = 233, IsActive = true },

        // Reports - Loading Report screen
        new Permission { Module = "Reports", Screen = "LoadingReport", PermissionKey = "reports.loadingreport.view", DisplayName = "View Loading Report", Description = "View loading report", SortOrder = 300, IsActive = true },
        new Permission { Module = "Reports", Screen = "LoadingReport", PermissionKey = "reports.loadingreport.export", DisplayName = "Export Loading Report", Description = "Export loading report", SortOrder = 301, IsActive = true },

        // Reports - Payment Report screen
        new Permission { Module = "Reports", Screen = "PaymentReport", PermissionKey = "reports.paymentreport.view", DisplayName = "View Payment Report", Description = "View payment report", SortOrder = 310, IsActive = true },
        new Permission { Module = "Reports", Screen = "PaymentReport", PermissionKey = "reports.paymentreport.export", DisplayName = "Export Payment Report", Description = "Export payment report", SortOrder = 311, IsActive = true },

        // Reports - Unloading Report screen
        new Permission { Module = "Reports", Screen = "UnloadingReport", PermissionKey = "reports.unloadingreport.view", DisplayName = "View Unloading Report", Description = "View unloading report", SortOrder = 320, IsActive = true },
        new Permission { Module = "Reports", Screen = "UnloadingReport", PermissionKey = "reports.unloadingreport.export", DisplayName = "Export Unloading Report", Description = "Export unloading report", SortOrder = 321, IsActive = true },

        // Reports - Party Billing Report screen
        new Permission { Module = "Reports", Screen = "PartyBillingReport", PermissionKey = "reports.partybillingreport.view", DisplayName = "View Party Billing Report", Description = "View party billing report", SortOrder = 330, IsActive = true },
        new Permission { Module = "Reports", Screen = "PartyBillingReport", PermissionKey = "reports.partybillingreport.export", DisplayName = "Export Party Billing Report", Description = "Export party billing report", SortOrder = 331, IsActive = true },

        // Reports - DO Status Report screen
        new Permission { Module = "Reports", Screen = "DOStatusReport", PermissionKey = "reports.dostatusreport.view", DisplayName = "View DO Status Report", Description = "View DO status report", SortOrder = 340, IsActive = true },
        new Permission { Module = "Reports", Screen = "DOStatusReport", PermissionKey = "reports.dostatusreport.export", DisplayName = "Export DO Status Report", Description = "Export DO status report", SortOrder = 341, IsActive = true },

        // Reports - Consolidated Report screen
        new Permission { Module = "Reports", Screen = "ConsolidatedReport", PermissionKey = "reports.consolidatedreport.view", DisplayName = "View Consolidated Report", Description = "View consolidated report", SortOrder = 350, IsActive = true },
        new Permission { Module = "Reports", Screen = "ConsolidatedReport", PermissionKey = "reports.consolidatedreport.export", DisplayName = "Export Consolidated Report", Description = "Export consolidated report", SortOrder = 351, IsActive = true }

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
