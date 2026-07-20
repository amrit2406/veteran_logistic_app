using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VeteranLogistics.Data.Entities.Administration;

namespace VeteranLogistics.Data.Configurations;

/// <summary>
/// Entity configuration for the DORate entity.
/// </summary>
public sealed class DORateConfiguration : IEntityTypeConfiguration<DORate>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<DORate> builder)
    {
        // Configure ConsignorId
        builder.Property(d => d.ConsignorId)
            .IsRequired();

        // Configure ConsigneeId
        builder.Property(d => d.ConsigneeId)
            .IsRequired();

        // Configure SourceId
        builder.Property(d => d.SourceId)
            .IsRequired();

        // Configure DestinationId
        builder.Property(d => d.DestinationId)
            .IsRequired();

        // Configure EffectiveDate
        builder.Property(d => d.EffectiveDate)
            .IsRequired()
            .HasColumnType("date");

        // Configure FreightRate with decimal precision
        builder.Property(d => d.FreightRate)
            .IsRequired()
            .HasPrecision(18, 2);

        // Configure UnionRate with decimal precision
        builder.Property(d => d.UnionRate)
            .IsRequired()
            .HasPrecision(18, 2);

        // Configure VendorRate with decimal precision
        builder.Property(d => d.VendorRate)
            .IsRequired()
            .HasPrecision(18, 2);

        // Configure DONumber
        builder.Property(d => d.DONumber)
            .IsRequired()
            .HasMaxLength(50);

        // Configure BillingRate with decimal precision
        builder.Property(d => d.BillingRate)
            .IsRequired()
            .HasPrecision(18, 2);

        // Configure AllowedShortage with decimal precision
        builder.Property(d => d.AllowedShortage)
            .IsRequired()
            .HasPrecision(18, 2);

        // Configure RatePerKg with decimal precision
        builder.Property(d => d.RatePerKg)
            .IsRequired()
            .HasPrecision(18, 2);

        // Configure VesselName
        builder.Property(d => d.VesselName)
            .HasMaxLength(200);

        // Configure TraderName
        builder.Property(d => d.TraderName)
            .HasMaxLength(200);

        // Configure Narration
        builder.Property(d => d.Narration)
            .HasMaxLength(1000);

        // Configure CreatedBy
        builder.Property(d => d.CreatedBy)
            .HasMaxLength(100);

        // Configure ModifiedBy
        builder.Property(d => d.ModifiedBy)
            .HasMaxLength(100);

        // Configure foreign key to Source
        builder.HasOne(d => d.Source)
            .WithMany()
            .HasForeignKey(d => d.SourceId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure foreign key to Destination
        builder.HasOne(d => d.Destination)
            .WithMany()
            .HasForeignKey(d => d.DestinationId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure unique index to prevent duplicate active DO setups
        builder.HasIndex(d => new { d.SourceId, d.DestinationId, d.EffectiveDate, d.DONumber })
            .HasFilter("IsDeleted = 0 AND IsActive = 1")
            .IsUnique();

        // Configure global query filter to automatically exclude soft-deleted DO rates
        builder.HasQueryFilter(doRate => !doRate.IsDeleted);
    }
}
