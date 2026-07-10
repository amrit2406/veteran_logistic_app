using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using veteran_logistic.Masters.Customers.Contracts;
using veteran_logistic.Masters.Customers.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.Customers.ViewModels;

/// <summary>
/// ViewModel for the Edit Customer screen.
/// </summary>
public sealed partial class EditCustomerViewModel : ViewModelBase, INavigationAware
{
    private readonly ICustomerCommandService _customerCommandService;
    private readonly ICustomerQueryService _customerQueryService;
    private readonly INavigationService _navigationService;
    private int _customerId;
    private string _customerCode = string.Empty;
    private string _customerName = string.Empty;
    private string _addressLine1 = string.Empty;
    private string _addressLine2 = string.Empty;
    private string _city = string.Empty;
    private string _state = string.Empty;
    private string _country = string.Empty;
    private string _postalCode = string.Empty;
    private string _phoneNumber = string.Empty;
    private string _email = string.Empty;
    private string _gstNumber = string.Empty;
    private string _panNumber = string.Empty;
    private bool _isActive;
    private string _validationError = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditCustomerViewModel"/> class.
    /// </summary>
    /// <param name="customerCommandService">The customer command service.</param>
    /// <param name="customerQueryService">The customer query service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public EditCustomerViewModel(ICustomerCommandService customerCommandService, ICustomerQueryService customerQueryService, INavigationService navigationService)
    {
        _customerCommandService = customerCommandService ?? throw new ArgumentNullException(nameof(customerCommandService));
        _customerQueryService = customerQueryService ?? throw new ArgumentNullException(nameof(customerQueryService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        
        Title = "Edit Customer";
    }

    public void OnNavigatedTo(NavigationParameter? parameter)
    {
        if (parameter is not null && parameter.TryGetValue<int>("CustomerId", out var customerId))
        {
            _customerId = customerId;
        }
    }

    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (IsInitialized)
        {
            return;
        }

        await LoadCustomerAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    /// <summary>
    /// Gets or sets the customer code.
    /// </summary>
    public string CustomerCode
    {
        get => _customerCode;
        set => SetProperty(ref _customerCode, value);
    }

    /// <summary>
    /// Gets or sets the customer name.
    /// </summary>
    public string CustomerName
    {
        get => _customerName;
        set => SetProperty(ref _customerName, value);
    }

    /// <summary>
    /// Gets or sets the address line 1.
    /// </summary>
    public string AddressLine1
    {
        get => _addressLine1;
        set => SetProperty(ref _addressLine1, value);
    }

    /// <summary>
    /// Gets or sets the address line 2.
    /// </summary>
    public string AddressLine2
    {
        get => _addressLine2;
        set => SetProperty(ref _addressLine2, value);
    }

    /// <summary>
    /// Gets or sets the city.
    /// </summary>
    public string City
    {
        get => _city;
        set => SetProperty(ref _city, value);
    }

    /// <summary>
    /// Gets or sets the state.
    /// </summary>
    public string State
    {
        get => _state;
        set => SetProperty(ref _state, value);
    }

    /// <summary>
    /// Gets or sets the country.
    /// </summary>
    public string Country
    {
        get => _country;
        set => SetProperty(ref _country, value);
    }

    /// <summary>
    /// Gets or sets the postal code.
    /// </summary>
    public string PostalCode
    {
        get => _postalCode;
        set => SetProperty(ref _postalCode, value);
    }

    /// <summary>
    /// Gets or sets the phone number.
    /// </summary>
    public string PhoneNumber
    {
        get => _phoneNumber;
        set => SetProperty(ref _phoneNumber, value);
    }

    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    /// <summary>
    /// Gets or sets the GST number.
    /// </summary>
    public string GSTNumber
    {
        get => _gstNumber;
        set => SetProperty(ref _gstNumber, value);
    }

    /// <summary>
    /// Gets or sets the PAN number.
    /// </summary>
    public string PANNumber
    {
        get => _panNumber;
        set => SetProperty(ref _panNumber, value);
    }

    /// <summary>
    /// Gets or sets whether the customer is active.
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
    /// Loads the customer data for editing.
    /// </summary>
    private async Task LoadCustomerAsync(CancellationToken cancellationToken = default)
    {
        if (_customerId == 0)
        {
            ValidationError = "Customer ID is required.";
            return;
        }

        SetBusy("Loading customer...");
        var customer = await _customerQueryService.GetCustomerForEditAsync(_customerId, cancellationToken).ConfigureAwait(false);
        ClearBusy();

        if (customer is null)
        {
            ValidationError = "Customer not found.";
            return;
        }

        CustomerCode = customer.CustomerCode;
        CustomerName = customer.CustomerName;
        AddressLine1 = customer.AddressLine1;
        AddressLine2 = customer.AddressLine2;
        City = customer.City;
        State = customer.State;
        Country = customer.Country;
        PostalCode = customer.PostalCode;
        PhoneNumber = customer.PhoneNumber;
        Email = customer.Email;
        GSTNumber = customer.GSTNumber;
        PANNumber = customer.PANNumber;
        IsActive = customer.IsActive;
    }

    /// <summary>
    /// Command to save the customer.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidationError = string.Empty;

        var request = new UpdateCustomerRequest
        {
            CustomerId = _customerId,
            CustomerCode = CustomerCode,
            CustomerName = CustomerName,
            AddressLine1 = AddressLine1,
            AddressLine2 = AddressLine2,
            City = City,
            State = State,
            Country = Country,
            PostalCode = PostalCode,
            PhoneNumber = PhoneNumber,
            Email = Email,
            GSTNumber = GSTNumber,
            PANNumber = PANNumber,
            IsActive = IsActive
        };

        SetBusy("Updating customer...");
        var result = await _customerCommandService.UpdateCustomerAsync(request, CancellationToken.None).ConfigureAwait(false);
        ClearBusy();

        if (result.IsSuccess)
        {
            await _navigationService.GoBackAsync().ConfigureAwait(false);
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to update customer.";
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
