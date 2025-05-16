namespace Smart.Maui.Interactivity;

using Smart.Mvvm.Messaging;

public sealed class ResolveRequestBehavior : RequestBehaviorBase<ResolveEventArgs>
{
    protected override void OnEventRequest(object? sender, ResolveEventArgs e)
    {
        InvokeActions(e);
    }
}
