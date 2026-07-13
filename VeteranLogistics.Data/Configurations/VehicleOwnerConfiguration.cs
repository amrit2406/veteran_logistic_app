using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VeteranLogistics.Data.Entities.Administration;

namespace VeteranLogistics.Data.Configurations;

/// <summary>
/// Entity configuration for the VehicleOwner entity.
/// </summary>
public sealed class VehicleOwnerConfiguration : IEntityTypeConfiguration<VehicleOwner>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<VehicleOwner> builder)
    {
        // Configure OwnerCode as required and unique
        builder.Property(o => o.OwnerCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(o => o.OwnerCode)
            .IsUnique();

        // Configure PANType as required
        builder.Property(o => o.PANType)
            .IsRequired()
            .HasMaxLength(50);

        // Configure PANNumber as required and unique
        builder.Property(o => o.PANNumber)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasIndex(o => o.PANNumber)
            .IsUnique();

        // Configure FirstName as required
        builder.Property(o => o.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        // Configure MiddleName
        builder.Property(o => o.MiddleName)
            .HasMaxLength(100);

        // Configure LastName
        builder.Property(o => o.LastName)
            .HasMaxLength(100);

        // Configure CompanyName
        builder.Property(o => o.CompanyName)
            .HasMaxLength(200);

        // Configure City as required
        builder.Property(o => o.City)
            .IsRequired()
            .HasMaxLength(100);

        // Configure State as required
        builder.Property(o => o.State)
            .IsRequired()
            .HasMaxLength(100);

        // Configure Address
        builder.Property(o => o.Address)
            .HasMaxLength(500);

        // Configure Phone
        builder.Property(o => o.Phone)
            .HasMaxLength(20);

        // Configure Mobile as required
        builder.Property(o => o.Mobile)
            .IsRequired()
            .HasMaxLength(20);

        // Configure Email
        builder.Property(o => o.Email)
            .HasMaxLength(150);

        // Configure Fax
        builder.Property(o => o.Fax)
            .HasMaxLength(50);

        // Configure CreatedBy
        builder.Property(o => o.CreatedBy)
            .HasMaxLength(100);

        // Configure ModifiedBy
        builder.Property(o => o.ModifiedBy)
            .HasMaxLength(100);

        // Configure global query filter to automatically exclude soft-deleted vehicle owners
        builder.HasQueryFilter(owner => !owner.IsDeleted);
    }
}
