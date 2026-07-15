using Microsoft.Extensions.DependencyInjection;
using veteran_logistic.Masters.Companies.Contracts;
using veteran_logistic.Masters.Companies.Services;
using veteran_logistic.Masters.Companies.Validators;
using veteran_logistic.Masters.Companies.ViewModels;
using veteran_logistic.Masters.Customers.Contracts;
using veteran_logistic.Masters.Customers.Services;
using veteran_logistic.Masters.Customers.Validators;
using veteran_logistic.Masters.Customers.ViewModels;
using veteran_logistic.Masters.Vendors.Contracts;
using veteran_logistic.Masters.Vendors.Services;
using veteran_logistic.Masters.Vendors.Validators;
using veteran_logistic.Masters.Vendors.ViewModels;
using veteran_logistic.Masters.Sources.Contracts;
using veteran_logistic.Masters.Sources.Services;
using veteran_logistic.Masters.Sources.Validators;
using veteran_logistic.Masters.Sources.ViewModels;
using veteran_logistic.Masters.Destinations.Contracts;
using veteran_logistic.Masters.Destinations.Services;
using veteran_logistic.Masters.Destinations.Validators;
using veteran_logistic.Masters.Destinations.ViewModels;
using veteran_logistic.Masters.Materials.Contracts;
using veteran_logistic.Masters.Materials.Services;
using veteran_logistic.Masters.Materials.Validators;
using veteran_logistic.Masters.Materials.ViewModels;
using veteran_logistic.Masters.FuelPumps.Contracts;
using veteran_logistic.Masters.FuelPumps.Services;
using veteran_logistic.Masters.FuelPumps.Validators;
using veteran_logistic.Masters.FuelPumps.ViewModels;
using veteran_logistic.Masters.HsdRates.Contracts;
using veteran_logistic.Masters.HsdRates.Services;
using veteran_logistic.Masters.HsdRates.Validators;
using veteran_logistic.Masters.HsdRates.ViewModels;
using veteran_logistic.Masters.PaymentLocations.Contracts;
using veteran_logistic.Masters.PaymentLocations.Services;
using veteran_logistic.Masters.PaymentLocations.Validators;
using veteran_logistic.Masters.PaymentLocations.ViewModels;
using veteran_logistic.Masters.VehicleOwners.Contracts;
using veteran_logistic.Masters.VehicleOwners.Services;
using veteran_logistic.Masters.VehicleOwners.Validators;
using veteran_logistic.Masters.VehicleOwners.ViewModels;
using veteran_logistic.Masters.Vehicles.Contracts;
using veteran_logistic.Masters.Vehicles.Services;
using veteran_logistic.Masters.Vehicles.Validators;
using veteran_logistic.Masters.Vehicles.ViewModels;
using veteran_logistic.Masters.VehicleAssignments.Contracts;
using veteran_logistic.Masters.VehicleAssignments.Services;
using veteran_logistic.Masters.VehicleAssignments.Validators;
using veteran_logistic.Masters.VehicleAssignments.ViewModels;

namespace veteran_logistic.Masters.DependencyInjection;

/// <summary>
/// Extension methods for registering Masters feature infrastructure.
/// </summary>
public static class MastersServiceCollectionExtensions
{
    /// <summary>
    /// Adds Masters feature infrastructure to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddMasters(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Company services
        services.AddScoped<ICompanyQueryService, CompanyQueryService>();
        services.AddScoped<ICompanyCommandService, CompanyCommandService>();
        services.AddScoped<ICreateCompanyValidator, CreateCompanyValidator>();
        services.AddScoped<IUpdateCompanyValidator, UpdateCompanyValidator>();
        services.AddScoped<IUpdateCompanyStatusValidator, UpdateCompanyStatusValidator>();
        services.AddScoped<IDeleteCompanyValidator, DeleteCompanyValidator>();
        services.AddTransient<CompaniesViewModel>();
        services.AddTransient<AddCompanyViewModel>();
        services.AddTransient<EditCompanyViewModel>();

