using Microsoft.EntityFrameworkCore;
using veteran_logistic.Authentication.Contracts;
using veteran_logistic.Authentication.Models;
using veteran_logistic.Configuration.Options;
using VeteranLogistics.Data.Context;
using VeteranLogistics.Data.Entities.Administration;

namespace VeteranLogistics.Data.Seed;

/// <summary>
/// Seeds authentication data including a default administrator account.
/// </summary>
public static class AuthenticationSeed
{
    /// <summary>
    /// Ensures the default administrator account exists.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="authenticationOptions">Authentication configuration options.</param>
    /// <param name="passwordHasher">Password hasher service.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public static async Task EnsureDefaultAdministratorAsync(
        VeteranLogisticsDbContext dbContext,
        AuthenticationOptions authenticationOptions,
        IPasswordHasher passwordHasher,
        CancellationToken cancellationToken = default)
    {
        if (dbContext is null)
        {
            throw new ArgumentNullException(nameof(dbContext));
        }

        if (authenticationOptions is null)
        {
            throw new ArgumentNullException(nameof(authenticationOptions));
        }

        if (passwordHasher is null)
        {
            throw new ArgumentNullException(nameof(passwordHasher));
        }

        var adminUsername = authenticationOptions.DefaultAdmin.Username;
        var adminPassword = authenticationOptions.DefaultAdmin.Password;
        var adminDisplayName = authenticationOptions.DefaultAdmin.DisplayName;
        var adminEmail = authenticationOptions.DefaultAdmin.Email;

        if (string.IsNullOrWhiteSpace(adminUsername) || string.IsNullOrWhiteSpace(adminPassword))
        {
            throw new InvalidOperationException("Default administrator username and password must be configured.");
        }

        var existingAdmin = await dbContext.Set<User>()
            .FirstOrDefaultAsync(u => u.Username == adminUsername, cancellationToken);

        if (existingAdmin is null)
        {
            // Hash password using IPasswordHasher
            var hashResult = passwordHasher.HashPassword(adminPassword);

            var adminUser = new User
            {
                Username = adminUsername,
                PasswordHash = hashResult.Hash,
                PasswordSalt = hashResult.Salt,
                DisplayName = adminDisplayName,
                Email = adminEmail,
                Role = "Administrator",
                IsActive = true,
                CreatedOn = DateTime.UtcNow
            };

            await dbContext.Set<User>().AddAsync(adminUser, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
