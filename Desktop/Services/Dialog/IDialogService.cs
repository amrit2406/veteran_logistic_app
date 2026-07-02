using System.Threading.Tasks;

namespace veteran_logistic.Services.Dialog;

/// <summary>
/// Abstraction for showing dialogs to the user. Implementations must ensure UI interaction is isolated from ViewModels.
/// </summary>
public interface IDialogService
{
    Task<DialogResult> ShowInformationAsync(DialogOptions options);
    Task<DialogResult> ShowWarningAsync(DialogOptions options);
    Task<DialogResult> ShowErrorAsync(DialogOptions options);
    Task<DialogResult> ShowConfirmationAsync(DialogOptions options);
    Task<DialogResult> ShowQuestionAsync(DialogOptions options);
}