        // Customer services
        services.AddScoped<ICustomerQueryService, CustomerQueryService>();
        services.AddScoped<ICustomerCommandService, CustomerCommandService>();
        services.AddScoped<ICreateCustomerValidator, CreateCustomerValidator>();
        services.AddScoped<IUpdateCustomerValidator, UpdateCustomerValidator>();
        services.AddScoped<IUpdateCustomerStatusValidator, UpdateCustomerStatusValidator>();
        services.AddScoped<IDeleteCustomerValidator, DeleteCustomerValidator>();
        services.AddTransient<CustomersViewModel>();
        services.AddTransient<AddCustomerViewModel>();
        services.AddTransient<EditCustomerViewModel>();

        // Vendor services
        services.AddScoped<IVendorQueryService, VendorQueryService>();
        services.AddScoped<IVendorCommandService, VendorCommandService>();
        services.AddScoped<ICreateVendorValidator, CreateVendorValidator>();
        services.AddScoped<IUpdateVendorValidator, UpdateVendorValidator>();
        services.AddScoped<IUpdateVendorStatusValidator, UpdateVendorStatusValidator>();
        services.AddScoped<IDeleteVendorValidator, DeleteVendorValidator>();
        services.AddTransient<VendorsViewModel>();
        services.AddTransient<AddVendorViewModel>();
        services.AddTransient<EditVendorViewModel>();

        // Source services
        services.AddScoped<ISourceQueryService, SourceQueryService>();
        services.AddScoped<ISourceCommandService, SourceCommandService>();
        services.AddScoped<ICreateSourceValidator, CreateSourceValidator>();
        services.AddScoped<IUpdateSourceValidator, UpdateSourceValidator>();
        services.AddScoped<IUpdateSourceStatusValidator, UpdateSourceStatusValidator>();
        services.AddScoped<IDeleteSourceValidator, DeleteSourceValidator>();
        services.AddTransient<SourcesViewModel>();
        services.AddTransient<AddSourceViewModel>();
        services.AddTransient<EditSourceViewModel>();

        // Destination services
        services.AddScoped<IDestinationQueryService, DestinationQueryService>();
        services.AddScoped<IDestinationCommandService, DestinationCommandService>();
        services.AddScoped<ICreateDestinationValidator, CreateDestinationValidator>();
        services.AddScoped<IUpdateDestinationValidator, UpdateDestinationValidator>();
        services.AddScoped<IUpdateDestinationStatusValidator, UpdateDestinationStatusValidator>();
        services.AddScoped<IDeleteDestinationValidator, DeleteDestinationValidator>();
        services.AddTransient<DestinationsViewModel>();
        services.AddTransient<AddDestinationViewModel>();
        services.AddTransient<EditDestinationViewModel>();

        // Material services
        services.AddScoped<IMaterialQueryService, MaterialQueryService>();
        services.AddScoped<IMaterialCommandService, MaterialCommandService>();
        services.AddScoped<ICreateMaterialValidator, CreateMaterialValidator>();
        services.AddScoped<IUpdateMaterialValidator, UpdateMaterialValidator>();
        services.AddScoped<IUpdateMaterialStatusValidator, UpdateMaterialStatusValidator>();
        services.AddScoped<IDeleteMaterialValidator, DeleteMaterialValidator>();
        services.AddTransient<MaterialsViewModel>();
        services.AddTransient<AddMaterialViewModel>();
        services.AddTransient<EditMaterialViewModel>();

        // Fuel Pump services
        services.AddScoped<IFuelPumpQueryService, FuelPumpQueryService>();
        services.AddScoped<IFuelPumpCommandService, FuelPumpCommandService>();
        services.AddScoped<ICreateFuelPumpValidator, CreateFuelPumpValidator>();
        services.AddScoped<IUpdateFuelPumpValidator, UpdateFuelPumpValidator>();
        services.AddScoped<IUpdateFuelPumpStatusValidator, UpdateFuelPumpStatusValidator>();
        services.AddScoped<IDeleteFuelPumpValidator, DeleteFuelPumpValidator>();
        services.AddTransient<FuelPumpsViewModel>();
        services.AddTransient<AddFuelPumpViewModel>();
        services.AddTransient<EditFuelPumpViewModel>();

