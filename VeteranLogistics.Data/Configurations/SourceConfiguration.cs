using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VeteranLogistics.Data.Entities.Administration;

namespace VeteranLogistics.Data.Configurations;

/// <summary>
/// Entity configuration for the Source entity.
/// </summary>
public sealed class SourceConfiguration : IEntityTypeConfiguration<Source>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Source> builder)
    {
        // Configure SourceCode as required and unique
        builder.Property(s => s.SourceCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(s => s.SourceCode)
            .IsUnique();

        // Configure SourceName as required
        builder.Property(s => s.SourceName)
            .IsRequired()
            .HasMaxLength(200);

        // Configure AddressLine1
        builder.Property(s => s.AddressLine1)
            .HasMaxLength(200);

        // Configure AddressLine2
        builder.Property(s => s.AddressLine2)
            .HasMaxLength(200);

        // Configure City
        builder.Property(s => s.City)
            .HasMaxLength(100);

        // Configure State
        builder.Property(s => s.State)
            .HasMaxLength(100);

        // Configure Country
        builder.Property(s => s.Country)
            .HasMaxLength(100);

        // Configure PostalCode
        builder.Property(s => s.PostalCode)
            .HasMaxLength(20);

        // Configure ContactPerson
        builder.Property(s => s.ContactPerson)
            .HasMaxLength(200);

        // Configure PhoneNumber
        builder.Property(s => s.PhoneNumber)
            .HasMaxLength(20);

        // Configure Email
        builder.Property(s => s.Email)
            .HasMaxLength(200);

        // Configure GSTNumber
        builder.Property(s => s.GSTNumber)
            .HasMaxLength(50);

        // Configure Latitude
        builder.Property(s => s.Latitude)
            .HasPrecision(9, 6);

        // Configure Longitude
        builder.Property(s => s.Longitude)
            .HasPrecision(9, 6);

        // Configure Remarks
        builder.Property(s => s.Remarks)
            .HasMaxLength(500);

        // Configure CreatedBy
        builder.Property(s => s.CreatedBy)
            .HasMaxLength(100);

        // Configure ModifiedBy
        builder.Property(s => s.ModifiedBy)
            .HasMaxLength(100);

        // Configure global query filter to automatically exclude soft-deleted sources
        builder.HasQueryFilter(source => !source.IsDeleted);
    }
}
