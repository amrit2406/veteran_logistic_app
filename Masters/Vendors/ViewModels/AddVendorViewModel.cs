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
    private string _code = string.Empty;
    private string _type = string.Empty;
    private string _name = string.Empty;
    private string _correspondenceAddress = string.Empty;
    private string _city = string.Empty;
    private string _billingAddress = string.Empty;
    private string _phone = string.Empty;
    private string _mobile = string.Empty;
    private string _fax = string.Empty;
    private string _email = string.Empty;
    private string _serviceTax = string.Empty;
    private string _cst = string.Empty;
    private string _pan = string.Empty;
    private string _gstin = string.Empty;
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
    /// Gets or sets the vendor code (auto-generated).
    /// </summary>
    public string Code
    {
        get => _code;
        set => SetProperty(ref _code, value);
    }

    /// <summary>
    /// Gets or sets the vendor type (Union/Vendor).
    /// </summary>
    public string Type
    {
        get => _type;
        set => SetProperty(ref _type, value);
    }

    /// <summary>
    /// Gets or sets the vendor name.
    /// </summary>
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    /// <summary>
    /// Gets or sets the correspondence address.
    /// </summary>
    public string CorrespondenceAddress
    {
        get => _correspondenceAddress;
        set => SetProperty(ref _correspondenceAddress, value);
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
    /// Gets or sets the billing address.
    /// </summary>
    public string BillingAddress
    {
        get => _billingAddress;
        set => SetProperty(ref _billingAddress, value);
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
    /// Gets or sets the fax number.
    /// </summary>
    public string Fax
    {
        get => _fax;
        set => SetProperty(ref _fax, value);
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
    /// Gets or sets the service tax.
    /// </summary>
    public string ServiceTax
    {
        get => _serviceTax;
        set => SetProperty(ref _serviceTax, value);
    }

    /// <summary>
    /// Gets or sets the CST number.
    /// </summary>
    public string CST
    {
        get => _cst;
        set => SetProperty(ref _cst, value);
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
    /// Gets or sets the GSTIN.
    /// </summary>
    public string GSTIN
    {
        get => _gstin;
        set => SetProperty(ref _gstin, value);
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
            Code = Code,
            Type = Type,
            Name = Name,
            CorrespondenceAddress = CorrespondenceAddress,
            City = City,
            BillingAddress = BillingAddress,
            Phone = Phone,
            Mobile = Mobile,
            Fax = Fax,
            Email = Email,
            ServiceTax = ServiceTax,
            CST = CST,
            PAN = PAN,
            GSTIN = GSTIN,
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
