using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VeteranLogistics.Data.Entities.Administration;

namespace VeteranLogistics.Data.Configurations;

/// <summary>
/// Entity configuration for the Vendor entity.
/// </summary>
public sealed class VendorConfiguration : IEntityTypeConfiguration<Vendor>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Vendor> builder)
    {
        // Configure VendorCode as required and unique
        builder.Property(v => v.VendorCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(v => v.VendorCode)
            .IsUnique();

        // Configure VendorName as required
        builder.Property(v => v.VendorName)
            .IsRequired()
            .HasMaxLength(200);

        // Configure AddressLine1
        builder.Property(v => v.AddressLine1)
            .HasMaxLength(200);

        // Configure AddressLine2
        builder.Property(v => v.AddressLine2)
            .HasMaxLength(200);

        // Configure City
        builder.Property(v => v.City)
            .HasMaxLength(100);

        // Configure State
        builder.Property(v => v.State)
            .HasMaxLength(100);

        // Configure Country
        builder.Property(v => v.Country)
            .HasMaxLength(100);

        // Configure PostalCode
        builder.Property(v => v.PostalCode)
            .HasMaxLength(20);

        // Configure PhoneNumber
        builder.Property(v => v.PhoneNumber)
            .HasMaxLength(20);

        // Configure Email
        builder.Property(v => v.Email)
            .HasMaxLength(200);

        // Configure GSTNumber as required and unique
        builder.Property(v => v.GSTNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(v => v.GSTNumber)
            .IsUnique();

        // Configure PANNumber
        builder.Property(v => v.PANNumber)
            .HasMaxLength(20);

        // Configure ContactPerson
        builder.Property(v => v.ContactPerson)
            .HasMaxLength(200);

        // Configure CreatedBy
        builder.Property(v => v.CreatedBy)
            .HasMaxLength(100);

        // Configure ModifiedBy
        builder.Property(v => v.ModifiedBy)
            .HasMaxLength(100);

        // Configure global query filter to automatically exclude soft-deleted vendors
        builder.HasQueryFilter(vendor => !vendor.IsDeleted);
    }
}
