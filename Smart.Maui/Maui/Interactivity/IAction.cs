namespace Smart.Maui.Interactivity;

public interface IAction
{
    void Execute(BindableObject associatedObject, object? parameter);
}
