namespace Smart.Maui.Messaging;

using System.ComponentModel;
using System.Diagnostics;

public sealed class CollectionScrollEventArgs : EventArgs
{
    public int Index { get; set; }

    public int GroupIndex { get; set; } = -1;

    public ScrollToPosition Position { get; set; } = ScrollToPosition.MakeVisible;

    public bool Animate { get; set; } = true;
}

[DebuggerDisplay("Reference = ({" + nameof(ScrollReferenceCount) + "})")]
public sealed class CollectionController
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public event EventHandler<CollectionScrollEventArgs>? ScrollRequested;

    private int ScrollReferenceCount => ScrollRequested?.GetInvocationList().Length ?? 0;

    public void ScrollRequest(int index, int groupIndex = -1, ScrollToPosition position = ScrollToPosition.MakeVisible, bool animate = true)
    {
        ScrollRequested?.Invoke(this, new CollectionScrollEventArgs
        {
            Index = index,
            GroupIndex = groupIndex,
            Position = position,
            Animate = animate
        });
    }
}
