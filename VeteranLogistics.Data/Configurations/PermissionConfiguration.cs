using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VeteranLogistics.Data.Entities.Administration;

namespace VeteranLogistics.Data.Configurations;

/// <summary>
/// Entity configuration for the Permission entity.
/// </summary>
public sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        // Configure Module as required
        builder.Property(p => p.Module)
            .IsRequired()
            .HasMaxLength(100);

        // Configure Screen as required
        builder.Property(p => p.Screen)
            .IsRequired()
            .HasMaxLength(100);

        // Configure PermissionKey as required and unique
        builder.Property(p => p.PermissionKey)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(p => p.PermissionKey)
            .IsUnique();

        // Configure DisplayName as required
        builder.Property(p => p.DisplayName)
            .IsRequired()
            .HasMaxLength(200);

        // Configure Description as optional with max length
        builder.Property(p => p.Description)
            .HasMaxLength(500);

        // Configure composite unique index for Module and Screen
        builder.HasIndex(p => new { p.Module, p.Screen });

        // Configure global query filter to automatically exclude soft-deleted permissions
        builder.HasQueryFilter(permission => !permission.IsDeleted);
    }
}
