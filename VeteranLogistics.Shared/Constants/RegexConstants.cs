namespace VeteranLogistics.Shared.Constants;

/// <summary>
/// Regular expression patterns used across the application.
/// </summary>
public static class RegexConstants
{
    // Simplified email regex for basic validation; keep strict patterns in validators if needed.
    public const string Email = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

    // Basic phone number pattern allowing digits, spaces, dashes and parentheses
    public const string Phone = @"^[0-9\-\s\(\)\+]+$";

    // Vehicle number placeholder pattern (provider-specific patterns should be implemented in validators)
    public const string VehicleNumber = @"^[A-Z0-9\-\s]+$";
}
