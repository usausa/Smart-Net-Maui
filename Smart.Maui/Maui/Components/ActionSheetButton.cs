namespace Smart.Maui.Components;

public enum ActionSheetButtonType
{
    Other,
    Cancel,
    Destroy
}

public interface IActionSheetButton
{
    ActionSheetButtonType ButtonType { get; }

    string Text { get; }

    Task Execute();
}

internal sealed class NoParameterActionSheetButton : IActionSheetButton
{
    public ActionSheetButtonType ButtonType { get; }

    public string Text { get; }

    private readonly Action? action;

    private readonly Func<Task>? asyncAction;

    internal NoParameterActionSheetButton(ActionSheetButtonType buttonType, string text, Action action)
    {
        ButtonType = buttonType;
        Text = text;
        this.action = action;
        asyncAction = null;
    }

    internal NoParameterActionSheetButton(ActionSheetButtonType buttonType, string text, Func<Task>? asyncAction)
    {
        ButtonType = buttonType;
        Text = text;
        action = null;
        this.asyncAction = asyncAction;
    }

    public async Task Execute()
    {
        if (action is not null)
        {
            action();
        }
        else if (asyncAction is not null)
        {
            await asyncAction();
        }
    }
}

internal sealed class ParameterActionSheetButton<T> : IActionSheetButton
{
    public ActionSheetButtonType ButtonType { get; }

    public string Text { get; }

    private readonly Action<T>? action;

    private readonly Func<T, Task>? asyncAction;

    private readonly T parameter;

    internal ParameterActionSheetButton(ActionSheetButtonType buttonType, string text, T parameter, Action<T> action)
    {
        ButtonType = buttonType;
        Text = text;
        this.parameter = parameter;
        this.action = action;
        asyncAction = null;
    }

    internal ParameterActionSheetButton(ActionSheetButtonType buttonType, string text, T parameter, Func<T, Task>? asyncAction)
    {
        ButtonType = buttonType;
        Text = text;
        this.parameter = parameter;
        action = null;
        this.asyncAction = asyncAction;
    }

    public async Task Execute()
    {
        if (action is not null)
        {
            action(parameter);
        }
        else if (asyncAction is not null)
        {
            await asyncAction(parameter);
        }
    }
}

public sealed class ActionSheetButton
{
    public static IActionSheetButton Create(string text, Action action) =>
        new NoParameterActionSheetButton(ActionSheetButtonType.Other, text, action);

    public static IActionSheetButton Create(string text, Func<Task> asyncAction) =>
        new NoParameterActionSheetButton(ActionSheetButtonType.Other, text, asyncAction);

    public static IActionSheetButton Create<T>(string text, T parameter, Action<T> action) =>
        new ParameterActionSheetButton<T>(ActionSheetButtonType.Other, text, parameter, action);

    public static IActionSheetButton Create<T>(string text, T parameter, Func<T, Task> asyncAction) =>
        new ParameterActionSheetButton<T>(ActionSheetButtonType.Other, text, parameter, asyncAction);

    public static IActionSheetButton CreateCancel(string text, Action action) =>
        new NoParameterActionSheetButton(ActionSheetButtonType.Cancel, text, action);

    public static IActionSheetButton CreateCancel(string text, Func<Task> asyncAction) =>
        new NoParameterActionSheetButton(ActionSheetButtonType.Cancel, text, asyncAction);

    public static IActionSheetButton CreateCancel<T>(string text, T parameter, Action<T> action) =>
        new ParameterActionSheetButton<T>(ActionSheetButtonType.Cancel, text, parameter, action);

    public static IActionSheetButton CreateCancel<T>(string text, T parameter, Func<T, Task> asyncAction) =>
        new ParameterActionSheetButton<T>(ActionSheetButtonType.Cancel, text, parameter, asyncAction);

    public static IActionSheetButton CreateDestroy(string text, Action action) =>
        new NoParameterActionSheetButton(ActionSheetButtonType.Destroy, text, action);

    public static IActionSheetButton CreateDestroy(string text, Func<Task> asyncAction) =>
        new NoParameterActionSheetButton(ActionSheetButtonType.Destroy, text, asyncAction);

    public static IActionSheetButton CreateDestroy<T>(string text, T parameter, Action<T> action) =>
        new ParameterActionSheetButton<T>(ActionSheetButtonType.Destroy, text, parameter, action);

    public static IActionSheetButton CreateDestroy<T>(string text, T parameter, Func<T, Task> asyncAction) =>
        new ParameterActionSheetButton<T>(ActionSheetButtonType.Destroy, text, parameter, asyncAction);
}
