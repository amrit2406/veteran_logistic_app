using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using veteran_logistic.Masters.SourceDestinations.Contracts;
using veteran_logistic.Masters.SourceDestinations.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.SourceDestinations.ViewModels;

/// <summary>
/// ViewModel for the Add Source/Destination screen.
/// </summary>
public sealed partial class AddSourceDestinationViewModel : ViewModelBase
{
    private readonly ISourceDestinationCommandService _sourceDestinationCommandService;
    private readonly INavigationService _navigationService;
    private string _locationName = string.Empty;
    private bool _isActive = true;
    private string _validationError = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddSourceDestinationViewModel"/> class.
    /// </summary>
    /// <param name="sourceDestinationCommandService">The source/destination command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public AddSourceDestinationViewModel(ISourceDestinationCommandService sourceDestinationCommandService, INavigationService navigationService)
    {
        _sourceDestinationCommandService = sourceDestinationCommandService ?? throw new ArgumentNullException(nameof(sourceDestinationCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        
        Title = "Add Source/Destination";
    }

    /// <summary>
    /// Gets or sets the source/destination name.
    /// </summary>
    public string LocationName
    {
        get => _locationName;
        set => SetProperty(ref _locationName, value);
    }

    /// <summary>
    /// Gets or sets whether the source/destination is active.
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
    /// Command to save the source/destination.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidationError = string.Empty;

        var request = new CreateSourceDestinationRequest
        {
            LocationName = LocationName,
            IsActive = IsActive
        };

        SetBusy("Creating source/destination...");
        var result = await _sourceDestinationCommandService.CreateSourceDestinationAsync(request).ConfigureAwait(false);
        ClearBusy();

        if (result.IsSuccess)
        {
            await _navigationService.GoBackAsync().ConfigureAwait(false);
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to create source/destination.";
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
