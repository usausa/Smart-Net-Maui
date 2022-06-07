namespace Smart.Maui.Components;

public interface IDialogService
{
    Task<bool> DisplayAlert(string? title, string? message, string? acceptButton, string? cancelButton);

    Task DisplayAlert(string? title, string? message, string? cancelButton);

    Task<string?> DisplayActionSheet(string? title, string? cancelButton, string? destroyButton, params string[] otherButtons);

    Task DisplayActionSheet(string? title, params IActionSheetButton[] buttons);

    Task<string?> DisplayPrompt(string? title, string? message, string accept = "OK", string cancel = "Cancel", string? placeholder = default, int maxLength = -1, Keyboard? keyboard = null, string initialValue = "");
}
