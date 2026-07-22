using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using veteran_logistic.Transactions.LoadingRegisters.Contracts;
using veteran_logistic.Transactions.LoadingRegisters.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Transactions.LoadingRegisters.ViewModels;

/// <summary>
/// ViewModel for the Add Loading Register screen.
/// </summary>
public sealed partial class AddLoadingRegisterViewModel : ViewModelBase
{
    private readonly ILoadingRegisterCommandService _loadingRegisterCommandService;
    private readonly INavigationService _navigationService;
    private int? _consignorId;
    private int? _consigneeId;
    private int? _sourceId;
    private int? _destinationId;
    private DateTime _loadingDate = DateTime.Today;
    private string _tpNumber = string.Empty;
    private int? _vehicleId;
    private string _vehicleType = string.Empty;
    private int? _unionVendorId;
    private decimal _driverCommission;
    private decimal _grossWeight;
    private decimal _tareWeight;
    private int? _materialId;
    private decimal _rate;
    private string _vehicleLoadedBy = string.Empty;
    private decimal _fuelQuantity;
    private decimal _fuelAmount;
    private decimal _fuelCash;
    private decimal _fuelAdvance;
    private decimal _shortageWeight;
    private decimal _cashAdvance;
    private int? _paymentLocationId;
    private decimal _otherAdvance;
    private DateTime? _otherAdvanceDate;
    private string _thirdParty = string.Empty;
    private int? _ownerId;
    private string _ownerMobile = string.Empty;
    private string _ownerAddress = string.Empty;
    private string _driver = string.Empty;
    private string _drivingLicenceNumber = string.Empty;
    private string _driverMobile = string.Empty;
    private string _notes = string.Empty;
    private bool _isActive = true;
    private string _validationError = string.Empty;
    private decimal _loadingWeight;
    private decimal _grossAmount;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddLoadingRegisterViewModel"/> class.
    /// </summary>
    /// <param name="loadingRegisterCommandService">The loading register command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public AddLoadingRegisterViewModel(ILoadingRegisterCommandService loadingRegisterCommandService, INavigationService navigationService)
    {
        _loadingRegisterCommandService = loadingRegisterCommandService ?? throw new ArgumentNullException(nameof(loadingRegisterCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        
        Title = "Add Loading Register";
    }

    /// <summary>
    /// Gets or sets the consignor ID.
    /// </summary>
    public int? ConsignorId
    {
        get => _consignorId;
        set
        {
            if (SetProperty(ref _consignorId, value))
            {
                OnPropertyChanged(nameof(LoadingWeight));
                OnPropertyChanged(nameof(GrossAmount));
            }
        }
    }

    /// <summary>
    /// Gets or sets the consignee ID.
    /// </summary>
    public int? ConsigneeId
    {
        get => _consigneeId;
        set => SetProperty(ref _consigneeId, value);
    }

    /// <summary>
    /// Gets or sets the source ID.
    /// </summary>
    public int? SourceId
    {
        get => _sourceId;
        set => SetProperty(ref _sourceId, value);
    }

    /// <summary>
    /// Gets or sets the destination ID.
    /// </summary>
    public int? DestinationId
    {
        get => _destinationId;
        set => SetProperty(ref _destinationId, value);
    }

    /// <summary>
    /// Gets or sets the loading date.
    /// </summary>
    public DateTime LoadingDate
    {
        get => _loadingDate;
        set => SetProperty(ref _loadingDate, value);
    }

    /// <summary>
    /// Gets or sets the TP number.
    /// </summary>
    public string TPNumber
    {
        get => _tpNumber;
        set => SetProperty(ref _tpNumber, value);
    }

    /// <summary>
    /// Gets or sets the vehicle ID.
    /// </summary>
    public int? VehicleId
    {
        get => _vehicleId;
        set => SetProperty(ref _vehicleId, value);
    }

    /// <summary>
    /// Gets or sets the vehicle type.
    /// </summary>
    public string VehicleType
    {
        get => _vehicleType;
        set => SetProperty(ref _vehicleType, value);
    }

    /// <summary>
    /// Gets or sets the union/vendor ID.
    /// </summary>
    public int? UnionVendorId
    {
        get => _unionVendorId;
        set => SetProperty(ref _unionVendorId, value);
    }

    /// <summary>
    /// Gets or sets the driver commission.
    /// </summary>
    public decimal DriverCommission
    {
        get => _driverCommission;
        set => SetProperty(ref _driverCommission, value);
    }

    /// <summary>
    /// Gets or sets the gross weight.
    /// </summary>
    public decimal GrossWeight
    {
        get => _grossWeight;
        set
        {
            if (SetProperty(ref _grossWeight, value))
            {
                CalculateLoadingWeight();
                CalculateGrossAmount();
            }
        }
    }

    /// <summary>
    /// Gets or sets the tare weight.
    /// </summary>
    public decimal TareWeight
    {
        get => _tareWeight;
        set
        {
            if (SetProperty(ref _tareWeight, value))
            {
                CalculateLoadingWeight();
                CalculateGrossAmount();
            }
        }
    }

    /// <summary>
    /// Gets or sets the loading weight (calculated).
    /// </summary>
    public decimal LoadingWeight
    {
        get => _loadingWeight;
        private set => SetProperty(ref _loadingWeight, value);
    }

    /// <summary>
    /// Gets or sets the material ID.
    /// </summary>
    public int? MaterialId
    {
        get => _materialId;
        set => SetProperty(ref _materialId, value);
    }

    /// <summary>
    /// Gets or sets the rate.
    /// </summary>
    public decimal Rate
    {
        get => _rate;
        set
        {
            if (SetProperty(ref _rate, value))
            {
                CalculateGrossAmount();
            }
        }
    }

    /// <summary>
    /// Gets or sets the gross amount (calculated).
    /// </summary>
    public decimal GrossAmount
    {
        get => _grossAmount;
        private set => SetProperty(ref _grossAmount, value);
    }

    /// <summary>
    /// Gets or sets who loaded the vehicle.
    /// </summary>
    public string VehicleLoadedBy
    {
        get => _vehicleLoadedBy;
        set => SetProperty(ref _vehicleLoadedBy, value);
    }

    /// <summary>
    /// Gets or sets the fuel quantity.
    /// </summary>
    public decimal FuelQuantity
    {
        get => _fuelQuantity;
        set => SetProperty(ref _fuelQuantity, value);
    }

    /// <summary>
    /// Gets or sets the fuel amount.
    /// </summary>
    public decimal FuelAmount
    {
        get => _fuelAmount;
        set => SetProperty(ref _fuelAmount, value);
    }

    /// <summary>
    /// Gets or sets the fuel cash.
    /// </summary>
    public decimal FuelCash
    {
        get => _fuelCash;
        set => SetProperty(ref _fuelCash, value);
    }

    /// <summary>
    /// Gets or sets the fuel advance.
    /// </summary>
    public decimal FuelAdvance
    {
        get => _fuelAdvance;
        set => SetProperty(ref _fuelAdvance, value);
    }

    /// <summary>
    /// Gets or sets the shortage weight.
    /// </summary>
    public decimal ShortageWeight
    {
        get => _shortageWeight;
        set => SetProperty(ref _shortageWeight, value);
    }

    /// <summary>
    /// Gets or sets the cash advance.
    /// </summary>
    public decimal CashAdvance
    {
        get => _cashAdvance;
        set => SetProperty(ref _cashAdvance, value);
    }

    /// <summary>
    /// Gets or sets the payment location ID.
    /// </summary>
    public int? PaymentLocationId
    {
        get => _paymentLocationId;
        set => SetProperty(ref _paymentLocationId, value);
    }

    /// <summary>
    /// Gets or sets the other advance.
    /// </summary>
    public decimal OtherAdvance
    {
        get => _otherAdvance;
        set => SetProperty(ref _otherAdvance, value);
    }

    /// <summary>
    /// Gets or sets the other advance date.
    /// </summary>
    public DateTime? OtherAdvanceDate
    {
        get => _otherAdvanceDate;
        set => SetProperty(ref _otherAdvanceDate, value);
    }

    /// <summary>
    /// Gets or sets the third party.
    /// </summary>
    public string ThirdParty
    {
        get => _thirdParty;
        set => SetProperty(ref _thirdParty, value);
    }

    /// <summary>
    /// Gets or sets the owner ID.
    /// </summary>
    public int? OwnerId
    {
        get => _ownerId;
        set => SetProperty(ref _ownerId, value);
    }

    /// <summary>
    /// Gets or sets the owner mobile.
    /// </summary>
    public string OwnerMobile
    {
        get => _ownerMobile;
        set => SetProperty(ref _ownerMobile, value);
    }

    /// <summary>
    /// Gets or sets the owner address.
    /// </summary>
    public string OwnerAddress
    {
        get => _ownerAddress;
        set => SetProperty(ref _ownerAddress, value);
    }

    /// <summary>
    /// Gets or sets the driver name.
    /// </summary>
    public string Driver
    {
        get => _driver;
        set => SetProperty(ref _driver, value);
    }

    /// <summary>
    /// Gets or sets the driving licence number.
    /// </summary>
    public string DrivingLicenceNumber
    {
        get => _drivingLicenceNumber;
        set => SetProperty(ref _drivingLicenceNumber, value);
    }

    /// <summary>
    /// Gets or sets the driver mobile.
    /// </summary>
    public string DriverMobile
    {
        get => _driverMobile;
        set => SetProperty(ref _driverMobile, value);
    }

    /// <summary>
    /// Gets or sets the notes.
    /// </summary>
    public string Notes
    {
        get => _notes;
        set => SetProperty(ref _notes, value);
    }

    /// <summary>
    /// Gets or sets whether the loading register is active.
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
    /// Calculates the loading weight.
    /// </summary>
    private void CalculateLoadingWeight()
    {
        LoadingWeight = GrossWeight - TareWeight;
    }

    /// <summary>
    /// Calculates the gross amount.
    /// </summary>
    private void CalculateGrossAmount()
    {
        GrossAmount = LoadingWeight * Rate;
    }

    /// <summary>
    /// Command to save the loading register.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidationError = string.Empty;

        var request = new CreateLoadingRegisterRequest
        {
            ConsignorId = ConsignorId,
            ConsigneeId = ConsigneeId,
            SourceId = SourceId,
            DestinationId = DestinationId,
            LoadingDate = LoadingDate,
            TPNumber = TPNumber,
            VehicleId = VehicleId,
            VehicleType = VehicleType,
            UnionVendorId = UnionVendorId,
            DriverCommission = DriverCommission,
            GrossWeight = GrossWeight,
            TareWeight = TareWeight,
            MaterialId = MaterialId,
            Rate = Rate,
            VehicleLoadedBy = VehicleLoadedBy,
            FuelQuantity = FuelQuantity,
            FuelAmount = FuelAmount,
            FuelCash = FuelCash,
            FuelAdvance = FuelAdvance,
            ShortageWeight = ShortageWeight,
            CashAdvance = CashAdvance,
            PaymentLocationId = PaymentLocationId,
            OtherAdvance = OtherAdvance,
            OtherAdvanceDate = OtherAdvanceDate,
            ThirdParty = ThirdParty,
            OwnerId = OwnerId,
            OwnerMobile = OwnerMobile,
            OwnerAddress = OwnerAddress,
            Driver = Driver,
            DrivingLicenceNumber = DrivingLicenceNumber,
            DriverMobile = DriverMobile,
            Notes = Notes,
            IsActive = IsActive
        };

        SetBusy("Creating loading register...");
        var result = await _loadingRegisterCommandService.CreateLoadingRegisterAsync(request).ConfigureAwait(false);
        ClearBusy();

        if (result.IsSuccess)
        {
            await _navigationService.GoBackAsync().ConfigureAwait(false);
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to create loading register.";
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
