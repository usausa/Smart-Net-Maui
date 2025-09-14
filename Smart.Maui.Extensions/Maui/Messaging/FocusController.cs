namespace Smart.Maui.Messaging;

using System;
using System.ComponentModel;
using System.Diagnostics;

public sealed class FocusFindEventArgs : EventArgs
{
    public string? Name { get; set; }
}

[DebuggerDisplay("Reference = ({" + nameof(FocusReferenceCount) + "}, {" + nameof(FindReferenceCount) + "})")]
public sealed class FocusController
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public event EventHandler<EventArgs<string>>? FocusRequested;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public event EventHandler<FocusFindEventArgs>? FindRequested;

    private int FocusReferenceCount => FocusRequested?.GetInvocationList().Length ?? 0;

    private int FindReferenceCount => FindRequested?.GetInvocationList().Length ?? 0;

    public void FocusRequest(string name)
    {
        FocusRequested?.Invoke(this, new EventArgs<string>(name));
    }

    public string? FindRequest()
    {
        var args = new FocusFindEventArgs();
        FindRequested?.Invoke(this, args);
        return args.Name;
    }
}
