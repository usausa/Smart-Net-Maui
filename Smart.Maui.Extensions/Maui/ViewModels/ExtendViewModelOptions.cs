namespace Smart.Maui.ViewModels;

using Smart.Mvvm.ViewModels;

public class ExtendViewModelOptions : ViewModelOptions, IExtendViewModelOptions
{
    public CommandBehavior CommandBehavior { get; init; } = CommandBehavior.Standard;

    public bool AutoUpdateCommandState { get; init; } = true;
}
