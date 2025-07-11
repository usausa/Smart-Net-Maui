namespace Smart.Maui.ViewModels;

using Smart.Mvvm.ViewModels;

public interface IExtendViewModelOptions : IViewModelOptions
{
    CommandBehavior CommandBehavior { get; }
}
