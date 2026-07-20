using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using veteran_logistic.Masters.SourceDestinations.Contracts;
using veteran_logistic.Masters.SourceDestinations.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.SourceDestinations.ViewModels;

/// <summary>
/// ViewModel for the Edit Source/Destination screen.
/// </summary>
public sealed partial class EditSourceDestinationViewModel : ViewModelBase, INavigationAware
{
    private readonly ISourceDestinationCommandService _sourceDestinationCommandService;
    private readonly ISourceDestinationQueryService _sourceDestinationQueryService;
    private readonly INavigationService _navigationService;
    private int _sourceDestinationId;
    private string _locationName = string.Empty;
    private bool _isActive;
    private string _validationError = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditSourceDestinationViewModel"/> class.
    /// </summary>
    /// <param name="sourceDestinationCommandService">The source/destination command service.</param>
    /// <param name="sourceDestinationQueryService">The source/destination query service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public EditSourceDestinationViewModel(ISourceDestinationCommandService sourceDestinationCommandService, ISourceDestinationQueryService sourceDestinationQueryService, INavigationService navigationService)
    {
        _sourceDestinationCommandService = sourceDestinationCommandService ?? throw new ArgumentNullException(nameof(sourceDestinationCommandService));
        _sourceDestinationQueryService = sourceDestinationQueryService ?? throw new ArgumentNullException(nameof(sourceDestinationQueryService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        
        Title = "Edit Source/Destination";
    }

    public void OnNavigatedTo(NavigationParameter? parameter)
    {
        if (parameter is not null && parameter.TryGetValue<int>("SourceDestinationId", out var sourceDestinationId))
        {
            _sourceDestinationId = sourceDestinationId;
        }
    }

    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (IsInitialized)
        {
            return;
        }

        await LoadSourceDestinationAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
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
    /// Loads the source/destination data for editing.
    /// </summary>
    private async Task LoadSourceDestinationAsync(CancellationToken cancellationToken = default)
    {
        if (_sourceDestinationId == 0)
        {
            ValidationError = "Source/Destination ID is required.";
            return;
        }

        SetBusy("Loading source/destination...");
        var sourceDestination = await _sourceDestinationQueryService.GetSourceDestinationForEditAsync(_sourceDestinationId, cancellationToken).ConfigureAwait(false);
        ClearBusy();

        if (sourceDestination is null)
        {
            ValidationError = "Source/Destination not found.";
            return;
        }

        LocationName = sourceDestination.LocationName;
        IsActive = sourceDestination.IsActive;
    }

    /// <summary>
    /// Command to save the source/destination.
    /// </summary>
    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidationError = string.Empty;

        var request = new UpdateSourceDestinationRequest
        {
            SourceDestinationId = _sourceDestinationId,
            LocationName = LocationName,
            IsActive = IsActive
        };

        SetBusy("Updating source/destination...");
        var result = await _sourceDestinationCommandService.UpdateSourceDestinationAsync(request, CancellationToken.None).ConfigureAwait(false);
        ClearBusy();

        if (result.IsSuccess)
        {
            await _navigationService.GoBackAsync().ConfigureAwait(false);
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to update source/destination.";
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
