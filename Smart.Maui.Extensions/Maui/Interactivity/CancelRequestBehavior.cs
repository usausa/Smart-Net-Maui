namespace Smart.Maui.Interactivity;

using System.ComponentModel;

public sealed class CancelRequestBehavior : RequestBehaviorBase<CancelEventArgs>
{
    protected override void OnEventRequest(object? sender, CancelEventArgs e)
    {
        InvokeActions(e);
    }
}
