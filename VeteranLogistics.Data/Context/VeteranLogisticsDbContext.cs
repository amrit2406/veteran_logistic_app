using Microsoft.EntityFrameworkCore;
using VeteranLogistics.Data.Entities.Administration;

namespace VeteranLogistics.Data.Context;

/// <summary>
/// EF Core DbContext for Veteran Logistics application.
/// Fluent API configurations are automatically discovered from the assembly.
/// </summary>
public class VeteranLogisticsDbContext : DbContext
{
    /// <summary>
    /// Construct a new instance of <see cref="VeteranLogisticsDbContext"/>.
    /// </summary>
    /// <param name="options">The DbContext options.</param>
    public VeteranLogisticsDbContext(DbContextOptions<VeteranLogisticsDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the Users DbSet.
    /// </summary>
    public DbSet<User> Users => Set<User>();

    /// <summary>
    /// Gets or sets the Roles DbSet.
    /// </summary>
    public DbSet<Role> Roles => Set<Role>();

    /// <summary>
    /// Gets or sets the Permissions DbSet.
    /// </summary>
    public DbSet<Permission> Permissions => Set<Permission>();

    /// <summary>
    /// Gets or sets the RolePermissions DbSet.
    /// </summary>
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

    /// <summary>
    /// Gets or sets the FinancialYears DbSet.
    /// </summary>
    public DbSet<FinancialYear> FinancialYears => Set<FinancialYear>();

    /// <summary>
    /// Gets or sets the Companies DbSet.
    /// </summary>
    public DbSet<Company> Companies => Set<Company>();

    /// <summary>
    /// Gets or sets the Customers DbSet.
    /// </summary>
    public DbSet<Customer> Customers => Set<Customer>();

    /// <summary>
    /// Gets or sets the Vendors DbSet.
    /// </summary>
    public DbSet<Vendor> Vendors => Set<Vendor>();

    /// <summary>
    /// Gets or sets the Sources DbSet.
    /// </summary>
    public DbSet<Source> Sources => Set<Source>();

    /// <summary>
    /// Gets or sets the Destinations DbSet.
    /// </summary>
    public DbSet<Destination> Destinations => Set<Destination>();

    /// <summary>
    /// Gets or sets the Materials DbSet.
    /// </summary>
    public DbSet<Material> Materials => Set<Material>();

    /// <summary>
    /// Apply entity configurations discovered in this assembly.
    /// </summary>
    /// <param name="modelBuilder">Model builder.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Automatically discover and apply IEntityTypeConfiguration<T> implementations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VeteranLogisticsDbContext).Assembly);
    }
}
