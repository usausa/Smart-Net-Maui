namespace Smart.Maui.Messaging;

public interface IEventRequest<TEventArgs>
    where TEventArgs : EventArgs
{
    event EventHandler<TEventArgs> Requested;
}
