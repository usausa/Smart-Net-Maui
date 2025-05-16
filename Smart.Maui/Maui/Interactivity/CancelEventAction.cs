namespace Smart.Maui.Interactivity;

using System.ComponentModel;

public sealed class CancelEventAction : BindableObject, IAction
{
    public static readonly BindableProperty CancelProperty = BindableProperty.Create(
        nameof(Cancel),
        typeof(bool),
        typeof(CancelEventAction),
        false);

    public bool Cancel
    {
        get => (bool)GetValue(CancelProperty);
        set => SetValue(CancelProperty, value);
    }

    public void Execute(BindableObject associatedObject, object? parameter)
    {
        if (parameter is CancelEventArgs args)
        {
            args.Cancel = Cancel;
        }
    }
}
