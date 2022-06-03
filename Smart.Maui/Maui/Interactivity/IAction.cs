namespace Smart.Maui.Interactivity;

public interface IAction
{
    void DoInvoke(BindableObject associatedObject, object? parameter);
}
