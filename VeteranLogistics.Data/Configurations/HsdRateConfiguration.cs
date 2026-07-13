using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VeteranLogistics.Data.Entities.Administration;

namespace VeteranLogistics.Data.Configurations;

/// <summary>
/// Entity configuration for the HsdRate entity.
/// </summary>
public sealed class HsdRateConfiguration : IEntityTypeConfiguration<HsdRate>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<HsdRate> builder)
    {
        // Configure FuelPump relationship
        builder.HasOne(h => h.FuelPump)
            .WithMany()
            .HasForeignKey(h => h.FuelPumpId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure ApplicableDate as required
        builder.Property(h => h.ApplicableDate)
            .IsRequired();

        // Configure RatePerLitre as required with decimal(18,2)
        builder.Property(h => h.RatePerLitre)
            .IsRequired()
            .HasPrecision(18, 2);

        // Configure CreatedBy
        builder.Property(h => h.CreatedBy)
            .HasMaxLength(100);

        // Configure ModifiedBy
        builder.Property(h => h.ModifiedBy)
            .HasMaxLength(100);

        // Configure unique index on FuelPumpId + ApplicableDate to prevent duplicates
        builder.HasIndex(h => new { h.FuelPumpId, h.ApplicableDate })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        // Configure global query filter to automatically exclude soft-deleted HSD rates
        builder.HasQueryFilter(hsdRate => !hsdRate.IsDeleted);
    }
}
