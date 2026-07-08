using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Threading;
using VeteranLogistics.Shared.Constants;
using veteran_logistic.Administration.Roles.Contracts;
using veteran_logistic.Administration.Roles.Models;
using veteran_logistic.MVVM;

namespace veteran_logistic.Administration.Roles.ViewModels;

/// <summary>
/// ViewModel for the Roles listing screen.
/// </summary>
public sealed partial class RolesViewModel : ViewModelBase
{
    private readonly IRoleQueryService _roleQueryService;
    private readonly ILogger<RolesViewModel> _logger;
    private string _searchText = string.Empty;
    private RoleListItem? _selectedRole;
    private CancellationTokenSource? _searchCancellationTokenSource;

    /// <summary>
    /// Initializes a new instance of the <see cref="RolesViewModel"/> class.
    /// </summary>
    /// <param name="roleQueryService">The role query service.</param>
    /// <param name="logger">The logger.</param>
    public RolesViewModel(IRoleQueryService roleQueryService, ILogger<RolesViewModel> logger)
    {
        _roleQueryService = roleQueryService ?? throw new ArgumentNullException(nameof(roleQueryService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (IsInitialized)
        {
            return;
        }

        await LoadRolesAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    public override async Task OnNavigatedToAsync(CancellationToken cancellationToken = default)
    {
        await LoadRolesAsync(cancellationToken);
    }

    /// <summary>
    /// Gets the collection of roles to display.
    /// </summary>
    public ObservableCollection<RoleListItem> Roles { get; } = new();

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
    /// Gets or sets the selected role.
    /// </summary>
    public RoleListItem? SelectedRole
    {
        get => _selectedRole;
        set => SetProperty(ref _selectedRole, value);
    }

    /// <summary>
    /// Command to refresh the role list.
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadRolesAsync();
    }

    /// <summary>
    /// Loads all roles.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task LoadRolesAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading roles...");
        var roles = await _roleQueryService.GetAllRolesAsync(cancellationToken);
        UpdateRoles(roles);
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
            // Wait debounce delay to allow user to finish typing
            await Task.Delay(ApplicationConstants.SearchDebounceDelayMilliseconds, token);

            // Re-check cancellation before network/db call
            token.ThrowIfCancellationRequested();

            // If not cancelled, perform the search
            await SearchRolesAsync(token);
        }
        catch (OperationCanceledException)
        {
            // Search was cancelled by new input, ignore
        }
        catch (Exception ex)
        {
            // Log unexpected exceptions to prevent UI crashes
            _logger.LogError(ex, "An unexpected error occurred during role search debouncing");
        }
    }

    /// <summary>
    /// Searches roles based on the current search text.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task SearchRolesAsync(CancellationToken cancellationToken)
    {
        try
        {
            SetBusy("Searching roles...");
            var roles = await _roleQueryService.SearchRolesAsync(SearchText, cancellationToken);
            UpdateRoles(roles);
            ClearBusy();
        }
        catch (OperationCanceledException)
        {
            // Search was cancelled, ignore and clear busy state
            ClearBusy();
        }
        catch (Exception ex)
        {
            // Log unexpected exceptions and clear busy state to prevent UI crashes
            _logger.LogError(ex, "An unexpected error occurred during role search");
            ClearBusy();
        }
    }

    /// <summary>
    /// Updates the roles collection on the UI thread.
    /// </summary>
    /// <param name="roles">The roles to update.</param>
    private void UpdateRoles(IEnumerable<RoleListItem> roles)
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher == null || dispatcher.CheckAccess())
        {
            // Already on UI thread or no dispatcher (fallback)
            UpdateRolesInternal(roles);
        }
        else
        {
            // Marshal to UI thread
            dispatcher.Invoke(() => UpdateRolesInternal(roles));
        }
    }

    /// <summary>
    /// Updates the roles collection internally (must be called on UI thread).
    /// </summary>
    /// <param name="roles">The roles to update.</param>
    private void UpdateRolesInternal(IEnumerable<RoleListItem> roles)
    {
        Roles.Clear();
        foreach (var role in roles)
        {
            Roles.Add(role);
        }
    }
}
