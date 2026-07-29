using Microsoft.Extensions.DependencyInjection;
using veteran_logistic.Transactions.LoadingRegisters.Contracts;
using veteran_logistic.Transactions.LoadingRegisters.Services;
using veteran_logistic.Transactions.LoadingRegisters.Validators;
using veteran_logistic.Transactions.LoadingRegisters.ViewModels;
using veteran_logistic.Transactions.UnloadingRegisters.Contracts;
using veteran_logistic.Transactions.UnloadingRegisters.Services;
using veteran_logistic.Transactions.UnloadingRegisters.Validators;
using veteran_logistic.Transactions.UnloadingRegisters.ViewModels;
using veteran_logistic.Transactions.PaymentRegisters.Contracts;
using veteran_logistic.Transactions.PaymentRegisters.Services;
using veteran_logistic.Transactions.PaymentRegisters.Validators;
using veteran_logistic.Transactions.PaymentRegisters.ViewModels;
using veteran_logistic.Transactions.PartyBillRegister.Contracts;
using veteran_logistic.Transactions.PartyBillRegister.Services;
using veteran_logistic.Transactions.PartyBillRegister.Validators;
using veteran_logistic.Transactions.PartyBillRegister.ViewModels;
using veteran_logistic.Masters.PaymentLocations.Contracts;
using veteran_logistic.Masters.Customers.Contracts;
using veteran_logistic.Masters.SourceDestinations.Contracts;

namespace veteran_logistic.Transactions.DependencyInjection;

/// <summary>
/// Extension methods for registering Transactions feature infrastructure.
/// </summary>
public static class TransactionsServiceCollectionExtensions
{
    /// <summary>
    /// Adds Transactions feature infrastructure to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddTransactions(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        // Loading Register services
        services.AddScoped<ILoadingRegisterQueryService, LoadingRegisterQueryService>();
        services.AddScoped<ILoadingRegisterCommandService, LoadingRegisterCommandService>();
        services.AddScoped<ICreateLoadingRegisterValidator, CreateLoadingRegisterValidator>();
        services.AddScoped<IUpdateLoadingRegisterValidator, UpdateLoadingRegisterValidator>();
        services.AddScoped<IUpdateLoadingRegisterStatusValidator, UpdateLoadingRegisterStatusValidator>();
        services.AddScoped<IDeleteLoadingRegisterValidator, DeleteLoadingRegisterValidator>();
        services.AddTransient<LoadingRegistersViewModel>();
        services.AddTransient<AddLoadingRegisterViewModel>();
        services.AddTransient<EditLoadingRegisterViewModel>();

        // Unloading Register services
        services.AddScoped<IUnloadingRegisterQueryService, UnloadingRegisterQueryService>();
        services.AddScoped<IUnloadingRegisterCommandService, UnloadingRegisterCommandService>();
        services.AddScoped<ICreateUnloadingRegisterValidator, CreateUnloadingRegisterValidator>();
        services.AddScoped<IUpdateUnloadingRegisterValidator, UpdateUnloadingRegisterValidator>();
        services.AddScoped<IUpdateUnloadingRegisterStatusValidator, UpdateUnloadingRegisterStatusValidator>();
        services.AddScoped<IDeleteUnloadingRegisterValidator, DeleteUnloadingRegisterValidator>();
        services.AddTransient<UnloadingRegistersViewModel>();
        services.AddTransient<AddUnloadingRegisterViewModel>();
        services.AddTransient<EditUnloadingRegisterViewModel>();

        // Payment Register services
        services.AddScoped<IPaymentRegisterQueryService, PaymentRegisterQueryService>();
        services.AddScoped<IPaymentRegisterCommandService, PaymentRegisterCommandService>();
        services.AddScoped<ICreatePaymentRegisterValidator, CreatePaymentRegisterValidator>();
        services.AddScoped<IUpdatePaymentRegisterValidator, UpdatePaymentRegisterValidator>();
        services.AddScoped<IUpdatePaymentRegisterStatusValidator, UpdatePaymentRegisterStatusValidator>();
        services.AddScoped<IDeletePaymentRegisterValidator, DeletePaymentRegisterValidator>();
        services.AddTransient<PaymentRegistersViewModel>();
        services.AddTransient<AddPaymentRegisterViewModel>();
        services.AddTransient<EditPaymentRegisterViewModel>();

        // Party Bill Register services
        services.AddScoped<IPartyBillRegisterQueryService, PartyBillRegisterQueryService>();
        services.AddScoped<IPartyBillRegisterCommandService, PartyBillRegisterCommandService>();
        services.AddScoped<ICreatePartyBillRegisterValidator, CreatePartyBillRegisterValidator>();
        services.AddScoped<IUpdatePartyBillRegisterValidator, UpdatePartyBillRegisterValidator>();
        services.AddScoped<IUpdatePartyBillRegisterStatusValidator, UpdatePartyBillRegisterStatusValidator>();
        services.AddScoped<IDeletePartyBillRegisterValidator, DeletePartyBillRegisterValidator>();
        services.AddTransient<PartyBillRegistersViewModel>();
        services.AddTransient<AddPartyBillRegisterViewModel>();
        services.AddTransient<EditPartyBillRegisterViewModel>();

        return services;
    }
}
