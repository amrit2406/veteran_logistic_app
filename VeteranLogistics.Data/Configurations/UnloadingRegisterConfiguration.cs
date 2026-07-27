using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VeteranLogistics.Data.Entities.Administration;

namespace VeteranLogistics.Data.Configurations;

/// <summary>
/// Entity configuration for the UnloadingRegister entity.
/// </summary>
public sealed class UnloadingRegisterConfiguration : IEntityTypeConfiguration<UnloadingRegister>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<UnloadingRegister> builder)
    {
        // Configure ChallanNumber as required and unique
        builder.Property(ur => ur.ChallanNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(ur => ur.ChallanNumber)
            .IsUnique();

        // Configure UnloadingDate as required
        builder.Property(ur => ur.UnloadingDate)
            .IsRequired();

        // Configure TPNumber
        builder.Property(ur => ur.TPNumber)
            .HasMaxLength(50);

        // Configure VehicleType
        builder.Property(ur => ur.VehicleType)
            .HasMaxLength(50);

        // Configure VehicleLoadedBy
        builder.Property(ur => ur.VehicleLoadedBy)
            .HasMaxLength(200);

        // Configure DriverCommission
        builder.Property(ur => ur.DriverCommission)
            .HasPrecision(18, 2);

        // Configure GrossWeight
        builder.Property(ur => ur.GrossWeight)
            .HasPrecision(18, 3);

        // Configure TareWeight
        builder.Property(ur => ur.TareWeight)
            .HasPrecision(18, 3);

        // Configure LoadingWeight
        builder.Property(ur => ur.LoadingWeight)
            .HasPrecision(18, 3);

        // Configure Rate
        builder.Property(ur => ur.Rate)
            .HasPrecision(18, 2);

        // Configure GrossAmount
        builder.Property(ur => ur.GrossAmount)
            .HasPrecision(18, 2);

        // Configure FuelQuantity
        builder.Property(ur => ur.FuelQuantity)
            .HasPrecision(18, 3);

        // Configure FuelAmount
        builder.Property(ur => ur.FuelAmount)
            .HasPrecision(18, 2);

        // Configure FuelCash
        builder.Property(ur => ur.FuelCash)
            .HasPrecision(18, 2);

        // Configure FuelAdvance
        builder.Property(ur => ur.FuelAdvance)
            .HasPrecision(18, 2);

        // Configure ShortageWeight
        builder.Property(ur => ur.ShortageWeight)
            .HasPrecision(18, 3);

        // Configure CashAdvance
        builder.Property(ur => ur.CashAdvance)
            .HasPrecision(18, 2);

        // Configure OtherAdvance
        builder.Property(ur => ur.OtherAdvance)
            .HasPrecision(18, 2);

        // Configure ThirdParty
        builder.Property(ur => ur.ThirdParty)
            .HasMaxLength(200);

        // Configure OwnerMobile
        builder.Property(ur => ur.OwnerMobile)
            .HasMaxLength(20);

        // Configure OwnerAddress
        builder.Property(ur => ur.OwnerAddress)
            .HasMaxLength(500);

        // Configure Driver
        builder.Property(ur => ur.Driver)
            .HasMaxLength(200);

        // Configure DrivingLicenceNumber
        builder.Property(ur => ur.DrivingLicenceNumber)
            .HasMaxLength(50);

        // Configure DriverMobile
        builder.Property(ur => ur.DriverMobile)
            .HasMaxLength(20);

        // Configure Notes
        builder.Property(ur => ur.Notes)
            .HasMaxLength(1000);

        // Configure CreatedBy
        builder.Property(ur => ur.CreatedBy)
            .HasMaxLength(100);

        // Configure ModifiedBy
        builder.Property(ur => ur.ModifiedBy)
            .HasMaxLength(100);

        // Configure foreign key relationships
        builder.HasOne(ur => ur.LoadingRegister)
            .WithMany()
            .HasForeignKey(ur => ur.LoadingRegisterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ur => ur.Consignor)
            .WithMany()
            .HasForeignKey(ur => ur.ConsignorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ur => ur.Consignee)
            .WithMany()
            .HasForeignKey(ur => ur.ConsigneeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ur => ur.Source)
            .WithMany()
            .HasForeignKey(ur => ur.SourceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ur => ur.Destination)
            .WithMany()
            .HasForeignKey(ur => ur.DestinationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ur => ur.Vehicle)
            .WithMany()
            .HasForeignKey(ur => ur.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ur => ur.UnionVendor)
            .WithMany()
            .HasForeignKey(ur => ur.UnionVendorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ur => ur.Material)
            .WithMany()
            .HasForeignKey(ur => ur.MaterialId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ur => ur.PaymentLocation)
            .WithMany()
            .HasForeignKey(ur => ur.PaymentLocationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ur => ur.Owner)
            .WithMany()
            .HasForeignKey(ur => ur.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure global query filter to automatically exclude soft-deleted unloading registers
        builder.HasQueryFilter(ur => !ur.IsDeleted);
    }
}
