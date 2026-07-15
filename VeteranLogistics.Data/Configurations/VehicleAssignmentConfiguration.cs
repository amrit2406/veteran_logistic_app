using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VeteranLogistics.Data.Entities.Administration;

namespace VeteranLogistics.Data.Configurations;

/// <summary>
/// Entity configuration for the VehicleAssignment entity.
/// </summary>
public sealed class VehicleAssignmentConfiguration : IEntityTypeConfiguration<VehicleAssignment>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<VehicleAssignment> builder)
    {
        // Configure foreign key to Vehicle
        builder.HasOne(va => va.Vehicle)
            .WithMany()
            .HasForeignKey(va => va.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure foreign key to VehicleOwner
        builder.HasOne(va => va.VehicleOwner)
            .WithMany()
            .HasForeignKey(va => va.VehicleOwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure AssignDate as required
        builder.Property(va => va.AssignDate)
            .IsRequired();

        // Configure ReleaseDate as optional
        builder.Property(va => va.ReleaseDate)
            .IsRequired(false);

        // Configure CreatedBy
        builder.Property(va => va.CreatedBy)
            .HasMaxLength(100);

        // Configure ModifiedBy
        builder.Property(va => va.ModifiedBy)
            .HasMaxLength(100);

        // Configure global query filter to automatically exclude soft-deleted vehicle assignments
        builder.HasQueryFilter(assignment => !assignment.IsDeleted);

        // Configure unique constraint to ensure a vehicle can have only one active assignment at a time
        // This is enforced at the business logic level, but we can add an index for performance
        builder.HasIndex(va => new { va.VehicleId, va.ReleaseDate })
            .HasFilter("[ReleaseDate] IS NULL")
            .IsUnique();
    }
}
