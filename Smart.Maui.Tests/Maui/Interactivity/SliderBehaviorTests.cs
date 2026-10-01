namespace Smart.Maui.Interactivity;

using System.ComponentModel;

using Microsoft.Maui.Controls;

using Smart.Maui.Input;

public sealed class SliderBehaviorTests
{
    private sealed class PositionSource : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public double Position
        {
            get;
            set
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Position)));
            }
        }
    }

    private static Slider CreateBoundSlider(PositionSource source, double maximum)
    {
        var slider = new Slider { Maximum = maximum };
        slider.SetBinding(Slider.ValueProperty, new Binding(nameof(PositionSource.Position), BindingMode.OneWay, source: source));
        return slider;
    }

    //------------------------------------------------------------------
    // Seek
    //------------------------------------------------------------------

    [Fact]
    public void SeekExecutesCommandWithValueOnDragCompleted()
    {
        // Arrange
        TestDispatcher.Install();
        var source = new PositionSource();
        var slider = CreateBoundSlider(source, 10);
        object? received = null;
        slider.Behaviors.Add(new SliderSeekBehavior { Command = new DelegateCommand<double>(x => received = x) });

        // Act
        slider.Value = 4;
        ((ISliderController)slider).SendDragCompleted();

        // Assert
        Assert.Equal(4d, received);
    }

    [Fact]
    public void SeekShowsBoundValueAfterDragCompleted()
    {
        // Arrange
        TestDispatcher.Install();
        var source = new PositionSource();
        var slider = CreateBoundSlider(source, 10);
        slider.Behaviors.Add(new SliderSeekBehavior());

        // Act
        slider.Value = 4;
        ((ISliderController)slider).SendDragCompleted();
        source.Position = 6;

        // Assert
        Assert.Equal(6d, slider.Value);
    }

    [Fact]
    public void SeekShowsBoundValueAfterMaximumChanged()
    {
        // Arrange
        var dispatcher = TestDispatcher.Install();
        var source = new PositionSource();
        var slider = CreateBoundSlider(source, 1);
        slider.Behaviors.Add(new SliderSeekBehavior());

        // Act
        slider.Maximum = 10;
        dispatcher.RunPending();
        source.Position = 6;

        // Assert
        Assert.Equal(6d, slider.Value);
    }

    //------------------------------------------------------------------
    // Step
    //------------------------------------------------------------------

    [Theory]
    [InlineData(3.4, 3)]
    [InlineData(3.6, 4)]
    public void StepRoundsValue(double value, double expected)
    {
        // Arrange
        var slider = new Slider { Maximum = 10 };
        slider.Behaviors.Add(new SliderStepBehavior { Step = 1 });

        // Act
        slider.Value = value;

        // Assert
        Assert.Equal(expected, slider.Value);
    }

    [Fact]
    public void StepRoundsFromMinimum()
    {
        // Arrange
        var slider = new Slider { Maximum = 10, Minimum = 1 };
        slider.Behaviors.Add(new SliderStepBehavior { Step = 2 });

        // Act
        slider.Value = 4.2;

        // Assert
        Assert.Equal(5d, slider.Value);
    }

    [Fact]
    public void StepDoesNotRoundWhenNotPositive()
    {
        // Arrange
        var slider = new Slider { Maximum = 10 };
        slider.Behaviors.Add(new SliderStepBehavior { Step = 0 });

        // Act
        slider.Value = 3.4;

        // Assert
        Assert.Equal(3.4, slider.Value);
    }
}
