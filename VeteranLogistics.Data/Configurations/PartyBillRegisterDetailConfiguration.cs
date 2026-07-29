using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VeteranLogistics.Data.Entities.Administration;

namespace VeteranLogistics.Data.Configurations;

/// <summary>
/// Entity configuration for the PartyBillRegisterDetail entity.
/// </summary>
public sealed class PartyBillRegisterDetailConfiguration : IEntityTypeConfiguration<PartyBillRegisterDetail>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<PartyBillRegisterDetail> builder)
    {
        // Configure PartyBillRegisterId as required
        builder.Property(pbrd => pbrd.PartyBillRegisterId)
            .IsRequired();

        // Configure LoadingRegisterId as required
        builder.Property(pbrd => pbrd.LoadingRegisterId)
            .IsRequired();

        // Configure TPNumber
        builder.Property(pbrd => pbrd.TPNumber)
            .HasMaxLength(50);

        // Configure ChallanNumber
        builder.Property(pbrd => pbrd.ChallanNumber)
            .HasMaxLength(50);

        // Configure VehicleNumber
        builder.Property(pbrd => pbrd.VehicleNumber)
            .HasMaxLength(50);

        // Configure LoadingDate as required
        builder.Property(pbrd => pbrd.LoadingDate)
            .IsRequired();

        // Configure MaterialWeight
        builder.Property(pbrd => pbrd.MaterialWeight)
            .HasPrecision(18, 3)
            .IsRequired();

        // Configure BillingRate
        builder.Property(pbrd => pbrd.BillingRate)
            .HasPrecision(18, 2)
            .IsRequired();

        // Configure DriverCommission
        builder.Property(pbrd => pbrd.DriverCommission)
            .HasPrecision(18, 2)
            .IsRequired();

        // Configure Amount
        builder.Property(pbrd => pbrd.Amount)
            .HasPrecision(18, 2)
            .IsRequired();

        // Configure foreign key relationships
        builder.HasOne(pbrd => pbrd.PartyBillRegister)
            .WithMany(pbr => pbr.PartyBillRegisterDetails)
            .HasForeignKey(pbrd => pbrd.PartyBillRegisterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pbrd => pbrd.LoadingRegister)
            .WithMany()
            .HasForeignKey(pbrd => pbrd.LoadingRegisterId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure global query filter to automatically exclude soft-deleted party bill register details
        builder.HasQueryFilter(pbrd => !pbrd.IsDeleted);
    }
}
