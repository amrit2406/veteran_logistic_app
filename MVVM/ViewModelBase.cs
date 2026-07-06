using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using veteran_logistic.Navigation;

namespace veteran_logistic.MVVM;

/// <summary>
/// Base class for all ViewModels. Provides lightweight, reusable properties and lifecycle hooks.
/// </summary>
public abstract class ViewModelBase : ObservableObject
{
    private bool _isBusy;
    private string? _title;
    private string? _busyMessage;
    private bool _isInitialized;
    private NavigationParameter? _navigationParameter;

    /// <summary>
    /// Indicates whether the ViewModel is in a busy state. Bind UI elements to this property.
    /// </summary>
    public bool IsBusy
    {
        get => _isBusy;
        protected set => SetProperty(ref _isBusy, value);
    }

    /// <summary>
    /// Optional human-readable title for the ViewModel / View.
    /// </summary>
    public string? Title
    {
        get => _title;
        protected set => SetProperty(ref _title, value);
    }

    /// <summary>
    /// Optional message describing the current busy operation.
    /// </summary>
    public string? BusyMessage
    {
        get => _busyMessage;
        protected set => SetProperty(ref _busyMessage, value);
    }

    /// <summary>
    /// Indicates whether the ViewModel has been initialized.
    /// </summary>
    public bool IsInitialized
    {
        get => _isInitialized;
        protected set => SetProperty(ref _isInitialized, value);
    }

    /// <summary>
    /// Navigation parameter passed during navigation.
    /// </summary>
    public NavigationParameter? NavigationParameter
    {
        get => _navigationParameter;
        set => SetProperty(ref _navigationParameter, value);
    }

    /// <summary>
    /// Override to perform asynchronous initialization work.
    /// </summary>
    public virtual Task InitializeAsync(
    CancellationToken cancellationToken = default)
    {
        IsInitialized = true;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Override to perform cleanup when the ViewModel is being discarded.
    /// </summary>
    public virtual void Cleanup(
    CancellationToken cancellationToken = default)
    {
    }

    /// <summary>
    /// Set the ViewModel into a busy state with an optional message.
    /// </summary>
    /// <param name="message">Optional busy message.</param>
    protected void SetBusy(string? message = null)
    {
        BusyMessage = message;
        IsBusy = true;
    }

    /// <summary>
    /// Clear the busy state and message.
    /// </summary>
    protected void ClearBusy()
    {
        BusyMessage = null;
        IsBusy = false;
    }
}
