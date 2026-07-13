using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VeteranLogistics.Data.Entities.Administration;

namespace VeteranLogistics.Data.Configurations;

/// <summary>
/// Entity configuration for the PaymentLocation entity.
/// </summary>
public sealed class PaymentLocationConfiguration : IEntityTypeConfiguration<PaymentLocation>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<PaymentLocation> builder)
    {
        // Configure PaymentLocationName as required and unique
        builder.Property(p => p.PaymentLocationName)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(p => p.PaymentLocationName)
            .IsUnique();

        // Configure CreatedBy
        builder.Property(p => p.CreatedBy)
            .HasMaxLength(100);

        // Configure ModifiedBy
        builder.Property(p => p.ModifiedBy)
            .HasMaxLength(100);

        // Configure global query filter to automatically exclude soft-deleted payment locations
        builder.HasQueryFilter(paymentLocation => !paymentLocation.IsDeleted);
    }
}
