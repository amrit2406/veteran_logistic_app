using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace VeteranLogistics.Data.Context;

/// <summary>
/// Factory for creating DbContext instances at design time (for EF Core migrations).
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<VeteranLogisticsDbContext>
{
    public VeteranLogisticsDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<VeteranLogisticsDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new VeteranLogisticsDbContext(optionsBuilder.Options);
    }
}
