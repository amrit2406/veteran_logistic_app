namespace VeteranLogistics.Shared.Interfaces;

/// <summary>
/// Represents a user's session information.
/// Implementations are application-specific and will be provided in future phases.
/// </summary>
public interface IUserSession
{
    string? UserId { get; }
    string? Username { get; }
    string? Role { get; }
    string? FinancialYear { get; }
    bool IsAuthenticated { get; }
}
