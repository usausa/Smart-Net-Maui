namespace Smart.Maui.Interactivity;

using Smart.Mvvm.Messaging;

public sealed class EventRequestBehavior : RequestBehaviorBase<ParameterEventArgs>
{
    protected override void OnEventRequest(object? sender, ParameterEventArgs e)
    {
        InvokeActions(e.Parameter);
    }
}
