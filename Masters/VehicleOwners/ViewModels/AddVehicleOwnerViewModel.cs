using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using veteran_logistic.Masters.VehicleOwners.Contracts;
using veteran_logistic.Masters.VehicleOwners.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.VehicleOwners.ViewModels;

/// <summary>
/// ViewModel for the Add Vehicle Owner screen.
/// </summary>
public sealed partial class AddVehicleOwnerViewModel : ViewModelBase
{
    private readonly IVehicleOwnerCommandService _vehicleOwnerCommandService;
    private readonly INavigationService _navigationService;
    private string _panType = string.Empty;
    private string _panNumber = string.Empty;
    private string _firstName = string.Empty;
    private string _middleName = string.Empty;
    private string _lastName = string.Empty;
    private string _companyName = string.Empty;
    private string _city = string.Empty;
    private string _state = string.Empty;
    private string _address = string.Empty;
    private string _phone = string.Empty;
    private string _mobile = string.Empty;
    private string _email = string.Empty;
    private string _fax = string.Empty;
    private bool _isActive = true;
    private string _validationError = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddVehicleOwnerViewModel"/> class.
    /// </summary>
    /// <param name="vehicleOwnerCommandService">The vehicle owner command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public AddVehicleOwnerViewModel(IVehicleOwnerCommandService vehicleOwnerCommandService, INavigationService navigationService)
    {
        _vehicleOwnerCommandService = vehicleOwnerCommandService ?? throw new ArgumentNullException(nameof(vehicleOwnerCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        
        Title = "Add Vehicle Owner";
    }

    /// <summary>
    /// Gets the PAN type options.
    /// </summary>
    public ObservableCollection<string> PANTypes { get; } = new ObservableCollection<string>
    {
        "Individual",
        "AOP",
        "AOP (Trust)",
        "Cooperative",
        "Company Not Public Interested",
        "Firm",
        "HUF"
    };

    /// <summary>
    /// Gets or sets the PAN type.
    /// </summary>
    public string PANType
    {
        get => _panType;
        set => SetProperty(ref _panType, value);
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
    /// Gets or sets the first name.
    /// </summary>
    public string FirstName
    {
        get => _firstName;
        set => SetProperty(ref _firstName, value);
    }

    /// <summary>
    /// Gets or sets the middle name.
    /// </summary>
    public string MiddleName
    {
        get => _middleName;
        set => SetProperty(ref _middleName, value);
    }

    /// <summary>
    /// Gets or sets the last name.
    /// </summary>
    public string LastName
    {
        get => _lastName;
        set => SetProperty(ref _lastName, value);
    }

    /// <summary>
    /// Gets or sets the company name.
    /// </summary>
    public string CompanyName
    {
        get => _companyName;
        set => SetProperty(ref _companyName, value);
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
    /// Gets or sets the address.
    /// </summary>
    public string Address
    {
        get => _address;
        set => SetProperty(ref _address, value);
    }

    /// <summary>
    /// Gets or sets the phone number.
    /// </summary>
    public string Phone
    {
        get => _phone;
        set => SetProperty(ref _phone, value);
    }

    /// <summary>
    /// Gets or sets the mobile number.
    /// </summary>
    public string Mobile
    {
        get => _mobile;
        set => SetProperty(ref _mobile, value);
    }

    /// <summary>
    /// Gets or sets the email address.
    /// </summary>
    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    /// <summary>
    /// Gets or sets the fax number.
    /// </summary>
    public string Fax
    {
        get => _fax;
        set => SetProperty(ref _fax, value);
    }

    /// <summary>
    /// Gets or sets whether the vehicle owner is active.
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
    /// Command to save the vehicle owner.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidationError = string.Empty;

        var request = new CreateVehicleOwnerRequest
        {
            PANType = PANType,
            PANNumber = PANNumber,
            FirstName = FirstName,
            MiddleName = MiddleName,
            LastName = LastName,
            CompanyName = CompanyName,
            City = City,
            State = State,
            Address = Address,
            Phone = Phone,
            Mobile = Mobile,
            Email = Email,
            Fax = Fax,
            IsActive = IsActive
        };

        SetBusy("Creating vehicle owner...");
        var result = await _vehicleOwnerCommandService.CreateVehicleOwnerAsync(request).ConfigureAwait(false);
        ClearBusy();

        if (result.IsSuccess)
        {
            await _navigationService.GoBackAsync().ConfigureAwait(false);
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to create vehicle owner.";
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
