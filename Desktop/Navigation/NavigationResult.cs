namespace veteran_logistic.Navigation;

/// <summary>
/// Result of a navigation attempt.
/// </summary>
public sealed class NavigationResult
{
    public bool Succeeded { get; }

    public string? ErrorMessage { get; }

    private NavigationResult(bool succeeded, string? errorMessage = null)
    {
        Succeeded = succeeded;
        ErrorMessage = errorMessage;
    }

    public static NavigationResult Success() => new NavigationResult(true);

    public static NavigationResult Failure(string message) => new NavigationResult(false, message);
}
