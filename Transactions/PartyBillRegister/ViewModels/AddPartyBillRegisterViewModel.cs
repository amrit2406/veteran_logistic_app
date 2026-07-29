using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using veteran_logistic.Transactions.PartyBillRegister.Contracts;
using veteran_logistic.Transactions.PartyBillRegister.Models;
using veteran_logistic.Masters.Customers.Contracts;
using veteran_logistic.Masters.Customers.Models;
using veteran_logistic.Masters.SourceDestinations.Contracts;
using veteran_logistic.Masters.SourceDestinations.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;
using VeteranLogistics.Shared.Validation;

namespace veteran_logistic.Transactions.PartyBillRegister.ViewModels;

/// <summary>
/// ViewModel for the Add Party Bill Register screen.
/// </summary>
public sealed partial class AddPartyBillRegisterViewModel : ViewModelBase
{
    private readonly IPartyBillRegisterQueryService _partyBillRegisterQueryService;
    private readonly IPartyBillRegisterCommandService _partyBillRegisterCommandService;
    private readonly ICreatePartyBillRegisterValidator _createPartyBillRegisterValidator;
    private readonly INavigationService _navigationService;
    private readonly ICustomerQueryService _customerQueryService;
    private readonly ISourceDestinationQueryService _sourceDestinationQueryService;
    private string _validationError = string.Empty;
    private bool _selectAll = false;
    private IReadOnlyList<CustomerListItem> _customers = [];
    private IReadOnlyList<SourceDestinationListItem> _sourceDestinations = [];

    // Header fields
    private string _billNumber = string.Empty;
    private DateTime _billDate = DateTime.Today;
    private int _partyId;
    private string _partyName = string.Empty;
    private string _thirdPartyName = string.Empty;
    private string _permitNumber = string.Empty;
    private int? _consignorId;
    private string _consignorName = string.Empty;
    private int? _destinationId;
    private string _destinationName = string.Empty;
    private DateTime? _fromDate;
    private DateTime? _toDate;

    // Remarks
    private string _remarks = string.Empty;

    // Calculated totals
    private int _totalRecords;
    private decimal _totalMaterialWeight;
    private decimal _totalAmount;
    private decimal _grandTotal;

    /// <summary>
    /// Command to navigate back to the previous screen.
    /// </summary>
    public IAsyncRelayCommand GoBackCommand { get; }

    /// <summary>
    /// Whether it's possible to go back in navigation history.
    /// </summary>
    public bool CanGoBack => _navigationService.CanGoBack;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddPartyBillRegisterViewModel"/> class.
    /// </summary>
    /// <param name="partyBillRegisterQueryService">The party bill register query service.</param>
    /// <param name="partyBillRegisterCommandService">The party bill register command service.</param>
    /// <param name="createPartyBillRegisterValidator">The create party bill register validator.</param>
    /// <param name="navigationService">The navigation service.</param>
    /// <param name="customerQueryService">The customer query service.</param>
    /// <param name="sourceDestinationQueryService">The source destination query service.</param>
    public AddPartyBillRegisterViewModel(IPartyBillRegisterQueryService partyBillRegisterQueryService, IPartyBillRegisterCommandService partyBillRegisterCommandService, ICreatePartyBillRegisterValidator createPartyBillRegisterValidator, INavigationService navigationService, ICustomerQueryService customerQueryService, ISourceDestinationQueryService sourceDestinationQueryService)
    {
        _partyBillRegisterQueryService = partyBillRegisterQueryService ?? throw new ArgumentNullException(nameof(partyBillRegisterQueryService));
        _partyBillRegisterCommandService = partyBillRegisterCommandService ?? throw new ArgumentNullException(nameof(partyBillRegisterCommandService));
        _createPartyBillRegisterValidator = createPartyBillRegisterValidator ?? throw new ArgumentNullException(nameof(createPartyBillRegisterValidator));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        _customerQueryService = customerQueryService ?? throw new ArgumentNullException(nameof(customerQueryService));
        _sourceDestinationQueryService = sourceDestinationQueryService ?? throw new ArgumentNullException(nameof(sourceDestinationQueryService));

        Title = "Add Party Bill Register";
        GoBackCommand = new AsyncRelayCommand(ExecuteGoBackAsync, () => CanGoBack);
    }

    private async Task ExecuteGoBackAsync()
    {
        await _navigationService.GoBackAsync();
        GoBackCommand.NotifyCanExecuteChanged();
    }

    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (IsInitialized)
        {
            return;
        }

