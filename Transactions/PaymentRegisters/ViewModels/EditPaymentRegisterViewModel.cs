using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading;
using System.Windows;
using veteran_logistic.Transactions.PaymentRegisters.Contracts;
using veteran_logistic.Transactions.PaymentRegisters.Models;
using veteran_logistic.Masters.PaymentLocations.Contracts;
using veteran_logistic.Masters.PaymentLocations.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Transactions.PaymentRegisters.ViewModels;

/// <summary>
/// ViewModel for the Edit Payment Register screen.
/// </summary>
public sealed partial class EditPaymentRegisterViewModel : ViewModelBase, INavigationAware
{
    private readonly IPaymentRegisterQueryService _paymentRegisterQueryService;
    private readonly IPaymentRegisterCommandService _paymentRegisterCommandService;
    private readonly INavigationService _navigationService;
    private readonly IPaymentLocationQueryService _paymentLocationQueryService;
    private int _paymentRegisterId;
    private string _validationError = string.Empty;
    private string _challanNumber = string.Empty;
    private string _tpNumber = string.Empty;
    private string? _vehicleNumber;
    private string _vehicleType = string.Empty;
    private string? _materialName;
    private decimal _driverCommission;
    private DateTime? _loadingDate;
    private DateTime? _unloadingDate;
    private decimal _loadingWeight;
    private decimal _unloadingWeight;
    private decimal _grossAmount;
    private DateTime _paymentDate;
    private int? _paymentLocationId;
    private string _paymentType = string.Empty;
    private string? _hsdParty;
    private string _notes = string.Empty;
    private string _beneficiary = string.Empty;
    private string _pan = string.Empty;
    private string _utrNumber = string.Empty;
    private string _mobileNumber = string.Empty;
    private string _accountNumber = string.Empty;
    private string _ifscCode = string.Empty;
    private string _bankName = string.Empty;
    private decimal _tdsPercentage;
    private decimal _challanMoney;
    private decimal _surcharge;
    private decimal _adminCharge;
    private decimal _payableAmount;
    private string _paymentStatus = string.Empty;
    private bool _isActive;
    private IReadOnlyList<string> _paymentTypes = ["Cash", "Cheque", "From Account"];
    private IReadOnlyList<string> _paymentStatuses = ["Pending", "Paid"];
    private IReadOnlyList<PaymentLocationListItem> _paymentLocations = [];

    /// <summary>
    /// Gets or sets the payment register ID.
    /// </summary>
    public int PaymentRegisterId
    {
        get => _paymentRegisterId;
        set => SetProperty(ref _paymentRegisterId, value);
    }

    /// <summary>
    /// Command to navigate back to the previous screen.
    /// </summary>
    public IAsyncRelayCommand GoBackCommand { get; }

