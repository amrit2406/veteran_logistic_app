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
/// Seeds the Permission table with administration module permissions.
/// </summary>
public static class PermissionSeed
{
    private static readonly List<Permission> Permissions = new()
    {
        // Users screen
        new Permission { Module = "Administration", Screen = "Users", PermissionKey = "Administration.Users.View", DisplayName = "View Users", Description = "View list of users", SortOrder = 1, IsActive = true },
        new Permission { Module = "Administration", Screen = "Users", PermissionKey = "Administration.Users.Add", DisplayName = "Add User", Description = "Create a new user", SortOrder = 2, IsActive = true },
        new Permission { Module = "Administration", Screen = "Users", PermissionKey = "Administration.Users.Edit", DisplayName = "Edit User", Description = "Edit existing user", SortOrder = 3, IsActive = true },
        new Permission { Module = "Administration", Screen = "Users", PermissionKey = "Administration.Users.Activate", DisplayName = "Activate User", Description = "Activate a user account", SortOrder = 4, IsActive = true },
        new Permission { Module = "Administration", Screen = "Users", PermissionKey = "Administration.Users.Delete", DisplayName = "Delete User", Description = "Delete a user account", SortOrder = 5, IsActive = true },
        // Roles screen
        new Permission { Module = "Administration", Screen = "Roles", PermissionKey = "Administration.Roles.View", DisplayName = "View Roles", Description = "View list of roles", SortOrder = 10, IsActive = true },
        new Permission { Module = "Administration", Screen = "Roles", PermissionKey = "Administration.Roles.Add", DisplayName = "Add Role", Description = "Create a new role", SortOrder = 11, IsActive = true },
        new Permission { Module = "Administration", Screen = "Roles", PermissionKey = "Administration.Roles.Edit", DisplayName = "Edit Role", Description = "Edit existing role", SortOrder = 12, IsActive = true },
        new Permission { Module = "Administration", Screen = "Roles", PermissionKey = "Administration.Roles.Activate", DisplayName = "Activate Role", Description = "Activate a role", SortOrder = 13, IsActive = true },
        new Permission { Module = "Administration", Screen = "Roles", PermissionKey = "Administration.Roles.Delete", DisplayName = "Delete Role", Description = "Delete a role", SortOrder = 14, IsActive = true },
        // Permission Matrix screen
        new Permission { Module = "Administration", Screen = "PermissionMatrix", PermissionKey = "Administration.PermissionMatrix.View", DisplayName = "View Permission Matrix", Description = "View permission matrix", SortOrder = 20, IsActive = true },
        new Permission { Module = "Administration", Screen = "PermissionMatrix", PermissionKey = "Administration.PermissionMatrix.Manage", DisplayName = "Manage Permission Matrix", Description = "Edit permission assignments", SortOrder = 21, IsActive = true },
        // Financial Year screen
        new Permission { Module = "Administration", Screen = "FinancialYear", PermissionKey = "Administration.FinancialYear.View", DisplayName = "View Financial Years", Description = "View financial year list", SortOrder = 30, IsActive = true },
        new Permission { Module = "Administration", Screen = "FinancialYear", PermissionKey = "Administration.FinancialYear.Add", DisplayName = "Add Financial Year", Description = "Create a new financial year", SortOrder = 31, IsActive = true },
        new Permission { Module = "Administration", Screen = "FinancialYear", PermissionKey = "Administration.FinancialYear.Edit", DisplayName = "Edit Financial Year", Description = "Edit existing financial year", SortOrder = 32, IsActive = true },
        new Permission { Module = "Administration", Screen = "FinancialYear", PermissionKey = "Administration.FinancialYear.Activate", DisplayName = "Activate Financial Year", Description = "Activate a financial year", SortOrder = 33, IsActive = true },
        new Permission { Module = "Administration", Screen = "FinancialYear", PermissionKey = "Administration.FinancialYear.Close", DisplayName = "Close Financial Year", Description = "Close a financial year", SortOrder = 34, IsActive = true }
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
