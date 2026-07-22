using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using veteran_logistic.Masters.DORates.Contracts;
using veteran_logistic.Masters.DORates.Models;
using veteran_logistic.Masters.SourceDestinations.Contracts;
using veteran_logistic.Masters.SourceDestinations.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.DORates.ViewModels;

/// <summary>
/// ViewModel for the Edit DO Rate screen.
/// </summary>
public sealed partial class EditDORateViewModel : ViewModelBase, INavigationAware
{
    private readonly IDORateCommandService _doRateCommandService;
    private readonly IDORateQueryService _doRateQueryService;
    private readonly ISourceDestinationQueryService _sourceDestinationQueryService;
    private readonly IDummyLookupService _dummyLookupService;
    private readonly INavigationService _navigationService;
    private int _doRateId;
    private int _consignorId;
    private int _consigneeId;
    private int _sourceId;
    private int _destinationId;
    private DateTime _effectiveDate;
    private decimal _freightRate;
    private decimal _unionRate;
    private decimal _vendorRate;
    private string _doNumber = string.Empty;
    private decimal _doQty;
    private decimal _billingRate;
    private decimal _allowedShortage;
    private decimal _ratePerKg;
    private string _vesselName = string.Empty;
    private string _traderName = string.Empty;
    private string _narration = string.Empty;
    private bool _isActive;
    private string _validationError = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditDORateViewModel"/> class.
    /// </summary>
    /// <param name="doRateCommandService">The DO rate command service.</param>
    /// <param name="doRateQueryService">The DO rate query service.</param>
    /// <param name="sourceDestinationQueryService">The source destination query service.</param>
    /// <param name="dummyLookupService">The dummy lookup service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public EditDORateViewModel(IDORateCommandService doRateCommandService, IDORateQueryService doRateQueryService, ISourceDestinationQueryService sourceDestinationQueryService, IDummyLookupService dummyLookupService, INavigationService navigationService)
    {
        _doRateCommandService = doRateCommandService ?? throw new ArgumentNullException(nameof(doRateCommandService));
        _doRateQueryService = doRateQueryService ?? throw new ArgumentNullException(nameof(doRateQueryService));
        _sourceDestinationQueryService = sourceDestinationQueryService ?? throw new ArgumentNullException(nameof(sourceDestinationQueryService));
        _dummyLookupService = dummyLookupService ?? throw new ArgumentNullException(nameof(dummyLookupService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        
        Title = "Edit DO Rate";
    }

    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (IsInitialized)
        {
            return;
        }

        await LoadDropdownDataAsync(cancellationToken);
        await LoadDORateAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    /// <summary>
    /// Gets the collection of consignors (dummy data).
    /// </summary>
    public ObservableCollection<LookupItem> Consignors { get; } = new();

    /// <summary>
    /// Gets the collection of consignees (dummy data).
    /// </summary>
    public ObservableCollection<LookupItem> Consignees { get; } = new();

    /// <summary>
    /// Gets the collection of source destinations.
    /// </summary>
    public ObservableCollection<SourceDestinationLookupItem> SourceDestinations { get; } = new();

    /// <summary>
    /// Gets or sets the consignor ID.
    /// </summary>
    public int ConsignorId
    {
        get => _consignorId;
        set => SetProperty(ref _consignorId, value);
    }

    /// <summary>
    /// Gets or sets the consignee ID.
    /// </summary>
    public int ConsigneeId
    {
        get => _consigneeId;
        set => SetProperty(ref _consigneeId, value);
    }

    /// <summary>
    /// Gets or sets the source location ID.
    /// </summary>
    public int SourceId
    {
        get => _sourceId;
        set => SetProperty(ref _sourceId, value);
    }

    /// <summary>
    /// Gets or sets the destination location ID.
    /// </summary>
    public int DestinationId
    {
        get => _destinationId;
        set => SetProperty(ref _destinationId, value);
    }

    /// <summary>
    /// Gets or sets the effective date.
    /// </summary>
    public DateTime EffectiveDate
    {
        get => _effectiveDate;
        set => SetProperty(ref _effectiveDate, value);
    }

    /// <summary>
    /// Gets or sets the freight rate.
    /// </summary>
    public decimal FreightRate
    {
        get => _freightRate;
        set => SetProperty(ref _freightRate, value);
    }

    /// <summary>
    /// Gets or sets the union rate.
    /// </summary>
    public decimal UnionRate
    {
        get => _unionRate;
        set => SetProperty(ref _unionRate, value);
    }

    /// <summary>
    /// Gets or sets the vendor rate.
    /// </summary>
    public decimal VendorRate
    {
        get => _vendorRate;
        set => SetProperty(ref _vendorRate, value);
    }

    /// <summary>
    /// Gets or sets the DO number.
    /// </summary>
    public string DONumber
    {
        get => _doNumber;
        set => SetProperty(ref _doNumber, value);
    }

    /// <summary>
    /// Gets or sets the DO quantity.
    /// </summary>
    public decimal DOQty
    {
        get => _doQty;
        set => SetProperty(ref _doQty, value);
    }

    /// <summary>
    /// Gets or sets the billing rate.
    /// </summary>
    public decimal BillingRate
    {
        get => _billingRate;
        set => SetProperty(ref _billingRate, value);
    }

    /// <summary>
    /// Gets or sets the allowed shortage.
    /// </summary>
    public decimal AllowedShortage
    {
        get => _allowedShortage;
        set => SetProperty(ref _allowedShortage, value);
    }

    /// <summary>
    /// Gets or sets the rate per kg.
    /// </summary>
    public decimal RatePerKg
    {
        get => _ratePerKg;
        set => SetProperty(ref _ratePerKg, value);
    }

    /// <summary>
    /// Gets or sets the vessel name.
    /// </summary>
    public string VesselName
    {
        get => _vesselName;
        set => SetProperty(ref _vesselName, value);
    }

    /// <summary>
    /// Gets or sets the trader name.
    /// </summary>
    public string TraderName
    {
        get => _traderName;
        set => SetProperty(ref _traderName, value);
    }

    /// <summary>
    /// Gets or sets the narration.
    /// </summary>
    public string Narration
    {
        get => _narration;
        set => SetProperty(ref _narration, value);
    }

    /// <summary>
    /// Gets or sets whether the DO rate is active.
    /// </summary>
    public bool IsActive
    {
        get => _isActive;
        set => SetProperty(ref _isActive, value);
    }

    /// <summary>
    /// Gets or sets the validation error message.
    /// </summary>
    public string ValidationError
    {
        get => _validationError;
        set => SetProperty(ref _validationError, value);
    }

    /// <summary>
    /// Loads dropdown data.
    /// </summary>
    private async Task LoadDropdownDataAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            SetBusy("Loading dropdown data...");
            
            // Load sequentially to avoid DbContext concurrency issues
            var sourceDestinations = await _sourceDestinationQueryService.GetAllSourceDestinationsAsync(cancellationToken).ConfigureAwait(false);
            var consignors = await _dummyLookupService.GetConsignorsAsync(cancellationToken).ConfigureAwait(false);
            var consignees = await _dummyLookupService.GetConsigneesAsync(cancellationToken).ConfigureAwait(false);
            
            var dispatcher = System.Windows.Application.Current?.Dispatcher;
            if (dispatcher == null || dispatcher.CheckAccess())
            {
                UpdateSourceDestinationsInternal(sourceDestinations);
                UpdateConsignorsInternal(consignors);
                UpdateConsigneesInternal(consignees);
            }
            else
            {
                dispatcher.Invoke(() =>
                {
                    UpdateSourceDestinationsInternal(sourceDestinations);
                    UpdateConsignorsInternal(consignors);
                    UpdateConsigneesInternal(consignees);
                });
            }
        }
        catch (Exception ex)
        {
            ValidationError = $"Error loading dropdown data: {ex.Message}";
        }
        finally
        {
            ClearBusy();
        }
    }

