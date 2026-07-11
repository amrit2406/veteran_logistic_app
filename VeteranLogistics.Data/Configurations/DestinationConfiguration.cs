using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VeteranLogistics.Data.Entities.Administration;

namespace VeteranLogistics.Data.Configurations;

/// <summary>
/// Entity configuration for the Destination entity.
/// </summary>
public sealed class DestinationConfiguration : IEntityTypeConfiguration<Destination>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Destination> builder)
    {
        // Configure DestinationCode as required and unique
        builder.Property(d => d.DestinationCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(d => d.DestinationCode)
            .IsUnique();

        // Configure DestinationName as required
        builder.Property(d => d.DestinationName)
            .IsRequired()
            .HasMaxLength(200);

        // Configure AddressLine1
        builder.Property(d => d.AddressLine1)
            .HasMaxLength(200);

        // Configure AddressLine2
        builder.Property(d => d.AddressLine2)
            .HasMaxLength(200);

        // Configure City
        builder.Property(d => d.City)
            .HasMaxLength(100);

        // Configure State
        builder.Property(d => d.State)
            .HasMaxLength(100);

        // Configure Country
        builder.Property(d => d.Country)
            .HasMaxLength(100);

        // Configure PostalCode
        builder.Property(d => d.PostalCode)
            .HasMaxLength(20);

        // Configure ContactPerson
        builder.Property(d => d.ContactPerson)
            .HasMaxLength(200);

        // Configure PhoneNumber
        builder.Property(d => d.PhoneNumber)
            .HasMaxLength(20);

        // Configure Email
        builder.Property(d => d.Email)
            .HasMaxLength(200);

        // Configure GSTNumber
        builder.Property(d => d.GSTNumber)
            .HasMaxLength(50);

        // Configure Latitude
        builder.Property(d => d.Latitude)
            .HasPrecision(9, 6);

        // Configure Longitude
        builder.Property(d => d.Longitude)
            .HasPrecision(9, 6);

        // Configure Remarks
        builder.Property(d => d.Remarks)
            .HasMaxLength(500);

        // Configure CreatedBy
        builder.Property(d => d.CreatedBy)
            .HasMaxLength(100);

        // Configure ModifiedBy
        builder.Property(d => d.ModifiedBy)
            .HasMaxLength(100);

        // Configure global query filter to automatically exclude soft-deleted destinations
        builder.HasQueryFilter(destination => !destination.IsDeleted);
    }
}
