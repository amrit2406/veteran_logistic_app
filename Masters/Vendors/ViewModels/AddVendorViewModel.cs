using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using veteran_logistic.Masters.Vendors.Contracts;
using veteran_logistic.Masters.Vendors.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.Vendors.ViewModels;

/// <summary>
/// ViewModel for the Add Vendor screen.
/// </summary>
public sealed partial class AddVendorViewModel : ViewModelBase
{
    private readonly IVendorCommandService _vendorCommandService;
    private readonly INavigationService _navigationService;
    private string _vendorCode = string.Empty;
    private string _vendorName = string.Empty;
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
    private string _contactPerson = string.Empty;
    private bool _isActive = true;
    private string _validationError = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddVendorViewModel"/> class.
    /// </summary>
    /// <param name="vendorCommandService">The vendor command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public AddVendorViewModel(IVendorCommandService vendorCommandService, INavigationService navigationService)
    {
        _vendorCommandService = vendorCommandService ?? throw new ArgumentNullException(nameof(vendorCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        
        Title = "Add Vendor";
    }

    /// <summary>
    /// Gets or sets the vendor code.
    /// </summary>
    public string VendorCode
    {
        get => _vendorCode;
        set => SetProperty(ref _vendorCode, value);
    }

    /// <summary>
    /// Gets or sets the vendor name.
    /// </summary>
    public string VendorName
    {
        get => _vendorName;
        set => SetProperty(ref _vendorName, value);
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
    /// Gets or sets the contact person.
    /// </summary>
    public string ContactPerson
    {
        get => _contactPerson;
        set => SetProperty(ref _contactPerson, value);
    }

    /// <summary>
    /// Gets or sets whether the vendor is active.
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
    /// Command to save the vendor.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidationError = string.Empty;

        var request = new CreateVendorRequest
        {
            VendorCode = VendorCode,
            VendorName = VendorName,
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
            ContactPerson = ContactPerson,
            IsActive = IsActive
        };

        SetBusy("Creating vendor...");
        var result = await _vendorCommandService.CreateVendorAsync(request).ConfigureAwait(false);
        ClearBusy();

        if (result.IsSuccess)
        {
            await _navigationService.GoBackAsync().ConfigureAwait(false);
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to create vendor.";
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
