namespace Smart.Maui.Components;

public sealed class DialogService : IDialogService
{
    public async Task<bool> DisplayAlert(string? title, string? message, string? acceptButton, string? cancelButton)
    {
        return await (Application.Current?.MainPage?.DisplayAlert(title, message, acceptButton, cancelButton) ?? Task.FromResult(false));
    }

    public async Task DisplayAlert(string? title, string? message, string? cancelButton)
    {
        await (Application.Current?.MainPage?.DisplayAlert(title, message, cancelButton) ?? Task.CompletedTask);
    }

    public async Task<string?> DisplayActionSheet(string? title, string? cancelButton, string? destroyButton, params string[] otherButtons)
    {
        return await (Application.Current?.MainPage?.DisplayActionSheet(title, cancelButton, destroyButton, otherButtons) ?? Task.FromResult(default(string)));
    }

    public async Task DisplayActionSheet(string? title, params IActionSheetButton[] buttons)
    {
        var cancelButton = buttons.FirstOrDefault(b => b.ButtonType == ActionSheetButtonType.Cancel);
        var destroyButton = buttons.FirstOrDefault(b => b.ButtonType == ActionSheetButtonType.Destroy);
        var otherButtonTexts = buttons.Where(b => b.ButtonType == ActionSheetButtonType.Other).Select(b => b.Text).ToArray();

        var selectedText = await DisplayActionSheet(title, cancelButton?.Text, destroyButton?.Text, otherButtonTexts);

        var selectedButton = buttons.FirstOrDefault(b => b.Text == selectedText);
        if (selectedButton is not null)
        {
            await selectedButton.Execute();
        }
    }

    public async Task<string?> DisplayPrompt(string? title, string? message, string accept = "OK", string cancel = "Cancel", string? placeholder = default, int maxLength = -1, Keyboard? keyboard = null, string initialValue = "")
    {
        return await (Application.Current?.MainPage?.DisplayPromptAsync(title, message, accept, cancel, placeholder, maxLength, keyboard, initialValue) ?? Task.FromResult(default(string)));
    }
}
