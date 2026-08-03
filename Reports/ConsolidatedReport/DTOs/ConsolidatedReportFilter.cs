using CommunityToolkit.Mvvm.ComponentModel;

namespace veteran_logistic.Reports.ConsolidatedReport.DTOs;

/// <summary>
/// Represents filter criteria for the consolidated report.
/// </summary>
public sealed partial class ConsolidatedReportFilter : ObservableObject
{
    // Date Filters
    /// <summary>
    /// Gets or sets the date range start.
    /// </summary>
    [ObservableProperty]
    private DateTime? _dateFrom;

    /// <summary>
    /// Gets or sets the date range end.
    /// </summary>
    [ObservableProperty]
    private DateTime? _dateTo;

    // Entity Filters
    /// <summary>
    /// Gets or sets the consignor ID filter.
    /// </summary>
    [ObservableProperty]
    private int? _consignorId;

    /// <summary>
    /// Gets or sets the consignee ID filter.
    /// </summary>
    [ObservableProperty]
    private int? _consigneeId;

    /// <summary>
    /// Gets or sets the source ID filter.
    /// </summary>
    [ObservableProperty]
    private int? _sourceId;

    /// <summary>
    /// Gets or sets the destination ID filter.
    /// </summary>
    [ObservableProperty]
    private int? _destinationId;

    /// <summary>
    /// Gets or sets the SAR (Status And Records) filter.
    /// </summary>
    [ObservableProperty]
    private string? _sARFilter;

    /// <summary>
    /// Determines whether the filter has any active criteria.
    /// </summary>
    public bool HasFilter =>
        DateFrom.HasValue ||
        DateTo.HasValue ||
        ConsignorId.HasValue ||
        ConsigneeId.HasValue ||
        SourceId.HasValue ||
        DestinationId.HasValue ||
        !string.IsNullOrWhiteSpace(SARFilter);

    /// <summary>
    /// Clears all filter criteria.
    /// </summary>
    public void Clear()
    {
        DateFrom = null;
        DateTo = null;
        ConsignorId = null;
        ConsigneeId = null;
        SourceId = null;
        DestinationId = null;
        SARFilter = null;
        OnPropertyChanged(nameof(HasFilter));
    }

    partial void OnDateFromChanged(DateTime? value) => OnPropertyChanged(nameof(HasFilter));
    partial void OnDateToChanged(DateTime? value) => OnPropertyChanged(nameof(HasFilter));
    partial void OnConsignorIdChanged(int? value) => OnPropertyChanged(nameof(HasFilter));
    partial void OnConsigneeIdChanged(int? value) => OnPropertyChanged(nameof(HasFilter));
    partial void OnSourceIdChanged(int? value) => OnPropertyChanged(nameof(HasFilter));
    partial void OnDestinationIdChanged(int? value) => OnPropertyChanged(nameof(HasFilter));
    partial void OnSARFilterChanged(string? value) => OnPropertyChanged(nameof(HasFilter));
}
