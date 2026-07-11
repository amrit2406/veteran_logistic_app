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

        return services;
    }
}
