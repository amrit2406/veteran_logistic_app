using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading;
using System.Windows;
using veteran_logistic.Transactions.PaymentRegisters.Contracts;
using veteran_logistic.Transactions.PaymentRegisters.Models;
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
    private int _paymentRegisterId;
    private string _validationError = string.Empty;

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
    public EditPaymentRegisterViewModel(IPaymentRegisterQueryService paymentRegisterQueryService, IPaymentRegisterCommandService paymentRegisterCommandService, INavigationService navigationService)
    {
        _paymentRegisterQueryService = paymentRegisterQueryService ?? throw new ArgumentNullException(nameof(paymentRegisterQueryService));
        _paymentRegisterCommandService = paymentRegisterCommandService ?? throw new ArgumentNullException(nameof(paymentRegisterCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

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
        if (IsInitialized)
        {
            return;
        }

        await base.InitializeAsync(cancellationToken);
    }

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
        await base.OnNavigatedToAsync(cancellationToken);
        await LoadPaymentRegisterAsync(cancellationToken);
        
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
        if (parameter != null && parameter.TryGetValue<int>("PaymentRegisterId", out var id))
        {
            PaymentRegisterId = id;
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
    /// Gets or sets the payment register ID.
    /// </summary>
    public int PaymentRegisterId
    {
        get => _paymentRegisterId;
        set => SetProperty(ref _paymentRegisterId, value);
    }

    /// <summary>
    /// Gets or sets the challan number (read-only, auto-populated).
    /// </summary>
    public string ChallanNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the TP number (read-only, auto-populated).
    /// </summary>
    public string TPNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the vehicle number (read-only, auto-populated).
    /// </summary>
    public string? VehicleNumber { get; set; }

    /// <summary>
    /// Gets or sets the vehicle type (read-only, auto-populated).
    /// </summary>
    public string VehicleType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the material name (read-only, auto-populated).
    /// </summary>
    public string? MaterialName { get; set; }

    /// <summary>
    /// Gets or sets the driver commission (read-only, auto-populated).
    /// </summary>
    public decimal DriverCommission { get; set; }

    /// <summary>
    /// Gets or sets the loading date (read-only, auto-populated).
    /// </summary>
    public DateTime? LoadingDate { get; set; }

    /// <summary>
    /// Gets or sets the unloading date (read-only, auto-populated).
    /// </summary>
    public DateTime? UnloadingDate { get; set; }

    /// <summary>
    /// Gets or sets the loading weight (read-only, auto-populated).
    /// </summary>
    public decimal LoadingWeight { get; set; }

    /// <summary>
    /// Gets or sets the unloading weight (read-only, auto-populated).
    /// </summary>
    public decimal UnloadingWeight { get; set; }

    /// <summary>
    /// Gets or sets the gross amount (read-only, auto-populated).
    /// </summary>
    public decimal GrossAmount { get; set; }

    /// <summary>
    /// Gets or sets the payment date.
    /// </summary>
    public DateTime PaymentDate { get; set; }

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
    /// Gets or sets the payable amount (calculated, read-only).
    /// </summary>
    public decimal PayableAmount { get; set; }

    /// <summary>
    /// Gets or sets the payment status (read-only, auto-calculated).
    /// </summary>
    public string PaymentStatus { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the payment register is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Loads the payment register data.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadPaymentRegisterAsync(CancellationToken cancellationToken = default)
    {
        if (PaymentRegisterId == 0)
        {
            ValidationError = "Payment register ID not provided.";
            return;
        }

        SetBusy("Loading payment register...");
        var paymentRegister = await _paymentRegisterQueryService.GetPaymentRegisterForEditAsync(PaymentRegisterId, cancellationToken);
        ClearBusy();

        if (paymentRegister is null)
        {
            ValidationError = "Payment register not found.";
            return;
        }

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
            IsActive = IsActive
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
