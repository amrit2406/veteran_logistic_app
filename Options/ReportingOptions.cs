namespace veteran_logistic.Configuration.Options;

/// <summary>
/// Reporting configuration placeholders bound to the "Reporting" section.
/// </summary>
public sealed class ReportingOptions
{
    /// <summary>
    /// Directory where exported reports will be written. Should be configurable per environment.
    /// </summary>
    public string ExportDirectory { get; set; } = "./Exports";

    /// <summary>
    /// Default export format (e.g. PDF, Excel, CSV).
    /// </summary>
    public string DefaultExportFormat { get; set; } = "PDF";

    /// <summary>
    /// Enable PDF export.
    /// </summary>
    public bool EnablePdfExport { get; set; } = true;

    /// <summary>
    /// Enable Excel export.
    /// </summary>
    public bool EnableExcelExport { get; set; } = true;
}
