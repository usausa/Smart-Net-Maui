namespace Smart.Maui.Interactivity;

using System.ComponentModel;
using System.Windows.Input;

public sealed class SliderSeekBehavior : BehaviorBase<Slider>
{
    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command),
        typeof(ICommand),
        typeof(SliderSeekBehavior));

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    protected override void OnAttachedTo(Slider bindable)
    {
        base.OnAttachedTo(bindable);

        bindable.DragCompleted += OnDragCompleted;
        bindable.PropertyChanged += OnPropertyChanged;
    }

    protected override void OnDetachingFrom(Slider bindable)
    {
        bindable.DragCompleted -= OnDragCompleted;
        bindable.PropertyChanged -= OnPropertyChanged;

        base.OnDetachingFrom(bindable);
    }

    private void OnDragCompleted(object? sender, EventArgs e)
    {
        if (sender is not Slider slider)
        {
            return;
        }

        var value = slider.Value;
        var command = Command;
        if (command?.CanExecute(value) ?? false)
        {
            command.Execute(value);
        }

        slider.ClearValue(Slider.ValueProperty);
    }

    private static void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if ((sender is Slider slider) &&
            ((e.PropertyName == Slider.MinimumProperty.PropertyName) || (e.PropertyName == Slider.MaximumProperty.PropertyName)))
        {
            slider.Dispatcher.Dispatch(() => slider.ClearValue(Slider.ValueProperty));
        }
    }
}
