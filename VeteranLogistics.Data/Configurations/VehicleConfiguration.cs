using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VeteranLogistics.Data.Entities.Administration;

namespace VeteranLogistics.Data.Configurations;

/// <summary>
/// Entity configuration for the Vehicle entity.
/// </summary>
public sealed class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        // Configure foreign key to VehicleOwner
        builder.HasOne(v => v.VehicleOwner)
            .WithMany()
            .HasForeignKey(v => v.VehicleOwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure VehicleNumber as required and unique
        builder.Property(v => v.VehicleNumber)
            .IsRequired()
            .HasMaxLength(30);

        builder.HasIndex(v => v.VehicleNumber)
            .IsUnique();

        // Configure VehicleType as required
        builder.Property(v => v.VehicleType)
            .IsRequired()
            .HasMaxLength(50);

        // Configure CreatedBy
        builder.Property(v => v.CreatedBy)
            .HasMaxLength(100);

        // Configure ModifiedBy
        builder.Property(v => v.ModifiedBy)
            .HasMaxLength(100);

        // Configure global query filter to automatically exclude soft-deleted vehicles
        builder.HasQueryFilter(vehicle => !vehicle.IsDeleted);
    }
}
