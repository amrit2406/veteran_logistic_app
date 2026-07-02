using System;

namespace VeteranLogistics.Shared.Interfaces;

/// <summary>
/// Provides date/time abstractions for application services and tests.
/// Use this interface to avoid direct dependency on DateTime.Now/UtcNow.
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>
    /// Local current date and time.
    /// </summary>
    DateTime Now { get; }

    /// <summary>
    /// Current date and time in UTC.
    /// </summary>
    DateTime UtcNow { get; }

    /// <summary>
    /// Current date (date component only) in the provider's local time.
    /// </summary>
    DateOnly Today { get; }
}
