using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using veteran_logistic.Masters.Materials.Contracts;
using veteran_logistic.Masters.Materials.Models;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Masters.Materials.ViewModels;

/// <summary>
/// ViewModel for the Materials listing screen.
/// </summary>
public sealed partial class MaterialsViewModel : ViewModelBase
{
    private readonly IMaterialQueryService _materialQueryService;
    private readonly IMaterialCommandService _materialCommandService;
    private readonly INavigationService _navigationService;
    private string _searchText = string.Empty;
    private MaterialListItem? _selectedMaterial;
    private string _validationError = string.Empty;
    private CancellationTokenSource? _searchCancellationTokenSource;

    /// <summary>
    /// Initializes a new instance of the <see cref="MaterialsViewModel"/> class.
    /// </summary>
    /// <param name="materialQueryService">The material query service.</param>
    /// <param name="materialCommandService">The material command service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public MaterialsViewModel(IMaterialQueryService materialQueryService, IMaterialCommandService materialCommandService, INavigationService navigationService)
    {
        _materialQueryService = materialQueryService ?? throw new ArgumentNullException(nameof(materialQueryService));
        _materialCommandService = materialCommandService ?? throw new ArgumentNullException(nameof(materialCommandService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

        Title = "Materials";
    }

    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (IsInitialized)
        {
            return;
        }

        await LoadMaterialsAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
        await LoadMaterialsAsync(cancellationToken);
    }

    /// <summary>
    /// Gets the collection of materials to display.
    /// </summary>
    public ObservableCollection<MaterialListItem> Materials { get; } = new();

    /// <summary>
    /// Gets or sets the search text.
    /// </summary>
    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value))
            {
                _ = DebouncedSearchAsync();
            }
        }
    }

    /// <summary>
    /// Gets or sets the selected material.
    /// </summary>
    public MaterialListItem? SelectedMaterial
    {
        get => _selectedMaterial;
        set
        {
            if (SetProperty(ref _selectedMaterial, value))
            {
                EditMaterialCommand.NotifyCanExecuteChanged();
                ActivateMaterialCommand.NotifyCanExecuteChanged();
                DeactivateMaterialCommand.NotifyCanExecuteChanged();
                DeleteMaterialCommand.NotifyCanExecuteChanged();
            }
        }
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
    /// Command to refresh the material list.
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadMaterialsAsync();
    }

    /// <summary>
    /// Command to navigate to the Add Material screen.
    /// </summary>
    [RelayCommand]
    private async Task AddMaterialAsync()
    {
        await _navigationService.NavigateAsync<AddMaterialViewModel>().ConfigureAwait(false);
    }

    /// <summary>
    /// Command to navigate to the Edit Material screen.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteMaterialCommand))]
    private async Task EditMaterialAsync()
    {
        if (SelectedMaterial is null)
        {
            return;
        }

        var parameter = new NavigationParameter
        {
            ["MaterialId"] = SelectedMaterial.Id
        };

        await _navigationService.NavigateAsync<EditMaterialViewModel>(parameter).ConfigureAwait(false);
    }

    /// <summary>
    /// Command to activate the selected material.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteMaterialCommand))]
    private async Task ActivateMaterialAsync()
    {
        if (SelectedMaterial is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdateMaterialStatusRequest
        {
            MaterialId = SelectedMaterial.Id,
            IsActive = true
        };

        SetBusy("Activating material...");
        var result = await _materialCommandService.UpdateMaterialStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleMaterialStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to activate material.";
        }
    }

    /// <summary>
    /// Command to deactivate the selected material.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteMaterialCommand))]
    private async Task DeactivateMaterialAsync()
    {
        if (SelectedMaterial is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var request = new UpdateMaterialStatusRequest
        {
            MaterialId = SelectedMaterial.Id,
            IsActive = false
        };

        SetBusy("Deactivating material...");
        var result = await _materialCommandService.UpdateMaterialStatusAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await HandleMaterialStatusUpdateSuccess();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to deactivate material.";
        }
    }

    private async Task HandleMaterialStatusUpdateSuccess()
    {
        await LoadMaterialsAsync();
        SelectedMaterial = null;
        ActivateMaterialCommand.NotifyCanExecuteChanged();
        DeactivateMaterialCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Command to delete the selected material.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteMaterialCommand))]
    private async Task DeleteMaterialAsync()
    {
        if (SelectedMaterial is null)
        {
            return;
        }

        ValidationError = string.Empty;

        var messageBoxResult = MessageBox.Show(
            "Are you sure you want to delete this material?\n\nThis action hides the material from the application.",
            "Delete Material",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (messageBoxResult != MessageBoxResult.Yes)
        {
            return;
        }

        var request = new DeleteMaterialRequest
        {
            MaterialId = SelectedMaterial.Id
        };

        SetBusy("Deleting material...");
        var result = await _materialCommandService.DeleteMaterialAsync(request, CancellationToken.None);
        ClearBusy();

        if (result.IsSuccess)
        {
            await LoadMaterialsAsync();
            SelectedMaterial = null;
            DeleteMaterialCommand.NotifyCanExecuteChanged();
        }
        else
        {
            ValidationError = result.ErrorMessage ?? "Failed to delete material.";
        }
    }

    private bool CanExecuteMaterialCommand()
    {
        return SelectedMaterial is not null;
    }

    /// <summary>
    /// Loads all materials.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadMaterialsAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading materials...");
        var materials = await _materialQueryService.GetAllMaterialsAsync(cancellationToken);
        UpdateMaterials(materials);
        ClearBusy();
    }

    /// <summary>
    /// Debounced search to prevent excessive database queries.
    /// </summary>
    private async Task DebouncedSearchAsync()
    {
        // Cancel and dispose previous search if still running
        var cts = _searchCancellationTokenSource;
        if (cts != null)
        {
            cts.Cancel();
            cts.Dispose();
        }

        _searchCancellationTokenSource = new CancellationTokenSource();
        var token = _searchCancellationTokenSource.Token;

        try
        {
            // Wait 300ms to allow user to finish typing
            await Task.Delay(300, token);

            // Re-check cancellation before network/db call
            token.ThrowIfCancellationRequested();

            // If not cancelled, perform the search
            await SearchMaterialsAsync(token);
        }
        catch (OperationCanceledException)
        {
            // Search was cancelled by new input, ignore
        }
    }

    /// <summary>
    /// Searches materials based on the current search text.
    /// </summary>
    private async Task SearchMaterialsAsync(CancellationToken cancellationToken)
    {
        SetBusy("Searching materials...");
        var materials = await _materialQueryService.SearchMaterialsAsync(SearchText, cancellationToken);
        UpdateMaterials(materials);
        ClearBusy();
    }

    /// <summary>
    /// Updates the materials collection on the UI thread.
    /// </summary>
    /// <param name="materials">The materials to update.</param>
    private void UpdateMaterials(IReadOnlyList<MaterialListItem> materials)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            // Already on UI thread or no dispatcher (fallback)
            UpdateMaterialsInternal(materials);
        }
        else
        {
            // Marshal to UI thread
            dispatcher.Invoke(() => UpdateMaterialsInternal(materials));
        }
    }

    /// <summary>
    /// Updates the materials collection internally (must be called on UI thread).
    /// </summary>
    /// <param name="materials">The materials to update.</param>
    private void UpdateMaterialsInternal(IReadOnlyList<MaterialListItem> materials)
    {
        Materials.Clear();
        foreach (var material in materials)
        {
            Materials.Add(material);
        }
    }
}
