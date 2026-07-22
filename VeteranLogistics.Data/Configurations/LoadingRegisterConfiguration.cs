using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VeteranLogistics.Data.Entities.Administration;

namespace VeteranLogistics.Data.Configurations;

/// <summary>
/// Entity configuration for the LoadingRegister entity.
/// </summary>
public sealed class LoadingRegisterConfiguration : IEntityTypeConfiguration<LoadingRegister>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<LoadingRegister> builder)
    {
        // Configure ChallanNumber as required and unique
        builder.Property(lr => lr.ChallanNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(lr => lr.ChallanNumber)
            .IsUnique();

        // Configure LoadingDate as required
        builder.Property(lr => lr.LoadingDate)
            .IsRequired();

        // Configure TPNumber
        builder.Property(lr => lr.TPNumber)
            .HasMaxLength(50);

        // Configure VehicleType
        builder.Property(lr => lr.VehicleType)
            .HasMaxLength(50);

        // Configure VehicleLoadedBy
        builder.Property(lr => lr.VehicleLoadedBy)
            .HasMaxLength(200);

        // Configure DriverCommission
        builder.Property(lr => lr.DriverCommission)
            .HasPrecision(18, 2);

        // Configure GrossWeight
        builder.Property(lr => lr.GrossWeight)
            .HasPrecision(18, 3);

        // Configure TareWeight
        builder.Property(lr => lr.TareWeight)
            .HasPrecision(18, 3);

        // Configure LoadingWeight
        builder.Property(lr => lr.LoadingWeight)
            .HasPrecision(18, 3);

        // Configure Rate
        builder.Property(lr => lr.Rate)
            .HasPrecision(18, 2);

        // Configure GrossAmount
        builder.Property(lr => lr.GrossAmount)
            .HasPrecision(18, 2);

        // Configure FuelQuantity
        builder.Property(lr => lr.FuelQuantity)
            .HasPrecision(18, 3);

        // Configure FuelAmount
        builder.Property(lr => lr.FuelAmount)
            .HasPrecision(18, 2);

        // Configure FuelCash
        builder.Property(lr => lr.FuelCash)
            .HasPrecision(18, 2);

        // Configure FuelAdvance
        builder.Property(lr => lr.FuelAdvance)
            .HasPrecision(18, 2);

        // Configure ShortageWeight
        builder.Property(lr => lr.ShortageWeight)
            .HasPrecision(18, 3);

        // Configure CashAdvance
        builder.Property(lr => lr.CashAdvance)
            .HasPrecision(18, 2);

        // Configure OtherAdvance
        builder.Property(lr => lr.OtherAdvance)
            .HasPrecision(18, 2);

        // Configure ThirdParty
        builder.Property(lr => lr.ThirdParty)
            .HasMaxLength(200);

        // Configure OwnerMobile
        builder.Property(lr => lr.OwnerMobile)
            .HasMaxLength(20);

        // Configure OwnerAddress
        builder.Property(lr => lr.OwnerAddress)
            .HasMaxLength(500);

        // Configure Driver
        builder.Property(lr => lr.Driver)
            .HasMaxLength(200);

        // Configure DrivingLicenceNumber
        builder.Property(lr => lr.DrivingLicenceNumber)
            .HasMaxLength(50);

        // Configure DriverMobile
        builder.Property(lr => lr.DriverMobile)
            .HasMaxLength(20);

        // Configure Notes
        builder.Property(lr => lr.Notes)
            .HasMaxLength(1000);

        // Configure CreatedBy
        builder.Property(lr => lr.CreatedBy)
            .HasMaxLength(100);

        // Configure ModifiedBy
        builder.Property(lr => lr.ModifiedBy)
            .HasMaxLength(100);

        // Configure foreign key relationships
        builder.HasOne(lr => lr.Consignor)
            .WithMany()
            .HasForeignKey(lr => lr.ConsignorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(lr => lr.Consignee)
            .WithMany()
            .HasForeignKey(lr => lr.ConsigneeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(lr => lr.Source)
            .WithMany()
            .HasForeignKey(lr => lr.SourceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(lr => lr.Destination)
            .WithMany()
            .HasForeignKey(lr => lr.DestinationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(lr => lr.Vehicle)
            .WithMany()
            .HasForeignKey(lr => lr.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(lr => lr.UnionVendor)
            .WithMany()
            .HasForeignKey(lr => lr.UnionVendorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(lr => lr.Material)
            .WithMany()
            .HasForeignKey(lr => lr.MaterialId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(lr => lr.PaymentLocation)
            .WithMany()
            .HasForeignKey(lr => lr.PaymentLocationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(lr => lr.Owner)
            .WithMany()
            .HasForeignKey(lr => lr.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure global query filter to automatically exclude soft-deleted loading registers
        builder.HasQueryFilter(lr => !lr.IsDeleted);
    }
}
