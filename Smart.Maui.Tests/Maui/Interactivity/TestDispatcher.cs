namespace Smart.Maui.Interactivity;

using Microsoft.Maui.Dispatching;

internal sealed class TestDispatcher : IDispatcher
{
    private static readonly ThreadLocal<TestDispatcher?> Current = new();

    private static readonly TestDispatcherProvider Provider = new();

    private readonly List<Action> pending = [];

    public List<TestDispatcherTimer> Timers { get; } = [];

    public bool IsDispatchRequired => false;

    // Install a new dispatcher for the current thread before creating elements
    public static TestDispatcher Install()
    {
        var dispatcher = new TestDispatcher();
        Current.Value = dispatcher;
        DispatcherProvider.SetCurrent(Provider);
        return dispatcher;
    }

    public bool Dispatch(Action action)
    {
        pending.Add(action);
        return true;
    }

    public bool DispatchDelayed(TimeSpan delay, Action action)
    {
        pending.Add(action);
        return true;
    }

    public IDispatcherTimer CreateTimer()
    {
        var timer = new TestDispatcherTimer();
        Timers.Add(timer);
        return timer;
    }

    public void RunPending()
    {
        var actions = pending.ToArray();
        pending.Clear();
        foreach (var action in actions)
        {
            action();
        }
    }

    private sealed class TestDispatcherProvider : IDispatcherProvider
    {
        public IDispatcher? GetForCurrentThread() => Current.Value;
    }
}

internal sealed class TestDispatcherTimer : IDispatcherTimer
{
    public event EventHandler? Tick;

    public TimeSpan Interval { get; set; }

    public bool IsRepeating { get; set; } = true;

    public bool IsRunning { get; private set; }

    public void Start() => IsRunning = true;

    public void Stop() => IsRunning = false;

    public void Fire()
    {
        if (IsRunning)
        {
            IsRunning = IsRepeating;
            Tick?.Invoke(this, EventArgs.Empty);
        }
    }
}
