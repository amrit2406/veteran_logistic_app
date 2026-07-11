using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VeteranLogistics.Data.Entities.Administration;

namespace VeteranLogistics.Data.Configurations;

/// <summary>
/// Entity configuration for the FuelPump entity.
/// </summary>
public sealed class FuelPumpConfiguration : IEntityTypeConfiguration<FuelPump>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<FuelPump> builder)
    {
        // Configure FuelPumpName as required and unique
        builder.Property(f => f.FuelPumpName)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(f => f.FuelPumpName)
            .IsUnique();

        // Configure CreatedBy
        builder.Property(f => f.CreatedBy)
            .HasMaxLength(100);

        // Configure ModifiedBy
        builder.Property(f => f.ModifiedBy)
            .HasMaxLength(100);

        // Configure global query filter to automatically exclude soft-deleted fuel pumps
        builder.HasQueryFilter(fuelPump => !fuelPump.IsDeleted);
    }
}
