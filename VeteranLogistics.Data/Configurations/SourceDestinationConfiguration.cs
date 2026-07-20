using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VeteranLogistics.Data.Entities.Administration;

namespace VeteranLogistics.Data.Configurations;

/// <summary>
/// Entity configuration for the SourceDestination entity.
/// </summary>
public sealed class SourceDestinationConfiguration : IEntityTypeConfiguration<SourceDestination>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<SourceDestination> builder)
    {
        // Configure LocationName as required and unique
        builder.Property(sd => sd.LocationName)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("SourceDestination");

        builder.HasIndex(sd => sd.LocationName)
            .IsUnique();

        // Configure CreatedBy
        builder.Property(sd => sd.CreatedBy)
            .HasMaxLength(100);

        // Configure ModifiedBy
        builder.Property(sd => sd.ModifiedBy)
            .HasMaxLength(100);

        // Configure global query filter to automatically exclude soft-deleted source/destinations
        builder.HasQueryFilter(sourceDestination => !sourceDestination.IsDeleted);
    }
}
