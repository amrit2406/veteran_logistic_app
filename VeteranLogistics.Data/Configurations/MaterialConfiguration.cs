using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VeteranLogistics.Data.Entities.Administration;

namespace VeteranLogistics.Data.Configurations;

/// <summary>
/// Entity configuration for the Material entity.
/// </summary>
public sealed class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        // Configure MaterialName as required and unique
        builder.Property(m => m.MaterialName)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(m => m.MaterialName)
            .IsUnique();

        // Configure CreatedBy
        builder.Property(m => m.CreatedBy)
            .HasMaxLength(100);

        // Configure ModifiedBy
        builder.Property(m => m.ModifiedBy)
            .HasMaxLength(100);

        // Configure global query filter to automatically exclude soft-deleted materials
        builder.HasQueryFilter(material => !material.IsDeleted);
    }
}
