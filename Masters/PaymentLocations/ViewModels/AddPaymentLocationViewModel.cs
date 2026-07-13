using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using veteran_logistic.Masters.PaymentLocations.Contracts;
using veteran_logistic.Masters.PaymentLocations.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.PaymentLocations.ViewModels;

/// <summary>
/// ViewModel for the Add Payment Location screen.
/// </summary>
public sealed partial class AddPaymentLocationViewModel : ViewModelBase
{
    private readonly IPaymentLocationCommandService _paymentLocationCommandService;
    private readonly INavigationService _navigationService;
    private string _paymentLocationName = string.Empty;
    private bool _isActive = true;
    private string _validationError = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddPaymentLocationViewModel"/> class.
    /// </summary>
    /// <param name="paymentLocationCommandService">The payment location command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public AddPaymentLocationViewModel(IPaymentLocationCommandService paymentLocationCommandService, INavigationService navigationService)
    {
        _paymentLocationCommandService = paymentLocationCommandService ?? throw new ArgumentNullException(nameof(paymentLocationCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        
        Title = "Add Payment Location";
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
    /// Command to save the payment location.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidationError = string.Empty;

        var request = new CreatePaymentLocationRequest
        {
            PaymentLocationName = PaymentLocationName,
            IsActive = IsActive
        };

        SetBusy("Creating payment location...");
        var result = await _paymentLocationCommandService.CreatePaymentLocationAsync(request).ConfigureAwait(false);
        ClearBusy();

        if (result.IsSuccess)
        {
            await _navigationService.GoBackAsync().ConfigureAwait(false);
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to create payment location.";
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
