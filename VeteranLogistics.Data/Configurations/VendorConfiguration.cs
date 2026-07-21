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
        // Configure Code as required and unique (auto-generated)
        builder.Property(v => v.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(v => v.Code)
            .IsUnique();

        // Configure Type as required (Union/Vendor)
        builder.Property(v => v.Type)
            .IsRequired()
            .HasMaxLength(50);

        // Configure Name as required
        builder.Property(v => v.Name)
            .IsRequired()
            .HasMaxLength(200);

        // Configure CorrespondenceAddress
        builder.Property(v => v.CorrespondenceAddress)
            .HasMaxLength(500);

        // Configure City
        builder.Property(v => v.City)
            .HasMaxLength(100);

        // Configure BillingAddress
        builder.Property(v => v.BillingAddress)
            .HasMaxLength(500);

        // Configure Phone
        builder.Property(v => v.Phone)
            .HasMaxLength(20);

        // Configure Mobile
        builder.Property(v => v.Mobile)
            .HasMaxLength(20);

        // Configure Fax
        builder.Property(v => v.Fax)
            .HasMaxLength(20);

        // Configure Email
        builder.Property(v => v.Email)
            .HasMaxLength(200);

        // Configure ServiceTax
        builder.Property(v => v.ServiceTax)
            .HasMaxLength(50);

        // Configure CST
        builder.Property(v => v.CST)
            .HasMaxLength(50);

        // Configure PAN
        builder.Property(v => v.PAN)
            .HasMaxLength(20);

        // Configure GSTIN
        builder.Property(v => v.GSTIN)
            .HasMaxLength(50);

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
