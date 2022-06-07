namespace Smart.Maui.Interactivity;

using Smart.Maui.Messaging;

public sealed class ResolveRequestTrigger : RequestTriggerBase<ResultEventArgs>
{
    protected override void OnEventRequest(object? sender, ResultEventArgs e)
    {
        InvokeActions(e);
    }
}