        await LoadMasterDataAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher != null && !dispatcher.CheckAccess())
        {
            dispatcher.Invoke(() =>
            {
                GoBackCommand.NotifyCanExecuteChanged();
                OnPropertyChanged(nameof(CanGoBack));
            });
        }
        else
        {
            GoBackCommand.NotifyCanExecuteChanged();
            OnPropertyChanged(nameof(CanGoBack));
        }
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
    /// Gets the collection of eligible loading registers.
    /// </summary>
    public ObservableCollection<EligibleLoadingRegisterModel> EligibleLoadingRegisters { get; } = new();

    /// <summary>
    /// Gets or sets the collection of customers for dropdowns.
    /// </summary>
    public IReadOnlyList<CustomerListItem> Customers
    {
        get => _customers;
        private set => SetProperty(ref _customers, value);
    }

    /// <summary>
    /// Gets or sets the collection of source/destinations for dropdowns.
    /// </summary>
    public IReadOnlyList<SourceDestinationListItem> SourceDestinations
    {
        get => _sourceDestinations;
        private set => SetProperty(ref _sourceDestinations, value);
    }

    /// <summary>
    /// Gets or sets the bill number.
    /// </summary>
    public string BillNumber
    {
        get => _billNumber;
        set => SetProperty(ref _billNumber, value);
    }

    /// <summary>
    /// Gets or sets the bill date.
    /// </summary>
    public DateTime BillDate
    {
        get => _billDate;
        set
        {
            if (SetProperty(ref _billDate, value))
            {
                OnPropertyChanged(nameof(BillDate));
            }
        }
    }

    /// <summary>
    /// Gets or sets the party ID.
    /// </summary>
    public int PartyId
    {
        get => _partyId;
        set => SetProperty(ref _partyId, value);
    }

    /// <summary>
    /// Gets or sets the party name.
    /// </summary>
    public string PartyName
    {
        get => _partyName;
        set => SetProperty(ref _partyName, value);
    }

    /// <summary>
    /// Gets or sets the third party name.
    /// </summary>
    public string ThirdPartyName
    {
        get => _thirdPartyName;
        set => SetProperty(ref _thirdPartyName, value);
    }

    /// <summary>
    /// Gets or sets the permit number.
    /// </summary>
    public string PermitNumber
    {
        get => _permitNumber;
        set => SetProperty(ref _permitNumber, value);
    }

    /// <summary>
    /// Gets or sets the consignor ID.
    /// </summary>
    public int? ConsignorId
    {
        get => _consignorId;
        set => SetProperty(ref _consignorId, value);
    }

    /// <summary>
    /// Gets or sets the consignor name.
    /// </summary>
    public string ConsignorName
    {
        get => _consignorName;
        set => SetProperty(ref _consignorName, value);
    }

    /// <summary>
    /// Gets or sets the destination ID.
    /// </summary>
    public int? DestinationId
    {
        get => _destinationId;
        set => SetProperty(ref _destinationId, value);
    }

    /// <summary>
    /// Gets or sets the destination name.
    /// </summary>
    public string DestinationName
    {
        get => _destinationName;
        set => SetProperty(ref _destinationName, value);
    }

    /// <summary>
    /// Gets or sets the from date.
    /// </summary>
    public DateTime? FromDate
    {
        get => _fromDate;
        set => SetProperty(ref _fromDate, value);
    }

    /// <summary>
    /// Gets or sets the to date.
    /// </summary>
    public DateTime? ToDate
    {
        get => _toDate;
        set => SetProperty(ref _toDate, value);
    }

    /// <summary>
    /// Gets or sets the remarks.
    /// </summary>
    public string Remarks
    {
        get => _remarks;
        set => SetProperty(ref _remarks, value);
    }

    /// <summary>
    /// Gets or sets whether to select all loading registers.
    /// </summary>
    public bool SelectAll
    {
        get => _selectAll;
        set
        {
            if (SetProperty(ref _selectAll, value))
            {
                foreach (var item in EligibleLoadingRegisters)
                {
                    item.IsSelected = value;
                }
                CalculateTotals();
            }
        }
    }

    /// <summary>
    /// Gets the total records.
    /// </summary>
    public int TotalRecords
    {
        get => _totalRecords;
        private set => SetProperty(ref _totalRecords, value);
    }

    /// <summary>
    /// Gets the total material weight.
    /// </summary>
    public decimal TotalMaterialWeight
    {
        get => _totalMaterialWeight;
        private set => SetProperty(ref _totalMaterialWeight, value);
    }

    /// <summary>
    /// Gets the total amount.
    /// </summary>
    public decimal TotalAmount
    {
        get => _totalAmount;
        private set => SetProperty(ref _totalAmount, value);
    }

    /// <summary>
    /// Gets the grand total.
    /// </summary>
    public decimal GrandTotal
    {
        get => _grandTotal;
        private set => SetProperty(ref _grandTotal, value);
    }

    /// <summary>
    /// Command to search eligible loading registers based on filters.
    /// </summary>
    [RelayCommand]
    private async Task SearchEligibleLoadingRegistersAsync()
    {
        await LoadEligibleLoadingRegistersAsync();
    }

    /// <summary>
    /// Command to save the party bill register.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidationError = string.Empty;

        var request = new CreatePartyBillRegisterRequest
        {
            BillNumber = BillNumber,
            BillDate = BillDate,
            PartyId = PartyId,
            ThirdPartyName = ThirdPartyName,
            PermitNumber = PermitNumber,
            ConsignorId = ConsignorId,
            DestinationId = DestinationId,
            FromDate = FromDate,
            ToDate = ToDate,
            Remarks = Remarks,
            SelectedLoadingRegisterIds = EligibleLoadingRegisters.Where(lr => lr.IsSelected).Select(lr => lr.Id).ToList(),
            CreatedBy = "System"
        };

        var validationResult = _createPartyBillRegisterValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            ValidationError = string.Join(Environment.NewLine, validationResult.Errors.Select(e => e.ErrorMessage));
            return;
        }

        SetBusy("Creating party bill register...");
        var result = await _partyBillRegisterCommandService.CreatePartyBillRegisterAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await _navigationService.GoBackAsync();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to create party bill register.";
        }
    }

    /// <summary>
    /// Command to cancel and go back.
    /// </summary>
    [RelayCommand]
    private async Task CancelAsync()
    {
        await _navigationService.GoBackAsync();
    }

    /// <summary>
    /// Loads master data for dropdowns.
    /// </summary>
    private async Task LoadMasterDataAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            Customers = await _customerQueryService.GetAllCustomersAsync(cancellationToken);
            SourceDestinations = (await _sourceDestinationQueryService.GetAllSourceDestinationsAsync(cancellationToken)).ToList();
        }
        catch (Exception ex)
        {
            ValidationError = $"Failed to load master data: {ex.Message}";
        }
    }

    /// <summary>
    /// Loads eligible loading registers based on filters.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadEligibleLoadingRegistersAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading eligible loading registers...");
        var eligibleLoadingRegisters = await _partyBillRegisterQueryService.GetEligibleLoadingRegistersAsync(
            ConsignorId,
            DestinationId,
            FromDate,
            ToDate,
            cancellationToken);
        
        UpdateEligibleLoadingRegisters(eligibleLoadingRegisters);
        CalculateTotals();
        ClearBusy();
    }

    /// <summary>
    /// Updates the eligible loading registers collection on the UI thread.
    /// </summary>
    /// <param name="eligibleLoadingRegisters">The eligible loading registers to update.</param>
    private void UpdateEligibleLoadingRegisters(IEnumerable<EligibleLoadingRegisterModel> eligibleLoadingRegisters)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            // Already on UI thread or no dispatcher (fallback)
            UpdateEligibleLoadingRegistersInternal(eligibleLoadingRegisters);
        }
        else
        {
            // Marshal to UI thread
            dispatcher.Invoke(() => UpdateEligibleLoadingRegistersInternal(eligibleLoadingRegisters));
        }
    }

    /// <summary>
    /// Updates the eligible loading registers collection internally (must be called on UI thread).
    /// </summary>
    /// <param name="eligibleLoadingRegisters">The eligible loading registers to update.</param>
    private void UpdateEligibleLoadingRegistersInternal(IEnumerable<EligibleLoadingRegisterModel> eligibleLoadingRegisters)
    {
        EligibleLoadingRegisters.Clear();
        foreach (var item in eligibleLoadingRegisters)
        {
            EligibleLoadingRegisters.Add(item);
        }
    }

    /// <summary>
    /// Calculates totals based on selected loading registers.
    /// </summary>
    private void CalculateTotals()
    {
        var selectedItems = EligibleLoadingRegisters.Where(lr => lr.IsSelected).ToList();
        TotalRecords = selectedItems.Count;
        TotalMaterialWeight = selectedItems.Sum(lr => lr.MaterialWeight);
        TotalAmount = selectedItems.Sum(lr => lr.Amount);
        GrandTotal = TotalAmount;
    }

    /// <summary>
    /// Called when a loading register selection changes.
    /// </summary>
    public void OnSelectionChanged()
    {
        CalculateTotals();
    }
}
