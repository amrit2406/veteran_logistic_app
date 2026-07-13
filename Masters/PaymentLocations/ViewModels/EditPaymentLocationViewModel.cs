using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using veteran_logistic.Masters.PaymentLocations.Contracts;
using veteran_logistic.Masters.PaymentLocations.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.PaymentLocations.ViewModels;

/// <summary>
/// ViewModel for the Edit Payment Location screen.
/// </summary>
public sealed partial class EditPaymentLocationViewModel : ViewModelBase, INavigationAware
{
    private readonly IPaymentLocationCommandService _paymentLocationCommandService;
    private readonly IPaymentLocationQueryService _paymentLocationQueryService;
    private readonly INavigationService _navigationService;
    private int _paymentLocationId;
    private string _paymentLocationName = string.Empty;
    private bool _isActive;
    private string _validationError = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditPaymentLocationViewModel"/> class.
    /// </summary>
    /// <param name="paymentLocationCommandService">The payment location command service.</param>
    /// <param name="paymentLocationQueryService">The payment location query service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public EditPaymentLocationViewModel(IPaymentLocationCommandService paymentLocationCommandService, IPaymentLocationQueryService paymentLocationQueryService, INavigationService navigationService)
    {
        _paymentLocationCommandService = paymentLocationCommandService ?? throw new ArgumentNullException(nameof(paymentLocationCommandService));
        _paymentLocationQueryService = paymentLocationQueryService ?? throw new ArgumentNullException(nameof(paymentLocationQueryService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        
        Title = "Edit Payment Location";
    }

    public void OnNavigatedTo(NavigationParameter? parameter)
    {
        if (parameter is not null && parameter.TryGetValue<int>("PaymentLocationId", out var paymentLocationId))
        {
            _paymentLocationId = paymentLocationId;
        }
    }

    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (IsInitialized)
        {
            return;
        }

        await LoadPaymentLocationAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    /// <summary>
    /// Gets or sets the payment location name.
    /// </summary>
    public string PaymentLocationName
    {
        get => _paymentLocationName;
        set => SetProperty(ref _paymentLocationName, value);
    }

    /// <summary>
    /// Gets or sets whether the payment location is active.
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
    /// Loads the payment location data for editing.
    /// </summary>
    private async Task LoadPaymentLocationAsync(CancellationToken cancellationToken = default)
    {
        if (_paymentLocationId == 0)
        {
            ValidationError = "Payment location ID is required.";
            return;
        }

        SetBusy("Loading payment location...");
        var paymentLocation = await _paymentLocationQueryService.GetPaymentLocationForEditAsync(_paymentLocationId, cancellationToken).ConfigureAwait(false);
        ClearBusy();

        if (paymentLocation is null)
        {
            ValidationError = "Payment location not found.";
            return;
        }

        PaymentLocationName = paymentLocation.PaymentLocationName;
        IsActive = paymentLocation.IsActive;
    }

    /// <summary>
    /// Command to save the payment location.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidationError = string.Empty;

        var request = new UpdatePaymentLocationRequest
        {
            PaymentLocationId = _paymentLocationId,
            PaymentLocationName = PaymentLocationName,
            IsActive = IsActive
        };

        SetBusy("Updating payment location...");
        var result = await _paymentLocationCommandService.UpdatePaymentLocationAsync(request, CancellationToken.None).ConfigureAwait(false);
        ClearBusy();

        if (result.IsSuccess)
        {
            await _navigationService.GoBackAsync().ConfigureAwait(false);
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to update payment location.";
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
