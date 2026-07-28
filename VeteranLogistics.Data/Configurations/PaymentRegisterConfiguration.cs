using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VeteranLogistics.Data.Entities.Administration;

namespace VeteranLogistics.Data.Configurations;

/// <summary>
/// Entity configuration for the PaymentRegister entity.
/// </summary>
public sealed class PaymentRegisterConfiguration : IEntityTypeConfiguration<PaymentRegister>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<PaymentRegister> builder)
    {
        // Configure ChallanNumber as required
        builder.Property(pr =>	pr.ChallanNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(pr => pr.ChallanNumber);

        // Configure TPNumber
        builder.Property(pr => pr.TPNumber)
            .HasMaxLength(50);

        // Configure VehicleNumber
        builder.Property(pr => pr.VehicleNumber)
            .HasMaxLength(50);

        // Configure VehicleType
        builder.Property(pr => pr.VehicleType)
            .HasMaxLength(50);

        // Configure MaterialName
        builder.Property(pr => pr.MaterialName)
            .HasMaxLength(200);

        // Configure DriverCommission
        builder.Property(pr => pr.DriverCommission)
            .HasPrecision(18, 2);

        // Configure LoadingWeight
        builder.Property(pr => pr.LoadingWeight)
            .HasPrecision(18, 3);

        // Configure UnloadingWeight
        builder.Property(pr => pr.UnloadingWeight)
            .HasPrecision(18, 3);

        // Configure PaymentDate as required
        builder.Property(pr => pr.PaymentDate)
            .IsRequired();

        // Configure PaymentType
        builder.Property(pr => pr.PaymentType)
            .IsRequired()
            .HasMaxLength(50);

        // Configure HSDParty
        builder.Property(pr => pr.HSDParty)
            .HasMaxLength(200);

        // Configure Notes
        builder.Property(pr => pr.Notes)
            .HasMaxLength(1000);

        // Configure Beneficiary
        builder.Property(pr => pr.Beneficiary)
            .HasMaxLength(200);

        // Configure PAN
        builder.Property(pr => pr.PAN)
            .HasMaxLength(20);

        // Configure UTRNumber
        builder.Property(pr => pr.UTRNumber)
            .HasMaxLength(50);

        // Configure MobileNumber
        builder.Property(pr => pr.MobileNumber)
            .HasMaxLength(20);

        // Configure AccountNumber
        builder.Property(pr => pr.AccountNumber)
            .HasMaxLength(50);

        // Configure IFSCCode
        builder.Property(pr => pr.IFSCCode)
            .HasMaxLength(20);

        // Configure BankName
        builder.Property(pr => pr.BankName)
            .HasMaxLength(200);

        // Configure TDSPercentage
        builder.Property(pr => pr.TDSPercentage)
            .HasPrecision(5, 2);

        // Configure ChallanMoney
        builder.Property(pr => pr.ChallanMoney)
            .HasPrecision(18, 2);

        // Configure Surcharge
        builder.Property(pr => pr.Surcharge)
            .HasPrecision(18, 2);

        // Configure AdminCharge
        builder.Property(pr => pr.AdminCharge)
            .HasPrecision(18, 2);

        // Configure GrossAmount
        builder.Property(pr => pr.GrossAmount)
            .HasPrecision(18, 2);

        // Configure PayableAmount
        builder.Property(pr => pr.PayableAmount)
            .HasPrecision(18, 2);

        // Configure PaymentStatus
        builder.Property(pr => pr.PaymentStatus)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("Pending");

        builder.HasIndex(pr => pr.PaymentStatus);

        // Configure CreatedBy
        builder.Property(pr => pr.CreatedBy)
            .HasMaxLength(100);

        // Configure ModifiedBy
        builder.Property(pr => pr.ModifiedBy)
            .HasMaxLength(100);

        // Configure foreign key relationships
        builder.HasOne(pr => pr.LoadingRegister)
            .WithMany()
            .HasForeignKey(pr => pr.LoadingRegisterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pr => pr.UnloadingRegister)
            .WithMany()
            .HasForeignKey(pr => pr.UnloadingRegisterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pr => pr.PaymentLocation)
            .WithMany()
            .HasForeignKey(pr => pr.PaymentLocationId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure global query filter to automatically exclude soft-deleted payment registers
        builder.HasQueryFilter(pr => !pr.IsDeleted);
    }
}
