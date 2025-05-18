namespace Smart.Maui.Messaging;

using System.ComponentModel;
using System.Diagnostics;

[DebuggerDisplay("Reference = {" + nameof(ReferenceCount) + "}")]
public sealed class ValidationFocusRequest
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public event EventHandler<EventArgs>? FocusRequested;

    private int ReferenceCount => FocusRequested?.GetInvocationList().Length ?? 0;

    public void FocusRequest()
    {
        FocusRequested?.Invoke(this, EventArgs.Empty);
    }
}
