using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VeteranLogistics.Data.Entities.Administration;

namespace VeteranLogistics.Data.Configurations;

/// <summary>
/// Entity configuration for the PartyBillRegister entity.
/// </summary>
public sealed class PartyBillRegisterConfiguration : IEntityTypeConfiguration<PartyBillRegister>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<PartyBillRegister> builder)
    {
        // Configure BillNumber as required and unique
        builder.Property(pbr => pbr.BillNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(pbr => pbr.BillNumber)
            .IsUnique();

        // Configure BillDate as required
        builder.Property(pbr => pbr.BillDate)
            .IsRequired();

        // Configure PartyId as required
        builder.Property(pbr => pbr.PartyId)
            .IsRequired();

        // Configure ThirdPartyName
        builder.Property(pbr => pbr.ThirdPartyName)
            .HasMaxLength(200);

        // Configure PermitNumber
        builder.Property(pbr => pbr.PermitNumber)
            .HasMaxLength(50);

        // Configure TotalRecords
        builder.Property(pbr => pbr.TotalRecords)
            .IsRequired();

        // Configure TotalMaterialWeight
        builder.Property(pbr => pbr.TotalMaterialWeight)
            .HasPrecision(18, 3)
            .IsRequired();

        // Configure TotalAmount
        builder.Property(pbr => pbr.TotalAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        // Configure ChargeHead1
        builder.Property(pbr => pbr.ChargeHead1)
            .HasMaxLength(100);

        // Configure ChargeType1
        builder.Property(pbr => pbr.ChargeType1)
            .HasMaxLength(50);

        // Configure ChargeAmount1
        builder.Property(pbr => pbr.ChargeAmount1)
            .HasPrecision(18, 2);

        // Configure ChargeHead2
        builder.Property(pbr => pbr.ChargeHead2)
            .HasMaxLength(100);

        // Configure ChargeType2
        builder.Property(pbr => pbr.ChargeType2)
            .HasMaxLength(50);

        // Configure ChargeAmount2
        builder.Property(pbr => pbr.ChargeAmount2)
            .HasPrecision(18, 2);

        // Configure GrandTotal
        builder.Property(pbr => pbr.GrandTotal)
            .HasPrecision(18, 2)
            .IsRequired();

        // Configure Remarks
        builder.Property(pbr => pbr.Remarks)
            .HasMaxLength(1000);

        // Configure CreatedBy
        builder.Property(pbr => pbr.CreatedBy)
            .HasMaxLength(100);

        // Configure ModifiedBy
        builder.Property(pbr => pbr.ModifiedBy)
            .HasMaxLength(100);

        // Configure foreign key relationships
        builder.HasOne(pbr => pbr.Party)
            .WithMany()
            .HasForeignKey(pbr => pbr.PartyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pbr => pbr.Consignor)
            .WithMany()
            .HasForeignKey(pbr => pbr.ConsignorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pbr => pbr.Destination)
            .WithMany()
            .HasForeignKey(pbr => pbr.DestinationId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure global query filter to automatically exclude soft-deleted party bill registers
        builder.HasQueryFilter(pbr => !pbr.IsDeleted);
    }
}
