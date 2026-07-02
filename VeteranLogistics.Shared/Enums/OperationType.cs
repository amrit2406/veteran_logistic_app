namespace VeteranLogistics.Shared.Enums;

/// <summary>
/// High-level operation types used in audit and UI flows.
/// </summary>
public enum OperationType : int
{
    Create = 1,
    Update = 2,
    Delete = 3,
    View = 4,
    Export = 5,
    Import = 6,
    Login = 7,
    Logout = 8
}