    /// <summary>
    /// Whether it's possible to go back in navigation history.
    /// </summary>
    public bool CanGoBack => _navigationService.CanGoBack;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditPaymentRegisterViewModel"/> class.
    /// </summary>
    /// <param name="paymentRegisterQueryService">The payment register query service.</param>
    /// <param name="paymentRegisterCommandService">The payment register command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    /// <param name="paymentLocationQueryService">The payment location query service.</param>
    public EditPaymentRegisterViewModel(IPaymentRegisterQueryService paymentRegisterQueryService, IPaymentRegisterCommandService paymentRegisterCommandService, INavigationService navigationService, IPaymentLocationQueryService paymentLocationQueryService)
    {
        _paymentRegisterQueryService = paymentRegisterQueryService ?? throw new ArgumentNullException(nameof(paymentRegisterQueryService));
        _paymentRegisterCommandService = paymentRegisterCommandService ?? throw new ArgumentNullException(nameof(paymentRegisterCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        _paymentLocationQueryService = paymentLocationQueryService ?? throw new ArgumentNullException(nameof(paymentLocationQueryService));

        Title = "Edit Payment Register";
        GoBackCommand = new AsyncRelayCommand(ExecuteGoBackAsync, () => CanGoBack);
    }

    private async Task ExecuteGoBackAsync()
    {
        await _navigationService.GoBackAsync();
        GoBackCommand.NotifyCanExecuteChanged();
    }

    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        System.Diagnostics.Debug.WriteLine($"EditPaymentRegisterViewModel: InitializeAsync called (Instance: {GetHashCode()})");
        System.Diagnostics.Debug.WriteLine($"EditPaymentRegisterViewModel: IsInitialized={IsInitialized}");
        
        if (IsInitialized)
        {
            System.Diagnostics.Debug.WriteLine($"EditPaymentRegisterViewModel: Already initialized, returning");
            return;
        }

        try
        {
            // Load payment locations
            System.Diagnostics.Debug.WriteLine($"EditPaymentRegisterViewModel: Loading payment locations...");
            var paymentLocations = await _paymentLocationQueryService.GetAllPaymentLocationsAsync(cancellationToken);
            PaymentLocations = paymentLocations ?? [];
            System.Diagnostics.Debug.WriteLine($"EditPaymentRegisterViewModel: Loaded {PaymentLocations.Count} payment locations");
            
            // Load the payment register data
            await LoadPaymentRegisterAsync(cancellationToken);
            
            await base.InitializeAsync(cancellationToken);
            System.Diagnostics.Debug.WriteLine($"EditPaymentRegisterViewModel: Initialization complete, IsInitialized={IsInitialized}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"InitializeAsync Error: {ex.Message}");
            if (ex.InnerException != null)
            {
                System.Diagnostics.Debug.WriteLine($"InitializeAsync Inner: {ex.InnerException.Message}");
            }
            System.Diagnostics.Debug.WriteLine($"InitializeAsync Stack: {ex.StackTrace}");
            ValidationError = $"Error initializing edit view: {ex.Message}{(ex.InnerException != null ? $" | Inner: {ex.InnerException.Message}" : "")}";
        }
    }

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
        await base.OnNavigatedToAsync(cancellationToken);
        
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

    public void OnNavigatedTo(NavigationParameter? parameter)
    {
        System.Diagnostics.Debug.WriteLine($"EditPaymentRegisterViewModel: OnNavigatedTo called (Instance: {GetHashCode()})");
        
        if (parameter != null && parameter.TryGetValue<int>("PaymentRegisterId", out var id))
        {
            System.Diagnostics.Debug.WriteLine($"EditPaymentRegisterViewModel: PaymentRegisterId from parameter = {id}");
            _paymentRegisterId = id;
            System.Diagnostics.Debug.WriteLine($"EditPaymentRegisterViewModel: _paymentRegisterId set = {_paymentRegisterId}");
        }
        else
        {
            System.Diagnostics.Debug.WriteLine($"EditPaymentRegisterViewModel: Parameter is null or PaymentRegisterId not found");
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
    /// Gets or sets the challan number (read-only, auto-populated).
    /// </summary>
    public string ChallanNumber
    {
        get => _challanNumber;
        private set => SetProperty(ref _challanNumber, value);
    }

    /// <summary>
    /// Gets or sets the TP number (read-only, auto-populated).
    /// </summary>
    public string TPNumber
    {
        get => _tpNumber;
        private set => SetProperty(ref _tpNumber, value);
    }

    /// <summary>
    /// Gets or sets the vehicle number (read-only, auto-populated).
    /// </summary>
    public string? VehicleNumber
    {
        get => _vehicleNumber;
        private set => SetProperty(ref _vehicleNumber, value);
    }

    /// <summary>
    /// Gets or sets the vehicle type (read-only, auto-populated).
    /// </summary>
    public string VehicleType
    {
        get => _vehicleType;
        private set => SetProperty(ref _vehicleType, value);
    }

    /// <summary>
    /// Gets or sets the material name (read-only, auto-populated).
    /// </summary>
    public string? MaterialName
    {
        get => _materialName;
        private set => SetProperty(ref _materialName, value);
    }

    /// <summary>
    /// Gets or sets the driver commission (read-only, auto-populated).
    /// </summary>
    public decimal DriverCommission
    {
        get => _driverCommission;
        private set => SetProperty(ref _driverCommission, value);
    }

    /// <summary>
    /// Gets or sets the loading date (read-only, auto-populated).
    /// </summary>
    public DateTime? LoadingDate
    {
        get => _loadingDate;
        private set => SetProperty(ref _loadingDate, value);
    }

    /// <summary>
    /// Gets or sets the unloading date (read-only, auto-populated).
    /// </summary>
    public DateTime? UnloadingDate
    {
        get => _unloadingDate;
        private set => SetProperty(ref _unloadingDate, value);
    }

    /// <summary>
    /// Gets or sets the loading weight (read-only, auto-populated).
    /// </summary>
    public decimal LoadingWeight
    {
        get => _loadingWeight;
        private set => SetProperty(ref _loadingWeight, value);
    }

    /// <summary>
    /// Gets or sets the unloading weight (read-only, auto-populated).
    /// </summary>
    public decimal UnloadingWeight
    {
        get => _unloadingWeight;
        private set => SetProperty(ref _unloadingWeight, value);
    }

    /// <summary>
    /// Gets or sets the gross amount (read-only, auto-populated).
    /// </summary>
    public decimal GrossAmount
    {
        get => _grossAmount;
        private set => SetProperty(ref _grossAmount, value);
    }

    /// <summary>
    /// Gets or sets the payment date.
    /// </summary>
    public DateTime PaymentDate
    {
        get => _paymentDate;
        set => SetProperty(ref _paymentDate, value);
    }

    /// <summary>
    /// Gets or sets the payment location ID.
    /// </summary>
    public int? PaymentLocationId
    {
        get => _paymentLocationId;
        set => SetProperty(ref _paymentLocationId, value);
    }

    /// <summary>
    /// Gets or sets the payment type.
    /// </summary>
    public string PaymentType
    {
        get => _paymentType;
        set => SetProperty(ref _paymentType, value);
    }

    /// <summary>
    /// Gets or sets the HSD party.
    /// </summary>
    public string? HSDParty
    {
        get => _hsdParty;
        set => SetProperty(ref _hsdParty, value);
    }

    /// <summary>
    /// Gets or sets the notes.
    /// </summary>
    public string Notes
    {
        get => _notes;
        set => SetProperty(ref _notes, value);
    }

    /// <summary>
    /// Gets or sets the beneficiary name.
    /// </summary>
    public string Beneficiary
    {
        get => _beneficiary;
        set => SetProperty(ref _beneficiary, value);
    }

    /// <summary>
    /// Gets or sets the PAN number.
    /// </summary>
    public string PAN
    {
        get => _pan;
        set => SetProperty(ref _pan, value);
    }

    /// <summary>
    /// Gets or sets the UTR number.
    /// </summary>
    public string UTRNumber
    {
        get => _utrNumber;
        set => SetProperty(ref _utrNumber, value);
    }

    /// <summary>
    /// Gets or sets the mobile number.
    /// </summary>
    public string MobileNumber
    {
        get => _mobileNumber;
        set => SetProperty(ref _mobileNumber, value);
    }

    /// <summary>
    /// Gets or sets the account number.
    /// </summary>
    public string AccountNumber
    {
        get => _accountNumber;
        set => SetProperty(ref _accountNumber, value);
    }

    /// <summary>
    /// Gets or sets the IFSC code.
    /// </summary>
    public string IFSCCode
    {
        get => _ifscCode;
        set => SetProperty(ref _ifscCode, value);
    }

    /// <summary>
    /// Gets or sets the bank name.
    /// </summary>
    public string BankName
    {
        get => _bankName;
        set => SetProperty(ref _bankName, value);
    }

    /// <summary>
    /// Gets or sets the TDS percentage.
    /// </summary>
    public decimal TDSPercentage
    {
        get => _tdsPercentage;
        set => SetProperty(ref _tdsPercentage, value);
    }

    /// <summary>
    /// Gets or sets the challan money.
    /// </summary>
    public decimal ChallanMoney
    {
        get => _challanMoney;
        set => SetProperty(ref _challanMoney, value);
    }

    /// <summary>
    /// Gets or sets the surcharge at 2%.
    /// </summary>
    public decimal Surcharge
    {
        get => _surcharge;
        set => SetProperty(ref _surcharge, value);
    }

    /// <summary>
    /// Gets or sets the admin charge.
    /// </summary>
    public decimal AdminCharge
    {
        get => _adminCharge;
        set => SetProperty(ref _adminCharge, value);
    }

    /// <summary>
    /// Gets or sets the payable amount (calculated, read-only).
    /// </summary>
    public decimal PayableAmount
    {
        get => _payableAmount;
        private set => SetProperty(ref _payableAmount, value);
    }

    /// <summary>
    /// Gets or sets the payment status (read-only, auto-calculated).
    /// </summary>
    public string PaymentStatus
    {
        get => _paymentStatus;
        private set => SetProperty(ref _paymentStatus, value);
    }

    /// <summary>
    /// Gets or sets whether the payment register is active.
    /// </summary>
    public bool IsActive
    {
        get => _isActive;
        set => SetProperty(ref _isActive, value);
    }

    /// <summary>
    /// Gets the collection of payment types.
    /// </summary>
    public IReadOnlyList<string> PaymentTypes => _paymentTypes;

    /// <summary>
    /// Gets the collection of payment statuses.
    /// </summary>
    public IReadOnlyList<string> PaymentStatuses => _paymentStatuses;

    /// <summary>
    /// Gets the collection of payment locations.
    /// </summary>
    public IReadOnlyList<PaymentLocationListItem> PaymentLocations
    {
        get => _paymentLocations;
        private set => SetProperty(ref _paymentLocations, value);
    }

    /// <summary>
    /// Loads the payment register data.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadPaymentRegisterAsync(CancellationToken cancellationToken = default)
    {
        System.Diagnostics.Debug.WriteLine($"EditPaymentRegisterViewModel: LoadPaymentRegisterAsync called with PaymentRegisterId = {PaymentRegisterId}");
        
        if (PaymentRegisterId == 0)
        {
            ValidationError = "Payment register ID not provided.";
            System.Diagnostics.Debug.WriteLine($"EditPaymentRegisterViewModel: PaymentRegisterId is 0, returning");
            return;
        }

        SetBusy("Loading payment register...");
        System.Diagnostics.Debug.WriteLine($"EditPaymentRegisterViewModel: Calling GetPaymentRegisterForEditAsync with ID = {PaymentRegisterId}");
        var paymentRegister = await _paymentRegisterQueryService.GetPaymentRegisterForEditAsync(PaymentRegisterId, cancellationToken);
        ClearBusy();

        if (paymentRegister is null)
        {
            ValidationError = "Payment register not found.";
            System.Diagnostics.Debug.WriteLine($"EditPaymentRegisterViewModel: paymentRegister is null");
            return;
        }

        System.Diagnostics.Debug.WriteLine($"EditPaymentRegisterViewModel: paymentRegister loaded, ChallanNumber = {paymentRegister.ChallanNumber}");

        ChallanNumber = paymentRegister.ChallanNumber;
        TPNumber = paymentRegister.TPNumber;
        VehicleNumber = paymentRegister.VehicleNumber;
        VehicleType = paymentRegister.VehicleType;
        MaterialName = paymentRegister.MaterialName;
        DriverCommission = paymentRegister.DriverCommission;
        LoadingDate = paymentRegister.LoadingDate;
        UnloadingDate = paymentRegister.UnloadingDate;
        LoadingWeight = paymentRegister.LoadingWeight;
        UnloadingWeight = paymentRegister.UnloadingWeight;
        GrossAmount = paymentRegister.GrossAmount;
        PaymentDate = paymentRegister.PaymentDate;
        PaymentLocationId = paymentRegister.PaymentLocationId;
        PaymentType = paymentRegister.PaymentType;
        HSDParty = paymentRegister.HSDParty;
        Notes = paymentRegister.Notes;
        Beneficiary = paymentRegister.Beneficiary;
        PAN = paymentRegister.PAN;
        UTRNumber = paymentRegister.UTRNumber;
        MobileNumber = paymentRegister.MobileNumber;
        AccountNumber = paymentRegister.AccountNumber;
        IFSCCode = paymentRegister.IFSCCode;
        BankName = paymentRegister.BankName;
        TDSPercentage = paymentRegister.TDSPercentage;
        ChallanMoney = paymentRegister.ChallanMoney;
        Surcharge = paymentRegister.Surcharge;
        AdminCharge = paymentRegister.AdminCharge;
        PayableAmount = paymentRegister.PayableAmount;
        PaymentStatus = paymentRegister.PaymentStatus;
        IsActive = paymentRegister.IsActive;
        
        System.Diagnostics.Debug.WriteLine($"EditPaymentRegisterViewModel: All properties set");
    }

    /// <summary>
    /// Command to save the payment register.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidationError = string.Empty;

        var request = new UpdatePaymentRegisterRequest
        {
            PaymentRegisterId = PaymentRegisterId,
            ChallanNumber = ChallanNumber,
            PaymentDate = PaymentDate,
            PaymentLocationId = PaymentLocationId,
            PaymentType = PaymentType,
            HSDParty = HSDParty,
            Notes = Notes,
            Beneficiary = Beneficiary,
            PAN = PAN,
            UTRNumber = UTRNumber,
            MobileNumber = MobileNumber,
            AccountNumber = AccountNumber,
            IFSCCode = IFSCCode,
            BankName = BankName,
            TDSPercentage = TDSPercentage,
            ChallanMoney = ChallanMoney,
            Surcharge = Surcharge,
            AdminCharge = AdminCharge,
            IsActive = IsActive,
            PaymentStatus = PaymentStatus
        };

        SetBusy("Saving payment register...");
        var result = await _paymentRegisterCommandService.UpdatePaymentRegisterAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            MessageBox.Show("Payment register updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            await _navigationService.GoBackAsync();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to update payment register.";
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
}
