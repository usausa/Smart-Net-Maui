namespace Smart.Maui.Interactivity;

using System.Globalization;

using Microsoft.Maui.Controls;

public sealed class ChangePropertyActionTests
{
    private sealed class TestBindable : BindableObject;

    private sealed class NullConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => null;

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
    }

    private sealed class PropertyTarget
    {
        public object? Value { get; set; } = "initial";

        public string Failure { get; } = "failed";

        public long Number { get; set; }

        public DayOfWeek Day { get; set; }

        public string ReadOnly { get; } = "fixed";

        public string? Hidden { get; private set; }

        public string Guarded
        {
            get => Failure;
            set => throw new InvalidOperationException($"{Failure} {value}");
        }
    }

    private sealed class OtherTarget
    {
        public object? Value { get; set; }
    }

    [Fact]
    public void SetsNullWhenConverterReturnsNull()
    {
        // Arrange
        var target = new PropertyTarget();
        var action = new ChangePropertyAction
        {
            TargetObject = target,
            PropertyName = nameof(PropertyTarget.Value),
            Converter = new NullConverter()
        };

        // Act
        action.Execute(new TestBindable(), "eventArgument");

        // Assert
        Assert.Null(target.Value);
    }

    [Fact]
    public void SetsParameterWhenSet()
    {
        // Arrange
        var target = new PropertyTarget();
        var action = new ChangePropertyAction
        {
            TargetObject = target,
            PropertyName = nameof(PropertyTarget.Value),
            Parameter = "parameter"
        };

        // Act
        action.Execute(new TestBindable(), "eventArgument");

        // Assert
        Assert.Equal("parameter", target.Value);
    }

    [Fact]
    public void SetsEventParameterWhenParameterIsNotSet()
    {
        // Arrange
        var target = new PropertyTarget();
        var action = new ChangePropertyAction
        {
            TargetObject = target,
            PropertyName = nameof(PropertyTarget.Value)
        };

        // Act
        action.Execute(new TestBindable(), "eventArgument");

        // Assert
        Assert.Equal("eventArgument", target.Value);
    }

    [Fact]
    public void SetsNullWhenParameterIsSetToNull()
    {
        // Arrange
        var target = new PropertyTarget();
        var action = new ChangePropertyAction
        {
            TargetObject = target,
            PropertyName = nameof(PropertyTarget.Value),
            Parameter = null
        };

        // Act
        action.Execute(new TestBindable(), "eventArgument");

        // Assert
        Assert.Null(target.Value);
    }

    [Fact]
    public void ConvertsWideningValueToPropertyType()
    {
        // Arrange
        var target = new PropertyTarget();
        var action = new ChangePropertyAction
        {
            TargetObject = target,
            PropertyName = nameof(PropertyTarget.Number)
        };

        // Act
        action.Execute(new TestBindable(), 42);

        // Assert
        Assert.Equal(42L, target.Number);
    }

    [Fact]
    public void ConvertsEnumValueFromUnderlyingValue()
    {
        // Arrange
        var target = new PropertyTarget();
        var action = new ChangePropertyAction
        {
            TargetObject = target,
            PropertyName = nameof(PropertyTarget.Day)
        };

        // Act
        action.Execute(new TestBindable(), 3);

        // Assert
        Assert.Equal(DayOfWeek.Wednesday, target.Day);
    }

    [Fact]
    public void SetsPropertyWithPrivateSetter()
    {
        // Arrange
        var target = new PropertyTarget();
        var action = new ChangePropertyAction
        {
            TargetObject = target,
            PropertyName = nameof(PropertyTarget.Hidden),
            Parameter = "secret"
        };

        // Act
        action.Execute(new TestBindable(), null);

        // Assert
        Assert.Equal("secret", target.Hidden);
    }

    [Fact]
    public void ThrowsArgumentExceptionWhenPropertyIsReadOnly()
    {
        // Arrange
        var target = new PropertyTarget();
        var action = new ChangePropertyAction
        {
            TargetObject = target,
            PropertyName = nameof(PropertyTarget.ReadOnly),
            Parameter = "changed"
        };

        // Act
        var exception = Record.Exception(() => action.Execute(new TestBindable(), null));

        // Assert
        Assert.IsType<ArgumentException>(exception);
        Assert.Equal("fixed", target.ReadOnly);
    }

    [Fact]
    public void IgnoresNullPropertyName()
    {
        // Arrange
        var target = new PropertyTarget();
        var action = new ChangePropertyAction
        {
            TargetObject = target,
            PropertyName = null!,
            Parameter = "changed"
        };

        // Act
        var exception = Record.Exception(() => action.Execute(new TestBindable(), null));

        // Assert
        Assert.Null(exception);
        Assert.Equal("initial", target.Value);
    }

    [Fact]
    public void ResolvesAgainWhenPropertyNameChanged()
    {
        // Arrange
        var target = new PropertyTarget();
        var action = new ChangePropertyAction
        {
            TargetObject = target,
            PropertyName = nameof(PropertyTarget.Value)
        };
        action.Execute(new TestBindable(), "first");

        // Act
        action.PropertyName = nameof(PropertyTarget.Hidden);
        action.Execute(new TestBindable(), "second");

        // Assert
        Assert.Equal("first", target.Value);
        Assert.Equal("second", target.Hidden);
    }

    [Fact]
    public void ResolvesAgainWhenTargetTypeChanged()
    {
        // Arrange
        var first = new PropertyTarget();
        var second = new OtherTarget();
        var action = new ChangePropertyAction
        {
            TargetObject = first,
            PropertyName = nameof(PropertyTarget.Value)
        };
        action.Execute(new TestBindable(), "first");

        // Act
        action.TargetObject = second;
        action.Execute(new TestBindable(), "second");

        // Assert
        Assert.Equal("first", first.Value);
        Assert.Equal("second", second.Value);
    }

    [Fact]
    public void ThrowsOriginalExceptionWhenSetterThrows()
    {
        // Arrange
        var action = new ChangePropertyAction
        {
            TargetObject = new PropertyTarget(),
            PropertyName = nameof(PropertyTarget.Guarded),
            Parameter = "value"
        };

        // Act
        var exception = Assert.Throws<InvalidOperationException>(() => action.Execute(new TestBindable(), null));

        // Assert
        Assert.Equal("failed value", exception.Message);
    }
}
