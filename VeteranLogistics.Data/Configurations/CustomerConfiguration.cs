using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VeteranLogistics.Data.Entities.Administration;

namespace VeteranLogistics.Data.Configurations;

/// <summary>
/// Entity configuration for the Customer entity.
/// </summary>
public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        // Configure CustomerCode as required and unique
        builder.Property(c => c.CustomerCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(c => c.CustomerCode)
            .IsUnique();

        // Configure CustomerName as required
        builder.Property(c => c.CustomerName)
            .IsRequired()
            .HasMaxLength(200);

        // Configure AddressLine1
        builder.Property(c => c.AddressLine1)
            .HasMaxLength(200);

        // Configure AddressLine2
        builder.Property(c => c.AddressLine2)
            .HasMaxLength(200);

        // Configure City
        builder.Property(c => c.City)
            .HasMaxLength(100);

        // Configure State
        builder.Property(c => c.State)
            .HasMaxLength(100);

        // Configure Country
        builder.Property(c => c.Country)
            .HasMaxLength(100);

        // Configure PostalCode
        builder.Property(c => c.PostalCode)
            .HasMaxLength(20);

        // Configure PhoneNumber
        builder.Property(c => c.PhoneNumber)
            .HasMaxLength(20);

        // Configure Email
        builder.Property(c => c.Email)
            .HasMaxLength(200);

        // Configure GSTNumber as required and unique
        builder.Property(c => c.GSTNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(c => c.GSTNumber)
            .IsUnique();

        // Configure PANNumber
        builder.Property(c => c.PANNumber)
            .HasMaxLength(20);

        // Configure CreatedBy
        builder.Property(c => c.CreatedBy)
            .HasMaxLength(100);

        // Configure ModifiedBy
        builder.Property(c => c.ModifiedBy)
            .HasMaxLength(100);

        // Configure global query filter to automatically exclude soft-deleted customers
        builder.HasQueryFilter(customer => !customer.IsDeleted);
    }
}
