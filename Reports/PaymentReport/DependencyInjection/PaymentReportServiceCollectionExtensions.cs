using Microsoft.Extensions.DependencyInjection;
using veteran_logistic.Reports.PaymentReport.Contracts;
using veteran_logistic.Reports.PaymentReport.Services;
using veteran_logistic.Reports.PaymentReport.ViewModels;
using veteran_logistic.Reports.PaymentReport.Export.Pdf;
using veteran_logistic.Reports.PaymentReport.Export.Excel;

namespace veteran_logistic.Reports.PaymentReport.DependencyInjection;

/// <summary>
/// Extension methods for registering Payment Report feature infrastructure.
/// </summary>
public static class PaymentReportServiceCollectionExtensions
{
    /// <summary>
    /// Adds Payment Report feature infrastructure to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddPaymentReport(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Query service
        services.AddScoped<IPaymentReportQueryService, PaymentReportQueryService>();

        // Export services
        services.AddScoped<IPaymentReportPdfExporter, PaymentReportPdfExporter>();
        services.AddScoped<IPaymentReportExcelExporter, PaymentReportExcelExporter>();

        // ViewModel
        services.AddTransient<PaymentReportViewModel>();

        return services;
    }
}
