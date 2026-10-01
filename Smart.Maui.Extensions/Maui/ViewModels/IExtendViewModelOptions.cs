namespace Smart.Maui.ViewModels;

using Smart.Mvvm.ViewModels;

public interface IExtendViewModelOptions : IViewModelOptions
{
    CommandMode CommandMode { get; }

    bool AutoUpdateCommandState => true;
}
