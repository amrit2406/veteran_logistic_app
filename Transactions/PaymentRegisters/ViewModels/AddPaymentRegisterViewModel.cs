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
/// ViewModel for the Add Payment Register screen.
/// </summary>
public sealed partial class AddPaymentRegisterViewModel : ViewModelBase
{
    private readonly IPaymentRegisterQueryService _paymentRegisterQueryService;
    private readonly IPaymentRegisterCommandService _paymentRegisterCommandService;
    private readonly INavigationService _navigationService;
    private readonly IPaymentLocationQueryService _paymentLocationQueryService;
    private string _challanNumber = string.Empty;
    private string _validationError = string.Empty;
    private PaymentRegisterModel? _paymentRegisterData;
    private IReadOnlyList<PaymentLocationListItem> _paymentLocations = [];
    private IReadOnlyList<string> _paymentTypes = ["Cash", "Cheque", "From Account"];
    private IReadOnlyList<string> _paymentStatuses = ["Pending", "Paid"];

    /// <summary>
    /// Command to navigate back to the previous screen.
    /// </summary>
    public IAsyncRelayCommand GoBackCommand { get; }

    /// <summary>
    /// Whether it's possible to go back in navigation history.
    /// </summary>
    public bool CanGoBack => _navigationService.CanGoBack;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddPaymentRegisterViewModel"/> class.
    /// </summary>
    /// <param name="paymentRegisterQueryService">The payment register query service.</param>
    /// <param name="paymentRegisterCommandService">The payment register command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    /// <param name="paymentLocationQueryService">The payment location query service.</param>
    public AddPaymentRegisterViewModel(IPaymentRegisterQueryService paymentRegisterQueryService, IPaymentRegisterCommandService paymentRegisterCommandService, INavigationService navigationService, IPaymentLocationQueryService paymentLocationQueryService)
    {
        _paymentRegisterQueryService = paymentRegisterQueryService ?? throw new ArgumentNullException(nameof(paymentRegisterQueryService));
        _paymentRegisterCommandService = paymentRegisterCommandService ?? throw new ArgumentNullException(nameof(paymentRegisterCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        _paymentLocationQueryService = paymentLocationQueryService ?? throw new ArgumentNullException(nameof(paymentLocationQueryService));

        Title = "Add Payment Register";
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

        try
        {
            // Load payment locations
            var paymentLocations = await _paymentLocationQueryService.GetAllPaymentLocationsAsync(cancellationToken);
            PaymentLocations = paymentLocations;
            
            await base.InitializeAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            ValidationError = $"Error loading payment locations: {ex.Message}";
        }
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
    /// Gets or sets the challan number.
    /// </summary>
    public string ChallanNumber
    {
        get => _challanNumber;
        set
        {
            if (SetProperty(ref _challanNumber, value))
            {
                LoadChallanDataCommand.NotifyCanExecuteChanged();
            }
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
    /// Gets or sets the payment register data (auto-populated from challan).
    /// </summary>
    public PaymentRegisterModel? PaymentRegisterData
    {
        get => _paymentRegisterData;
        set => SetProperty(ref _paymentRegisterData, value);
    }

    /// <summary>
    /// Gets the collection of payment locations.
    /// </summary>
    public IReadOnlyList<PaymentLocationListItem> PaymentLocations
    {
        get => _paymentLocations;
        private set => SetProperty(ref _paymentLocations, value);
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
    /// Gets or sets the payment date.
    /// </summary>
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the payment location ID.
    /// </summary>
    public int? PaymentLocationId { get; set; }

    /// <summary>
    /// Gets or sets the payment type.
    /// </summary>
    public string PaymentType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the HSD party.
    /// </summary>
    public string? HSDParty { get; set; }

    /// <summary>
    /// Gets or sets the notes.
    /// </summary>
    public string Notes { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the beneficiary name.
    /// </summary>
    public string Beneficiary { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the PAN number.
    /// </summary>
    public string PAN { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the UTR number.
    /// </summary>
    public string UTRNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the mobile number.
    /// </summary>
    public string MobileNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the account number.
    /// </summary>
    public string AccountNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the IFSC code.
    /// </summary>
    public string IFSCCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the bank name.
    /// </summary>
    public string BankName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the TDS percentage.
    /// </summary>
    public decimal TDSPercentage { get; set; }

    /// <summary>
    /// Gets or sets the challan money.
    /// </summary>
    public decimal ChallanMoney { get; set; }

    /// <summary>
    /// Gets or sets the surcharge at 2%.
    /// </summary>
    public decimal Surcharge { get; set; }

    /// <summary>
    /// Gets or sets the admin charge.
    /// </summary>
    public decimal AdminCharge { get; set; }

    /// <summary>
    /// Gets or sets whether the payment register is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Gets or sets the payment status.
    /// </summary>
    public string PaymentStatus { get; set; } = "Pending";

    /// <summary>
    /// Command to load challan data.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteLoadChallanData))]
    private async Task LoadChallanDataAsync()
    {
        if (string.IsNullOrWhiteSpace(ChallanNumber))
        {
            return;
        }

        ValidationError = string.Empty;
        SetBusy("Loading challan data...");
        
        var data = await _paymentRegisterQueryService.GetPaymentRegisterDataByChallanNumberAsync(ChallanNumber, CancellationToken.None);
        
        ClearBusy();

        if (data is null)
        {
            ValidationError = "Challan number not found or payment already exists.";
            PaymentRegisterData = null;
        }
        else
        {
            PaymentRegisterData = data;
            PaymentDate = data.PaymentDate;
            ChallanMoney = data.ChallanMoney;
        }
    }

    private bool CanExecuteLoadChallanData()
    {
        return !string.IsNullOrWhiteSpace(ChallanNumber);
    }

    /// <summary>
    /// Command to save the payment register.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        if (PaymentRegisterData is null)
        {
            ValidationError = "Please load challan data first.";
            return;
        }

        ValidationError = string.Empty;

        var request = new CreatePaymentRegisterRequest
        {
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
        var result = await _paymentRegisterCommandService.CreatePaymentRegisterAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            MessageBox.Show("Payment register created successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            await _navigationService.GoBackAsync();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to create payment register.";
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