    private void UpdateConsignorsInternal(IEnumerable<LookupItem> consignors)
    {
        Consignors.Clear();
        foreach (var consignor in consignors)
        {
            Consignors.Add(consignor);
        }
    }

    private void UpdateConsigneesInternal(IEnumerable<LookupItem> consignees)
    {
        Consignees.Clear();
        foreach (var consignee in consignees)
        {
            Consignees.Add(consignee);
        }
    }

    private void UpdateSourceDestinationsInternal(IEnumerable<SourceDestinationListItem> sourceDestinations)
    {
        SourceDestinations.Clear();
        foreach (var sd in sourceDestinations)
        {
            SourceDestinations.Add(new SourceDestinationLookupItem
            {
                Id = sd.Id,
                LocationName = sd.LocationName
            });
        }
    }

    /// <summary>
    /// Loads the DO rate data for editing.
    /// </summary>
    private async Task LoadDORateAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading DO rate...");
        var doRate = await _doRateQueryService.GetDORateForEditAsync(_doRateId, cancellationToken);
        ClearBusy();

        if (doRate != null)
        {
            ConsignorId = doRate.ConsignorId;
            ConsigneeId = doRate.ConsigneeId;
            SourceId = doRate.SourceId;
            DestinationId = doRate.DestinationId;
            EffectiveDate = doRate.EffectiveDate;
            FreightRate = doRate.FreightRate;
            UnionRate = doRate.UnionRate;
            VendorRate = doRate.VendorRate;
            DONumber = doRate.DONumber;
            DOQty = doRate.DOQty;
            BillingRate = doRate.BillingRate;
            AllowedShortage = doRate.AllowedShortage;
            RatePerKg = doRate.RatePerKg;
            VesselName = doRate.VesselName;
            TraderName = doRate.TraderName;
            Narration = doRate.Narration;
            IsActive = doRate.IsActive;
        }
        else
        {
            ValidationError = "DO Rate not found.";
        }
    }

    public void OnNavigatedTo(NavigationParameter? parameter)
    {
        if (parameter is not null && parameter.TryGetValue<int>("DORateId", out var doRateId))
        {
            _doRateId = doRateId;
        }
    }

    /// <summary>
    /// Command to save the DO rate.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidationError = string.Empty;

        var request = new UpdateDORateRequest
        {
            DORateId = _doRateId,
            ConsignorId = ConsignorId,
            ConsigneeId = ConsigneeId,
            SourceId = SourceId,
            DestinationId = DestinationId,
            EffectiveDate = EffectiveDate,
            FreightRate = FreightRate,
            UnionRate = UnionRate,
            VendorRate = VendorRate,
            DONumber = DONumber,
            DOQty = DOQty,
            BillingRate = BillingRate,
            AllowedShortage = AllowedShortage,
            RatePerKg = RatePerKg,
            VesselName = VesselName,
            TraderName = TraderName,
            Narration = Narration,
            IsActive = IsActive
        };

        SetBusy("Updating DO rate...");
        var result = await _doRateCommandService.UpdateDORateAsync(request).ConfigureAwait(false);
        ClearBusy();

        if (result.IsSuccess)
        {
            await _navigationService.GoBackAsync().ConfigureAwait(false);
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to update DO rate.";
        }
    }

    /// <summary>
    /// Command to cancel and navigate back.
    /// </summary>
    [RelayCommand]
    private async Task CancelAsync()
    {
        await _navigationService.GoBackAsync().ConfigureAwait(false);
    }
}
