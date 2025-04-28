namespace Smart.Maui.Interactivity;

using Smart.Maui.Messaging;

public sealed class ResolveRequestTrigger : RequestTriggerBase<ResolveEventArgs>
{
    protected override void OnEventRequest(object? sender, ResolveEventArgs e)
    {
        InvokeActions(e);
    }
}
