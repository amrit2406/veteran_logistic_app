namespace veteran_logistic.Services.Dialog;

/// <summary>
/// Metadata describing a dialog to show.
/// </summary>
public sealed class DialogOptions
{
    /// <summary>
    /// Dialog title displayed in the window chrome.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Message body displayed in the dialog.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Logical dialog type.
    /// </summary>
    public DialogType Type { get; set; } = DialogType.Information;

    /// <summary>
    /// Default result to use if applicable.
    /// </summary>
    public DialogResult DefaultResult { get; set; } = DialogResult.None;

    /// <summary>
    /// Whether the dialog may be cancelled by the user.
    /// </summary>
    public bool AllowCancel { get; set; } = true;
}
