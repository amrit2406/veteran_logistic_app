using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using veteran_logistic.Masters.Destinations.Contracts;
using veteran_logistic.Masters.Destinations.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.Destinations.ViewModels;

/// <summary>
/// ViewModel for the Edit Destination screen.
/// </summary>
public sealed partial class EditDestinationViewModel : ViewModelBase, INavigationAware
{
    private readonly IDestinationCommandService _destinationCommandService;
    private readonly IDestinationQueryService _destinationQueryService;
    private readonly INavigationService _navigationService;
    private int _destinationId;
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
    private bool _isActive;
    private string _validationError = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditDestinationViewModel"/> class.
    /// </summary>
    /// <param name="destinationCommandService">The destination command service.</param>
    /// <param name="destinationQueryService">The destination query service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public EditDestinationViewModel(IDestinationCommandService destinationCommandService, IDestinationQueryService destinationQueryService, INavigationService navigationService)
    {
        _destinationCommandService = destinationCommandService ?? throw new ArgumentNullException(nameof(destinationCommandService));
        _destinationQueryService = destinationQueryService ?? throw new ArgumentNullException(nameof(destinationQueryService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        
        Title = "Edit Destination";
    }

    public void OnNavigatedTo(NavigationParameter? parameter)
    {
        if (parameter is not null && parameter.TryGetValue<int>("DestinationId", out var destinationId))
        {
            _destinationId = destinationId;
        }
    }

    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (IsInitialized)
        {
            return;
        }

        await LoadDestinationAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
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
    /// Loads the destination data for editing.
    /// </summary>
    private async Task LoadDestinationAsync(CancellationToken cancellationToken = default)
    {
        if (_destinationId == 0)
        {
            ValidationError = "Destination ID is required.";
            return;
        }

        SetBusy("Loading destination...");
        var destination = await _destinationQueryService.GetDestinationForEditAsync(_destinationId, cancellationToken).ConfigureAwait(false);
        ClearBusy();

        if (destination is null)
        {
            ValidationError = "Destination not found.";
            return;
        }

        DestinationCode = destination.DestinationCode;
        DestinationName = destination.DestinationName;
        AddressLine1 = destination.AddressLine1;
        AddressLine2 = destination.AddressLine2;
        City = destination.City;
        State = destination.State;
        Country = destination.Country;
        PostalCode = destination.PostalCode;
        ContactPerson = destination.ContactPerson;
        PhoneNumber = destination.PhoneNumber;
        Email = destination.Email;
        GSTNumber = destination.GSTNumber;
        Latitude = destination.Latitude;
        Longitude = destination.Longitude;
        Remarks = destination.Remarks;
        IsActive = destination.IsActive;
    }

    /// <summary>
    /// Command to save the destination.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidationError = string.Empty;

        var request = new UpdateDestinationRequest
        {
            DestinationId = _destinationId,
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

        SetBusy("Updating destination...");
        var result = await _destinationCommandService.UpdateDestinationAsync(request, CancellationToken.None).ConfigureAwait(false);
        ClearBusy();

        if (result.IsSuccess)
        {
            await _navigationService.GoBackAsync().ConfigureAwait(false);
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to update destination.";
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
