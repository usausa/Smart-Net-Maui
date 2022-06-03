namespace Smart.Maui.Messaging;

public sealed class ParameterEventArgs : EventArgs
{
    public object? Parameter { get; }

    public ParameterEventArgs(object? parameter)
    {
        Parameter = parameter;
    }
}