        // HSD Rate services
        services.AddScoped<IHsdRateQueryService, HsdRateQueryService>();
        services.AddScoped<IHsdRateCommandService, HsdRateCommandService>();
        services.AddScoped<ICreateHsdRateValidator, CreateHsdRateValidator>();
        services.AddScoped<IUpdateHsdRateValidator, UpdateHsdRateValidator>();
        services.AddScoped<IUpdateHsdRateStatusValidator, UpdateHsdRateStatusValidator>();
        services.AddScoped<IDeleteHsdRateValidator, DeleteHsdRateValidator>();
        services.AddTransient<HsdRatesViewModel>();
        services.AddTransient<AddHsdRateViewModel>();
        services.AddTransient<EditHsdRateViewModel>();

        // Payment Location services
        services.AddScoped<IPaymentLocationQueryService, PaymentLocationQueryService>();
        services.AddScoped<IPaymentLocationCommandService, PaymentLocationCommandService>();
        services.AddScoped<ICreatePaymentLocationValidator, CreatePaymentLocationValidator>();
        services.AddScoped<IUpdatePaymentLocationValidator, UpdatePaymentLocationValidator>();
        services.AddScoped<IUpdatePaymentLocationStatusValidator, UpdatePaymentLocationStatusValidator>();
        services.AddScoped<IDeletePaymentLocationValidator, DeletePaymentLocationValidator>();
        services.AddTransient<PaymentLocationsViewModel>();
        services.AddTransient<AddPaymentLocationViewModel>();
        services.AddTransient<EditPaymentLocationViewModel>();

        // Vehicle Owner services
        services.AddScoped<IVehicleOwnerQueryService, VehicleOwnerQueryService>();
        services.AddScoped<IVehicleOwnerCommandService, VehicleOwnerCommandService>();
        services.AddScoped<ICreateVehicleOwnerValidator, CreateVehicleOwnerValidator>();
        services.AddScoped<IUpdateVehicleOwnerValidator, UpdateVehicleOwnerValidator>();
        services.AddScoped<IUpdateVehicleOwnerStatusValidator, UpdateVehicleOwnerStatusValidator>();
        services.AddScoped<IDeleteVehicleOwnerValidator, DeleteVehicleOwnerValidator>();
        services.AddTransient<VehicleOwnersViewModel>();
        services.AddTransient<AddVehicleOwnerViewModel>();
        services.AddTransient<EditVehicleOwnerViewModel>();

        // Vehicle services
        services.AddScoped<IVehicleQueryService, VehicleQueryService>();
        services.AddScoped<IVehicleCommandService, VehicleCommandService>();
        services.AddScoped<ICreateVehicleValidator, CreateVehicleValidator>();
        services.AddScoped<IUpdateVehicleValidator, UpdateVehicleValidator>();
        services.AddScoped<IUpdateVehicleStatusValidator, UpdateVehicleStatusValidator>();
        services.AddScoped<IDeleteVehicleValidator, DeleteVehicleValidator>();
        services.AddTransient<VehiclesViewModel>();
        services.AddTransient<AddVehicleViewModel>();
        services.AddTransient<EditVehicleViewModel>();

        // Vehicle Assignment services
        services.AddScoped<IVehicleAssignmentQueryService, VehicleAssignmentQueryService>();
        services.AddScoped<IVehicleAssignmentCommandService, VehicleAssignmentCommandService>();
        services.AddScoped<IAssignVehicleValidator, AssignVehicleValidator>();
        services.AddScoped<IUpdateVehicleAssignmentValidator, UpdateVehicleAssignmentValidator>();
        services.AddScoped<IReleaseVehicleValidator, ReleaseVehicleValidator>();
        services.AddScoped<IDeleteVehicleAssignmentValidator, DeleteVehicleAssignmentValidator>();
        services.AddTransient<VehicleAssignmentsViewModel>();

        return services;
    }
}
