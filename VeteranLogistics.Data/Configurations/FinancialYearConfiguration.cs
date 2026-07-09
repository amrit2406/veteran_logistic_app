using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VeteranLogistics.Data.Entities.Administration;

namespace VeteranLogistics.Data.Configurations;

/// <summary>
/// Entity configuration for the FinancialYear entity.
/// </summary>
public sealed class FinancialYearConfiguration : IEntityTypeConfiguration<FinancialYear>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<FinancialYear> builder)
    {
        // Configure Name as required and unique
        builder.Property(fy => fy.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(fy => fy.Name)
            .IsUnique();

        // Configure StartDate as required
        builder.Property(fy => fy.StartDate)
            .IsRequired();

        // Configure EndDate as required
        builder.Property(fy => fy.EndDate)
            .IsRequired();

        // Configure global query filter to automatically exclude soft-deleted financial years
        builder.HasQueryFilter(financialYear => !financialYear.IsDeleted);
    }
}
