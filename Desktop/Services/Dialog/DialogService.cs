using System.Threading.Tasks;
using System.Windows;

namespace veteran_logistic.Services.Dialog;

/// <summary>
/// Default dialog service implementation. ViewModels should only depend on <see cref="IDialogService"/>.
/// This implementation uses MessageBox for now; it centralizes UI calls so it can be replaced later.
/// </summary>
public sealed class DialogService : IDialogService
{
    public Task<DialogResult> ShowInformationAsync(DialogOptions options)
    {
        return ShowMessageBox(options, MessageBoxButton.OK, MessageBoxImage.Information);
    }

    public Task<DialogResult> ShowWarningAsync(DialogOptions options)
    {
        return ShowMessageBox(options, MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    public Task<DialogResult> ShowErrorAsync(DialogOptions options)
    {
        return ShowMessageBox(options, MessageBoxButton.OK, MessageBoxImage.Error);
    }

    public Task<DialogResult> ShowConfirmationAsync(DialogOptions options)
    {
        var buttons = options.AllowCancel ? MessageBoxButton.OKCancel : MessageBoxButton.OK;
        return ShowMessageBox(options, buttons, MessageBoxImage.Question);
    }

    public Task<DialogResult> ShowQuestionAsync(DialogOptions options)
    {
        var buttons = options.AllowCancel ? MessageBoxButton.YesNoCancel : MessageBoxButton.YesNo;
        return ShowMessageBox(options, buttons, MessageBoxImage.Question);
    }

    private Task<DialogResult> ShowMessageBox(DialogOptions options, MessageBoxButton buttons, MessageBoxImage icon)
    {
        // MessageBox must be called on the UI thread. Call synchronously and wrap result in Task for async signature.
        var result = MessageBox.Show(options.Message, string.IsNullOrWhiteSpace(options.Title) ? "" : options.Title, buttons, icon);

        return Task.FromResult(MapResult(result));
    }

    private static DialogResult MapResult(MessageBoxResult result)
    {
        return result switch
        {
            MessageBoxResult.OK => DialogResult.OK,
            MessageBoxResult.Cancel => DialogResult.Cancel,
            MessageBoxResult.Yes => DialogResult.Yes,
            MessageBoxResult.No => DialogResult.No,
            _ => DialogResult.None,
        };
    }
}
