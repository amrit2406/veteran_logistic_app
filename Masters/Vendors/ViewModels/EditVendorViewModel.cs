using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using veteran_logistic.Masters.Vendors.Contracts;
using veteran_logistic.Masters.Vendors.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.Vendors.ViewModels;

/// <summary>
/// ViewModel for the Edit Vendor screen.
/// </summary>
public sealed partial class EditVendorViewModel : ViewModelBase, INavigationAware
{
    private readonly IVendorCommandService _vendorCommandService;
    private readonly IVendorQueryService _vendorQueryService;
    private readonly INavigationService _navigationService;
    private int _vendorId;
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
    private bool _isActive;
    private string _validationError = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditVendorViewModel"/> class.
    /// </summary>
    /// <param name="vendorCommandService">The vendor command service.</param>
    /// <param name="vendorQueryService">The vendor query service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public EditVendorViewModel(IVendorCommandService vendorCommandService, IVendorQueryService vendorQueryService, INavigationService navigationService)
    {
        _vendorCommandService = vendorCommandService ?? throw new ArgumentNullException(nameof(vendorCommandService));
        _vendorQueryService = vendorQueryService ?? throw new ArgumentNullException(nameof(vendorQueryService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        
        Title = "Edit Vendor";
    }

    public void OnNavigatedTo(NavigationParameter? parameter)
    {
        if (parameter is not null && parameter.TryGetValue<int>("VendorId", out var vendorId))
        {
            _vendorId = vendorId;
        }
    }

    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (IsInitialized)
        {
            return;
        }

        await LoadVendorAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
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
    /// Loads the vendor data for editing.
    /// </summary>
    private async Task LoadVendorAsync(CancellationToken cancellationToken = default)
    {
        if (_vendorId == 0)
        {
            ValidationError = "Vendor ID is required.";
            return;
        }

        SetBusy("Loading vendor...");
        var vendor = await _vendorQueryService.GetVendorForEditAsync(_vendorId, cancellationToken).ConfigureAwait(false);
        ClearBusy();

        if (vendor is null)
        {
            ValidationError = "Vendor not found.";
            return;
        }

        Code = vendor.Code;
        Type = vendor.Type;
        Name = vendor.Name;
        CorrespondenceAddress = vendor.CorrespondenceAddress;
        City = vendor.City;
        BillingAddress = vendor.BillingAddress;
        Phone = vendor.Phone;
        Mobile = vendor.Mobile;
        Fax = vendor.Fax;
        Email = vendor.Email;
        ServiceTax = vendor.ServiceTax;
        CST = vendor.CST;
        PAN = vendor.PAN;
        GSTIN = vendor.GSTIN;
        IsActive = vendor.IsActive;
    }

    /// <summary>
    /// Command to save the vendor.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidationError = string.Empty;

        var request = new UpdateVendorRequest
        {
            VendorId = _vendorId,
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

        SetBusy("Updating vendor...");
        var result = await _vendorCommandService.UpdateVendorAsync(request, CancellationToken.None).ConfigureAwait(false);
        ClearBusy();

        if (result.IsSuccess)
        {
            await _navigationService.GoBackAsync().ConfigureAwait(false);
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to update vendor.";
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
