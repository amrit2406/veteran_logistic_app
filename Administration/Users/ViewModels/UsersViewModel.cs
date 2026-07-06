using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading;
using veteran_logistic.Administration.Users.Contracts;
using veteran_logistic.Administration.Users.Models;
using veteran_logistic.Administration.Users.ViewModels;
using veteran_logistic.MVVM;
using veteran_logistic.Navigation;

namespace veteran_logistic.Administration.Users.ViewModels;

/// <summary>
/// ViewModel for the Users listing screen.
/// </summary>
public sealed partial class UsersViewModel : ViewModelBase
{
    private readonly IUserQueryService _userQueryService;
    private readonly INavigationService _navigationService;
    private string _searchText = string.Empty;
    private UserListItem? _selectedUser;
    private CancellationTokenSource? _searchCancellationTokenSource;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersViewModel"/> class.
    /// </summary>
    /// <param name="userQueryService">The user query service.</param>
    /// <param name="navigationService">The navigation service.</param>
    public UsersViewModel(IUserQueryService userQueryService, INavigationService navigationService)
    {
        _userQueryService = userQueryService ?? throw new ArgumentNullException(nameof(userQueryService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
    }

    public override async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        if (IsInitialized)
        {
            return;
        }

        await LoadUsersAsync(cancellationToken);
        await base.InitializeAsync(cancellationToken);
    }

    /// <summary>
    /// Gets the collection of users to display.
    /// </summary>
    public ObservableCollection<UserListItem> Users { get; } = new();

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
    /// Gets or sets the selected user.
    /// </summary>
    public UserListItem? SelectedUser
    {
        get => _selectedUser;
        set => SetProperty(ref _selectedUser, value);
    }

    /// <summary>
    /// Command to refresh the user list.
    /// </summary>
    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadUsersAsync();
    }

    /// <summary>
    /// Command to navigate to the Add User screen.
    /// </summary>
    [RelayCommand]
    private async Task AddUserAsync()
    {
        await _navigationService.NavigateAsync<AddUserViewModel>().ConfigureAwait(false);
    }

    /// <summary>
    /// Loads all users.
    /// </summary>
    private async Task LoadUsersAsync(CancellationToken cancellationToken = default)
    {
        SetBusy("Loading users...");
        var users = await _userQueryService.GetAllUsersAsync(cancellationToken);
        UpdateUsers(users);
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
            await SearchUsersAsync(token);
        }
        catch (OperationCanceledException)
        {
            // Search was cancelled by new input, ignore
        }
    }

    /// <summary>
    /// Searches users based on the current search text.
    /// </summary>
    private async Task SearchUsersAsync(CancellationToken cancellationToken)
    {
        SetBusy("Searching users...");
        var users = await _userQueryService.SearchUsersAsync(SearchText, cancellationToken);
        UpdateUsers(users);
        ClearBusy();
    }

    private void UpdateUsers(IEnumerable<UserListItem> users)
    {
        Users.Clear();
        foreach (var user in users)
        {
            Users.Add(user);
        }
    }
}
