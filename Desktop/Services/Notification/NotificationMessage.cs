using System;
using veteran_logistic.Services.Notification;

namespace veteran_logistic.Services.Notification;

/// <summary>
/// Lightweight notification message model.
/// </summary>
public sealed class NotificationMessage
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; } = NotificationType.Information;
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}
