using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using veteran_logistic.Transactions.PartyBillRegister.Contracts;
using veteran_logistic.Transactions.PartyBillRegister.Models;
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
    private string _validationError = string.Empty;
    private bool _selectAll = false;

    // Header fields
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

    // Additional charges
    private string _chargeHead1 = string.Empty;
    private string _chargeType1 = string.Empty;
    private decimal _chargeAmount1;
    private string _chargeHead2 = string.Empty;
    private string _chargeType2 = string.Empty;
    private decimal _chargeAmount2;

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
    public AddPartyBillRegisterViewModel(IPartyBillRegisterQueryService partyBillRegisterQueryService, IPartyBillRegisterCommandService partyBillRegisterCommandService, ICreatePartyBillRegisterValidator createPartyBillRegisterValidator, INavigationService navigationService)
    {
        _partyBillRegisterQueryService = partyBillRegisterQueryService ?? throw new ArgumentNullException(nameof(partyBillRegisterQueryService));
        _partyBillRegisterCommandService = partyBillRegisterCommandService ?? throw new ArgumentNullException(nameof(partyBillRegisterCommandService));
        _createPartyBillRegisterValidator = createPartyBillRegisterValidator ?? throw new ArgumentNullException(nameof(createPartyBillRegisterValidator));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

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

        await LoadEligibleLoadingRegistersAsync(cancellationToken);
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
    /// Gets or sets the first charge head.
    /// </summary>
    public string ChargeHead1
    {
        get => _chargeHead1;
        set => SetProperty(ref _chargeHead1, value);
    }

    /// <summary>
    /// Gets or sets the first charge type.
    /// </summary>
    public string ChargeType1
    {
        get => _chargeType1;
        set => SetProperty(ref _chargeType1, value);
    }

    /// <summary>
    /// Gets or sets the first charge amount.
    /// </summary>
    public decimal ChargeAmount1
    {
        get => _chargeAmount1;
        set
        {
            if (SetProperty(ref _chargeAmount1, value))
            {
                CalculateTotals();
            }
        }
    }

    /// <summary>
    /// Gets or sets the second charge head.
    /// </summary>
    public string ChargeHead2
    {
        get => _chargeHead2;
        set => SetProperty(ref _chargeHead2, value);
    }

    /// <summary>
    /// Gets or sets the second charge type.
    /// </summary>
    public string ChargeType2
    {
        get => _chargeType2;
        set => SetProperty(ref _chargeType2, value);
    }

    /// <summary>
    /// Gets or sets the second charge amount.
    /// </summary>
    public decimal ChargeAmount2
    {
        get => _chargeAmount2;
        set
        {
            if (SetProperty(ref _chargeAmount2, value))
            {
                CalculateTotals();
            }
        }
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
            BillDate = BillDate,
            PartyId = PartyId,
            ThirdPartyName = ThirdPartyName,
            PermitNumber = PermitNumber,
            ConsignorId = ConsignorId,
            DestinationId = DestinationId,
            FromDate = FromDate,
            ToDate = ToDate,
            ChargeHead1 = ChargeHead1,
            ChargeType1 = ChargeType1,
            ChargeAmount1 = ChargeAmount1,
            ChargeHead2 = ChargeHead2,
            ChargeType2 = ChargeType2,
            ChargeAmount2 = ChargeAmount2,
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
    /// Calculates totals based on selected loading registers and additional charges.
    /// </summary>
    private void CalculateTotals()
    {
        var selectedItems = EligibleLoadingRegisters.Where(lr => lr.IsSelected).ToList();
        TotalRecords = selectedItems.Count;
        TotalMaterialWeight = selectedItems.Sum(lr => lr.MaterialWeight);
        TotalAmount = selectedItems.Sum(lr => lr.Amount);
        GrandTotal = TotalAmount + ChargeAmount1 + ChargeAmount2;
    }

    /// <summary>
    /// Called when a loading register selection changes.
    /// </summary>
    public void OnSelectionChanged()
    {
        CalculateTotals();
    }
}
