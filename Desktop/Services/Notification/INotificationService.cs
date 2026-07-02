using System.Threading.Tasks;

namespace veteran_logistic.Services.Notification;

/// <summary>
/// Shows non-blocking notifications to the user. Current implementation delegates to DialogService.
/// </summary>
public interface INotificationService
{
    Task ShowSuccessAsync(string title, string message);
    Task ShowInformationAsync(string title, string message);
    Task ShowWarningAsync(string title, string message);
    Task ShowErrorAsync(string title, string message);
}
