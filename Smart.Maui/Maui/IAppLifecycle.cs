namespace Smart.Maui;

public interface IAppLifecycle
{
    void OnCreated();

    void OnActivated();

    void OnDeactivated();

    void OnStopped();

    void OnResumed();

    void OnDestroying();
}
