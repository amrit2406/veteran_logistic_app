using CommunityToolkit.Mvvm.ComponentModel;

namespace veteran_logistic.MVVM;

/// <summary>
/// Base class for ViewModels that require validation support. Builds on CommunityToolkit's ObservableValidator.
/// </summary>
public abstract class ObservableValidatorBase : ObservableValidator
{
    // Intentionally minimal: expose ObservableValidator functionality to ViewModels.
}
