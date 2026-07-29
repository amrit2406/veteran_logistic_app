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
/// ViewModel for the Edit Party Bill Register screen.
/// </summary>
public sealed partial class EditPartyBillRegisterViewModel : ViewModelBase, INavigationAware
{
    private readonly IPartyBillRegisterQueryService _partyBillRegisterQueryService;
    private readonly IPartyBillRegisterCommandService _partyBillRegisterCommandService;
    private readonly IUpdatePartyBillRegisterValidator _updatePartyBillRegisterValidator;
    private readonly INavigationService _navigationService;
    private string _validationError = string.Empty;

    // Header fields
    private int _partyBillRegisterId;
    private string _billNumber = string.Empty;
    private DateTime _billDate;
    private int _partyId;
    private string _partyName = string.Empty;
    private string _thirdPartyName = string.Empty;
    private string _permitNumber = string.Empty;

    // Additional charges
    private string _chargeHead1 = string.Empty;
    private string _chargeType1 = string.Empty;
    private decimal _chargeAmount1;
    private string _chargeHead2 = string.Empty;
    private string _chargeType2 = string.Empty;
    private decimal _chargeAmount2;

    // Remarks
    private string _remarks = string.Empty;

    // Calculated totals (read-only)
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
    /// Initializes a new instance of the <see cref="EditPartyBillRegisterViewModel"/> class.
    /// </summary>
    /// <param name="partyBillRegisterQueryService">The party bill register query service.</param>
    /// <param name="partyBillRegisterCommandService">The party bill register command service.</param>
    /// <param name="updatePartyBillRegisterValidator">The update party bill register validator.</param>
    /// <param name="navigationService">The navigation service.</param>
    public EditPartyBillRegisterViewModel(IPartyBillRegisterQueryService partyBillRegisterQueryService, IPartyBillRegisterCommandService partyBillRegisterCommandService, IUpdatePartyBillRegisterValidator updatePartyBillRegisterValidator, INavigationService navigationService)
    {
        _partyBillRegisterQueryService = partyBillRegisterQueryService ?? throw new ArgumentNullException(nameof(partyBillRegisterQueryService));
        _partyBillRegisterCommandService = partyBillRegisterCommandService ?? throw new ArgumentNullException(nameof(partyBillRegisterCommandService));
        _updatePartyBillRegisterValidator = updatePartyBillRegisterValidator ?? throw new ArgumentNullException(nameof(updatePartyBillRegisterValidator));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

        Title = "Edit Party Bill Register";
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

        await base.InitializeAsync(cancellationToken);
    }

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
        await LoadPartyBillRegisterAsync(cancellationToken);
        
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
    /// Called when navigation to this ViewModel occurs with parameters.
    /// </summary>
    public void OnNavigatedTo(NavigationParameter? parameter)
    {
        if (parameter is not null && parameter.TryGetValue<int>("PartyBillRegisterId", out var partyBillRegisterId))
        {
            PartyBillRegisterId = partyBillRegisterId;
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
    /// Gets the collection of party bill register details.
    /// </summary>
    public ObservableCollection<PartyBillRegisterDetailModel> PartyBillRegisterDetails { get; } = new();

    /// <summary>
    /// Gets the party bill register ID.
    /// </summary>
    public int PartyBillRegisterId
    {
        get => _partyBillRegisterId;
        private set => SetProperty(ref _partyBillRegisterId, value);
    }

    /// <summary>
    /// Gets the bill number (read-only).
    /// </summary>
    public string BillNumber
    {
        get => _billNumber;
        private set => SetProperty(ref _billNumber, value);
    }

    /// <summary>
    /// Gets or sets the bill date.
    /// </summary>
    public DateTime BillDate
    {
        get => _billDate;
        set => SetProperty(ref _billDate, value);
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
                CalculateGrandTotal();
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
                CalculateGrandTotal();
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
    /// Gets the total records (read-only).
    /// </summary>
    public int TotalRecords
    {
        get => _totalRecords;
        private set => SetProperty(ref _totalRecords, value);
    }

    /// <summary>
    /// Gets the total material weight (read-only).
    /// </summary>
    public decimal TotalMaterialWeight
    {
        get => _totalMaterialWeight;
        private set => SetProperty(ref _totalMaterialWeight, value);
    }

    /// <summary>
    /// Gets the total amount (read-only).
    /// </summary>
    public decimal TotalAmount
    {
        get => _totalAmount;
        private set => SetProperty(ref _totalAmount, value);
    }

    /// <summary>
    /// Gets the grand total (read-only, recalculated when charges change).
    /// </summary>
    public decimal GrandTotal
    {
        get => _grandTotal;
        private set => SetProperty(ref _grandTotal, value);
    }

    /// <summary>
    /// Command to save the party bill register.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidationError = string.Empty;

        var request = new UpdatePartyBillRegisterRequest
        {
            PartyBillRegisterId = PartyBillRegisterId,
            BillDate = BillDate,
            PartyId = PartyId,
            ThirdPartyName = ThirdPartyName,
            PermitNumber = PermitNumber,
            ChargeHead1 = ChargeHead1,
            ChargeType1 = ChargeType1,
            ChargeAmount1 = ChargeAmount1,
            ChargeHead2 = ChargeHead2,
            ChargeType2 = ChargeType2,
            ChargeAmount2 = ChargeAmount2,
            Remarks = Remarks,
            ModifiedBy = "System"
        };

        var validationResult = _updatePartyBillRegisterValidator.Validate(request);
        if (!validationResult.IsValid)
        {
            ValidationError = string.Join(Environment.NewLine, validationResult.Errors.Select(e => e.ErrorMessage));
            return;
        }

        SetBusy("Updating party bill register...");
        var result = await _partyBillRegisterCommandService.UpdatePartyBillRegisterAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await _navigationService.GoBackAsync();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to update party bill register.";
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
    /// Loads the party bill register for editing.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadPartyBillRegisterAsync(CancellationToken cancellationToken = default)
    {
        if (PartyBillRegisterId <= 0)
        {
            ValidationError = "Invalid party bill register ID.";
            return;
        }

        SetBusy("Loading party bill register...");
        var partyBillRegister = await _partyBillRegisterQueryService.GetPartyBillRegisterForEditAsync(PartyBillRegisterId, cancellationToken);
        ClearBusy();

        if (partyBillRegister is null)
        {
            ValidationError = "Party bill register not found.";
            return;
        }

        // Populate header fields
        BillNumber = partyBillRegister.BillNumber;
        BillDate = partyBillRegister.BillDate;
        PartyId = partyBillRegister.PartyId;
        PartyName = partyBillRegister.PartyName;
        ThirdPartyName = partyBillRegister.ThirdPartyName;
        PermitNumber = partyBillRegister.PermitNumber;
        ChargeHead1 = partyBillRegister.ChargeHead1;
        ChargeType1 = partyBillRegister.ChargeType1;
        ChargeAmount1 = partyBillRegister.ChargeAmount1;
        ChargeHead2 = partyBillRegister.ChargeHead2;
        ChargeType2 = partyBillRegister.ChargeType2;
        ChargeAmount2 = partyBillRegister.ChargeAmount2;
        Remarks = partyBillRegister.Remarks;

        // Populate calculated totals
        TotalRecords = partyBillRegister.TotalRecords;
        TotalMaterialWeight = partyBillRegister.TotalMaterialWeight;
        TotalAmount = partyBillRegister.TotalAmount;
        GrandTotal = partyBillRegister.GrandTotal;

        // Populate details
        UpdatePartyBillRegisterDetails(partyBillRegister.PartyBillRegisterDetails);
    }

    /// <summary>
    /// Updates the party bill register details collection on the UI thread.
    /// </summary>
    /// <param name="details">The party bill register details to update.</param>
    private void UpdatePartyBillRegisterDetails(IEnumerable<PartyBillRegisterDetailModel> details)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            // Already on UI thread or no dispatcher (fallback)
            UpdatePartyBillRegisterDetailsInternal(details);
        }
        else
        {
            // Marshal to UI thread
            dispatcher.Invoke(() => UpdatePartyBillRegisterDetailsInternal(details));
        }
    }

    /// <summary>
    /// Updates the party bill register details collection internally (must be called on UI thread).
    /// </summary>
    /// <param name="details">The party bill register details to update.</param>
    private void UpdatePartyBillRegisterDetailsInternal(IEnumerable<PartyBillRegisterDetailModel> details)
    {
        PartyBillRegisterDetails.Clear();
        foreach (var item in details)
        {
            PartyBillRegisterDetails.Add(item);
        }
    }

    /// <summary>
    /// Calculates the grand total based on total amount and additional charges.
    /// </summary>
    private void CalculateGrandTotal()
    {
        GrandTotal = TotalAmount + ChargeAmount1 + ChargeAmount2;
    }
}
