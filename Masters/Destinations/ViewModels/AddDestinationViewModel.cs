using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using veteran_logistic.Masters.Destinations.Contracts;
using veteran_logistic.Masters.Destinations.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.Destinations.ViewModels;

/// <summary>
/// ViewModel for the Add Destination screen.
/// </summary>
public sealed partial class AddDestinationViewModel : ViewModelBase
{
    private readonly IDestinationCommandService _destinationCommandService;
    private readonly INavigationService _navigationService;
    private string _destinationCode = string.Empty;
    private string _destinationName = string.Empty;
    private string _addressLine1 = string.Empty;
    private string _addressLine2 = string.Empty;
    private string _city = string.Empty;
    private string _state = string.Empty;
    private string _country = string.Empty;
    private string _postalCode = string.Empty;
    private string _contactPerson = string.Empty;
    private string _phoneNumber = string.Empty;
    private string _email = string.Empty;
    private string _gstNumber = string.Empty;
    private decimal? _latitude;
    private decimal? _longitude;
    private string _remarks = string.Empty;
    private bool _isActive = true;
    private string _validationError = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddDestinationViewModel"/> class.
    /// </summary>
    /// <param name="destinationCommandService">The destination command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public AddDestinationViewModel(IDestinationCommandService destinationCommandService, INavigationService navigationService)
    {
        _destinationCommandService = destinationCommandService ?? throw new ArgumentNullException(nameof(destinationCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        
        Title = "Add Destination";
    }

    /// <summary>
    /// Gets or sets the destination code.
    /// </summary>
    public string DestinationCode
    {
        get => _destinationCode;
        set => SetProperty(ref _destinationCode, value);
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
    /// Gets or sets the contact person.
    /// </summary>
    public string ContactPerson
    {
        get => _contactPerson;
        set => SetProperty(ref _contactPerson, value);
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
    /// Gets or sets the latitude.
    /// </summary>
    public decimal? Latitude
    {
        get => _latitude;
        set => SetProperty(ref _latitude, value);
    }

    /// <summary>
    /// Gets or sets the longitude.
    /// </summary>
    public decimal? Longitude
    {
        get => _longitude;
        set => SetProperty(ref _longitude, value);
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
    /// Gets or sets whether the destination is active.
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
    /// Command to save the destination.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidationError = string.Empty;

        var request = new CreateDestinationRequest
        {
            DestinationCode = DestinationCode,
            DestinationName = DestinationName,
            AddressLine1 = AddressLine1,
            AddressLine2 = AddressLine2,
            City = City,
            State = State,
            Country = Country,
            PostalCode = PostalCode,
            ContactPerson = ContactPerson,
            PhoneNumber = PhoneNumber,
            Email = Email,
            GSTNumber = GSTNumber,
            Latitude = Latitude,
            Longitude = Longitude,
            Remarks = Remarks,
            IsActive = IsActive
        };

        SetBusy("Creating destination...");
        var result = await _destinationCommandService.CreateDestinationAsync(request).ConfigureAwait(false);
        ClearBusy();

        if (result.IsSuccess)
        {
            await _navigationService.GoBackAsync().ConfigureAwait(false);
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to create destination.";
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
