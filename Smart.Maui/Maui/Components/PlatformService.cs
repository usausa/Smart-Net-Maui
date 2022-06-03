namespace Smart.Maui.Components;

public sealed class PlatformService : IPlatformService
{
    public void BeginInvokeOnMainThread(Action action)
    {
        Device.BeginInvokeOnMainThread(action);
    }

    public void StartTimer(TimeSpan interval, Func<bool> callBack)
    {
        Device.StartTimer(interval, callBack);
    }
}
