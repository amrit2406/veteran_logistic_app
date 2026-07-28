using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using veteran_logistic.Transactions.UnloadingRegisters.Contracts;
using veteran_logistic.Transactions.UnloadingRegisters.Models;
using veteran_logistic.Transactions.LoadingRegisters.Contracts;
using veteran_logistic.Transactions.LoadingRegisters.Models;
using veteran_logistic.Masters.Customers.Contracts;
using veteran_logistic.Masters.Customers.Models;
using veteran_logistic.Masters.SourceDestinations.Contracts;
using veteran_logistic.Masters.SourceDestinations.Models;
using veteran_logistic.Masters.Materials.Contracts;
using veteran_logistic.Masters.Materials.Models;
using veteran_logistic.Masters.PaymentLocations.Contracts;
using veteran_logistic.Masters.PaymentLocations.Models;
using veteran_logistic.Masters.Vendors.Contracts;
using veteran_logistic.Masters.Vendors.Models;
using veteran_logistic.Masters.Vehicles.Contracts;
using veteran_logistic.Masters.Vehicles.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Transactions.UnloadingRegisters.ViewModels;

/// <summary>
/// ViewModel for the Add Unloading Register screen.
/// </summary>
public sealed partial class AddUnloadingRegisterViewModel : ViewModelBase
{
    private readonly IUnloadingRegisterCommandService _unloadingRegisterCommandService;
    private readonly INavigationService _navigationService;
    private readonly ICustomerQueryService _customerQueryService;
    private readonly ISourceDestinationQueryService _sourceDestinationQueryService;
    private readonly IMaterialQueryService _materialQueryService;
    private readonly IPaymentLocationQueryService _paymentLocationQueryService;
    private readonly IVendorQueryService _vendorQueryService;
    private readonly IVehicleQueryService _vehicleQueryService;
    private readonly ILoadingRegisterQueryService _loadingRegisterQueryService;
    private int? _loadingRegisterId;
    private string _loadingRegisterChallanNumber = string.Empty;
    private string _searchChallanNumber = string.Empty;
    private int? _consignorId;
    private int? _consigneeId;
    private int? _sourceId;
    private int? _destinationId;
    private DateTime _unloadingDate = DateTime.Today;
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
    private decimal _grossWeightUL;
    private decimal _tareWeightUL;
    private decimal _unloadingWeight;
    private decimal _challanMoney;
    private IReadOnlyList<CustomerListItem> _customers = [];
    private IReadOnlyList<SourceDestinationListItem> _sourceDestinations = [];
    private IReadOnlyList<MaterialListItem> _materials = [];
    private IReadOnlyList<PaymentLocationListItem> _paymentLocations = [];
    private IReadOnlyList<VendorListItem> _vendors = [];
    private IReadOnlyList<VehicleListItem> _vehicles = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="AddUnloadingRegisterViewModel"/> class.
    /// </summary>
    /// <param name="unloadingRegisterCommandService">The unloading register command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public AddUnloadingRegisterViewModel(
        IUnloadingRegisterCommandService unloadingRegisterCommandService,
        INavigationService navigationService,
        ICustomerQueryService customerQueryService,
        ISourceDestinationQueryService sourceDestinationQueryService,
        IMaterialQueryService materialQueryService,
        IPaymentLocationQueryService paymentLocationQueryService,
        IVendorQueryService vendorQueryService,
        IVehicleQueryService vehicleQueryService,
        ILoadingRegisterQueryService loadingRegisterQueryService)
    {
        _unloadingRegisterCommandService = unloadingRegisterCommandService ?? throw new ArgumentNullException(nameof(unloadingRegisterCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        _customerQueryService = customerQueryService ?? throw new ArgumentNullException(nameof(customerQueryService));
        _sourceDestinationQueryService = sourceDestinationQueryService ?? throw new ArgumentNullException(nameof(sourceDestinationQueryService));
        _materialQueryService = materialQueryService ?? throw new ArgumentNullException(nameof(materialQueryService));
        _paymentLocationQueryService = paymentLocationQueryService ?? throw new ArgumentNullException(nameof(paymentLocationQueryService));
        _vendorQueryService = vendorQueryService ?? throw new ArgumentNullException(nameof(vendorQueryService));
        _vehicleQueryService = vehicleQueryService ?? throw new ArgumentNullException(nameof(vehicleQueryService));
        _loadingRegisterQueryService = loadingRegisterQueryService ?? throw new ArgumentNullException(nameof(loadingRegisterQueryService));

        Title = "Add Unloading Register";
    }

    /// <summary>
    /// Gets or sets the loading register ID.
    /// </summary>
    public int? LoadingRegisterId
    {
        get => _loadingRegisterId;
        set => SetProperty(ref _loadingRegisterId, value);
    }

    /// <summary>
    /// Gets or sets the loading register challan number (display only).
    /// </summary>
    public string LoadingRegisterChallanNumber
    {
        get => _loadingRegisterChallanNumber;
        set => SetProperty(ref _loadingRegisterChallanNumber, value);
    }

    /// <summary>
    /// Gets or sets the search challan number input.
    /// </summary>
    public string SearchChallanNumber
    {
        get => _searchChallanNumber;
        set => SetProperty(ref _searchChallanNumber, value);
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
    /// Gets or sets the unloading date.
    /// </summary>
    public DateTime UnloadingDate
    {
        get => _unloadingDate;
        set => SetProperty(ref _unloadingDate, value);
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
    /// Gets or sets the gross weight at unloading.
    /// </summary>
    public decimal GrossWeightUL
    {
        get => _grossWeightUL;
        set
        {
            if (SetProperty(ref _grossWeightUL, value))
            {
                CalculateUnloadingWeight();
            }
        }
    }

    /// <summary>
    /// Gets or sets the tare weight at unloading.
    /// </summary>
    public decimal TareWeightUL
    {
        get => _tareWeightUL;
        set
        {
            if (SetProperty(ref _tareWeightUL, value))
            {
                CalculateUnloadingWeight();
            }
        }
    }

    /// <summary>
    /// Gets or sets the unloading weight (calculated).
    /// </summary>
    public decimal UnloadingWeight
    {
        get => _unloadingWeight;
        private set => SetProperty(ref _unloadingWeight, value);
    }

    /// <summary>
    /// Gets or sets the challan money.
    /// </summary>
    public decimal ChallanMoney
    {
        get => _challanMoney;
        set => SetProperty(ref _challanMoney, value);
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
    /// Gets or sets whether the unloading register is active.
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
    /// Gets the collection of customers for consignor/consignee selection.
    /// </summary>
    public IReadOnlyList<CustomerListItem> Customers
    {
        get => _customers;
        private set => SetProperty(ref _customers, value);
    }

    /// <summary>
    /// Gets the collection of source/destinations.
    /// </summary>
    public IReadOnlyList<SourceDestinationListItem> SourceDestinations
    {
        get => _sourceDestinations;
        private set => SetProperty(ref _sourceDestinations, value);
    }

    /// <summary>
    /// Gets the collection of materials.
    /// </summary>
    public IReadOnlyList<MaterialListItem> Materials
    {
        get => _materials;
        private set => SetProperty(ref _materials, value);
    }

    /// <summary>
    /// Gets the collection of payment locations.
    /// </summary>
    public IReadOnlyList<PaymentLocationListItem> PaymentLocations
    {
        get => _paymentLocations;
        private set => SetProperty(ref _paymentLocations, value);
    }

    /// <summary>
    /// Gets the collection of vendors (union/vendors).
    /// </summary>
    public IReadOnlyList<VendorListItem> Vendors
    {
        get => _vendors;
        private set => SetProperty(ref _vendors, value);
    }

    /// <summary>
    /// Gets the collection of vehicles.
    /// </summary>
    public IReadOnlyList<VehicleListItem> Vehicles
    {
        get => _vehicles;
        private set => SetProperty(ref _vehicles, value);
    }

    /// <summary>
    /// Calculates the loading weight.
    /// </summary>
    private void CalculateLoadingWeight()
    {
        LoadingWeight = GrossWeight - TareWeight;
    }

    /// <summary>
    /// Calculates the unloading weight.
    /// </summary>
    private void CalculateUnloadingWeight()
    {
        UnloadingWeight = GrossWeightUL - TareWeightUL;
    }

    /// <summary>
    /// Calculates the gross amount.
    /// </summary>
    private void CalculateGrossAmount()
    {
        GrossAmount = LoadingWeight * Rate;
    }

    /// <summary>
    /// Loads master data for dropdowns.
    /// </summary>
    private async Task LoadMasterDataAsync()
    {
        try
        {
            Customers = await _customerQueryService.GetAllCustomersAsync();
            SourceDestinations = (await _sourceDestinationQueryService.GetAllSourceDestinationsAsync()).ToList();
            Materials = await _materialQueryService.GetAllMaterialsAsync();
            PaymentLocations = await _paymentLocationQueryService.GetAllPaymentLocationsAsync();
            Vendors = await _vendorQueryService.GetAllVendorsAsync();
            Vehicles = await _vehicleQueryService.GetAllVehiclesAsync();
        }
        catch (Exception ex)
        {
            ValidationError = $"Failed to load master data: {ex.Message}";
        }
    }

    /// <summary>
    /// Command to save the unloading register.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidationError = string.Empty;

        try
        {
            var request = new CreateUnloadingRegisterRequest
            {
                LoadingRegisterId = LoadingRegisterId,
                ConsignorId = ConsignorId,
                ConsigneeId = ConsigneeId,
                SourceId = SourceId,
                DestinationId = DestinationId,
                UnloadingDate = UnloadingDate,
                TPNumber = TPNumber,
                VehicleId = VehicleId,
                VehicleType = VehicleType,
                UnionVendorId = UnionVendorId,
                DriverCommission = DriverCommission,
                GrossWeight = GrossWeight,
                TareWeight = TareWeight,
                GrossWeightUL = GrossWeightUL,
                TareWeightUL = TareWeightUL,
                UnloadingWeight = UnloadingWeight,
                ChallanMoney = ChallanMoney,
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

            SetBusy("Creating unloading register...");
            var result = await _unloadingRegisterCommandService.CreateUnloadingRegisterAsync(request).ConfigureAwait(false);
            ClearBusy();

            if (result.IsSuccess)
            {
                await _navigationService.GoBackAsync().ConfigureAwait(false);
            }
            else
            {
                ValidationError = result.ErrorMessage ?? "Failed to create unloading register.";
            }
        }
        catch (Exception ex)
        {
            ClearBusy();
            ValidationError = $"Error: {ex.Message}";
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

    /// <summary>
    /// Loads data when the view is navigated to.
    /// </summary>
    public override async Task OnNavigatedToAsync(
        CancellationToken cancellationToken = default)
    {
        await LoadMasterDataAsync();
    }

    /// <summary>
    /// Command to search for a loading register by challan number.
    /// </summary>
    [RelayCommand]
    private async Task SearchLoadingRegisterAsync()
    {
        ValidationError = string.Empty;

        if (string.IsNullOrWhiteSpace(SearchChallanNumber))
        {
            ValidationError = "Please enter a challan number to search.";
            return;
        }

        try
        {
            SetBusy("Searching loading register...");
            var loadingRegister = await _loadingRegisterQueryService.GetLoadingRegisterByChallanNumberAsync(SearchChallanNumber.Trim()).ConfigureAwait(false);
            ClearBusy();

            if (loadingRegister == null)
            {
                ValidationError = $"Loading register with challan number '{SearchChallanNumber}' not found.";
                return;
            }

            // Load the loading register data into the form
            LoadingRegisterId = loadingRegister.Id;
            LoadingRegisterChallanNumber = loadingRegister.ChallanNumber;

            // Prefill fields from loading register
            ConsignorId = loadingRegister.ConsignorId;
            ConsigneeId = loadingRegister.ConsigneeId;
            SourceId = loadingRegister.SourceId;
            DestinationId = loadingRegister.DestinationId;
            TPNumber = loadingRegister.TPNumber;
            VehicleId = loadingRegister.VehicleId;
            VehicleType = loadingRegister.VehicleType;
            UnionVendorId = loadingRegister.UnionVendorId;
            DriverCommission = loadingRegister.DriverCommission;
            GrossWeight = loadingRegister.GrossWeight;
            TareWeight = loadingRegister.TareWeight;
            MaterialId = loadingRegister.MaterialId;
            Rate = loadingRegister.Rate;
            VehicleLoadedBy = loadingRegister.VehicleLoadedBy;
            FuelQuantity = loadingRegister.FuelQuantity;
            FuelAmount = loadingRegister.FuelAmount;
            FuelCash = loadingRegister.FuelCash;
            FuelAdvance = loadingRegister.FuelAdvance;
            ShortageWeight = loadingRegister.ShortageWeight;
            CashAdvance = loadingRegister.CashAdvance;
            PaymentLocationId = loadingRegister.PaymentLocationId;
            OtherAdvance = loadingRegister.OtherAdvance;
            OtherAdvanceDate = loadingRegister.OtherAdvanceDate;
            ThirdParty = loadingRegister.ThirdParty;
            OwnerId = loadingRegister.OwnerId;
            OwnerMobile = loadingRegister.OwnerMobile;
            OwnerAddress = loadingRegister.OwnerAddress;
            Driver = loadingRegister.Driver;
            DrivingLicenceNumber = loadingRegister.DrivingLicenceNumber;
            DriverMobile = loadingRegister.DriverMobile;
            Notes = loadingRegister.Notes;

            ValidationError = string.Empty;
        }
        catch (Exception ex)
        {
            ClearBusy();
            ValidationError = $"Error searching loading register: {ex.Message}";
        }
    }

    /// <summary>
    /// Command to clear the loading register data.
    /// </summary>
    [RelayCommand]
    private void ClearLoadingRegisterData()
    {
        LoadingRegisterId = null;
        LoadingRegisterChallanNumber = string.Empty;
        SearchChallanNumber = string.Empty;
    }
}
