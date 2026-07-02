using System.Threading.Tasks;
using veteran_logistic.Services.Dialog;

namespace veteran_logistic.Services.Notification;

/// <summary>
/// Default notification service. For now it delegates to IDialogService to keep behavior simple.
/// Future implementations can show toasts or non-modal UI.
/// </summary>
public sealed class NotificationService : INotificationService
{
    private readonly IDialogService _dialogService;

    public NotificationService(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }

    public Task ShowSuccessAsync(string title, string message)
    {
        var opts = new DialogOptions { Title = title, Message = message, Type = DialogType.Success };
        return _dialogService.ShowInformationAsync(opts);
    }

    public Task ShowInformationAsync(string title, string message)
    {
        var opts = new DialogOptions { Title = title, Message = message, Type = DialogType.Information };
        return _dialogService.ShowInformationAsync(opts);
    }

    public Task ShowWarningAsync(string title, string message)
    {
        var opts = new DialogOptions { Title = title, Message = message, Type = DialogType.Warning };
        return _dialogService.ShowWarningAsync(opts);
    }

    public Task ShowErrorAsync(string title, string message)
    {
        var opts = new DialogOptions { Title = title, Message = message, Type = DialogType.Error };
        return _dialogService.ShowErrorAsync(opts);
    }
}
