namespace Smart.Maui.Interactivity;

using System.Globalization;

using Microsoft.Maui.Controls;

public sealed class CallMethodActionTests
{
    private sealed class TestBindable : BindableObject;

    private interface IArgument;

    private sealed class Argument : IArgument;

    private sealed class MethodTarget
    {
        public bool InterfaceCalled { get; private set; }

        public object? InterfaceReceived { get; private set; }

        public bool ObjectCalled { get; private set; }

        public object? ObjectReceived { get; private set; }

        public bool ParameterlessCalled { get; private set; }

        public long LongReceived { get; private set; }

        public DayOfWeek DayReceived { get; private set; }

        public string Failure { get; } = "failed";

        public void InvokeInterface(IArgument argument)
        {
            InterfaceCalled = true;
            InterfaceReceived = argument;
        }

        public void InvokeObject(object argument)
        {
            ObjectCalled = true;
            ObjectReceived = argument;
        }

        public void InvokeParameterless() => ParameterlessCalled = true;

        public void InvokeLong(long value) => LongReceived = value;

        public void InvokeDay(DayOfWeek day) => DayReceived = day;

        public void Fail() => throw new InvalidOperationException(Failure);

        public void FailWith(object argument) => throw new InvalidOperationException($"{Failure} {argument}");
    }

    private sealed class OtherTarget
    {
        public object? ObjectReceived { get; private set; }

        public void InvokeObject(object argument) => ObjectReceived = argument;
    }

    private sealed class MethodHost : BindableObject
    {
        public object? ObjectReceived { get; private set; }

        public void InvokeObject(object argument) => ObjectReceived = argument;
    }

    private sealed class OverloadTarget
    {
        public object? Received { get; private set; }

        public void Select(IArgument argument) => Received = argument;

        public void Select(string text) => Received = text;
    }

    private sealed class NullConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => null;

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
    }

    //------------------------------------------------------------------
    // Action
    //------------------------------------------------------------------

    [Fact]
    public void ResolvesMethodWhenArgumentIsAssignableToParameterType()
    {
        // Arrange
        var target = new MethodTarget();
        var argument = new Argument();
        var action = new CallMethodAction
        {
            TargetObject = target,
            MethodName = nameof(MethodTarget.InvokeInterface),
            MethodParameter = argument
        };

        // Act
        action.Execute(new TestBindable(), null);

        // Assert
        Assert.True(target.InterfaceCalled);
        Assert.Same(argument, target.InterfaceReceived);
    }

    [Fact]
    public void PassesEventParameterWhenMethodParameterIsNotSet()
    {
        // Arrange
        var target = new MethodTarget();
        var action = new CallMethodAction
        {
            TargetObject = target,
            MethodName = nameof(MethodTarget.InvokeObject)
        };

        // Act
        action.Execute(new TestBindable(), "eventArgument");

        // Assert
        Assert.True(target.ObjectCalled);
        Assert.Equal("eventArgument", target.ObjectReceived);
    }

    [Fact]
    public void PassesMethodParameterWhenSet()
    {
        // Arrange
        var target = new MethodTarget();
        var action = new CallMethodAction
        {
            TargetObject = target,
            MethodName = nameof(MethodTarget.InvokeObject),
            MethodParameter = "methodParameter"
        };

        // Act
        action.Execute(new TestBindable(), "eventArgument");

        // Assert
        Assert.True(target.ObjectCalled);
        Assert.Equal("methodParameter", target.ObjectReceived);
    }

    [Fact]
    public void CallsMethodOfAssociatedObjectWhenTargetObjectIsNotSet()
    {
        // Arrange
        var host = new MethodHost();
        var action = new CallMethodAction
        {
            MethodName = nameof(MethodHost.InvokeObject)
        };

        // Act
        action.Execute(host, "eventArgument");

        // Assert
        Assert.Equal("eventArgument", host.ObjectReceived);
    }

    [Fact]
    public void PassesNullWhenConverterReturnsNull()
    {
        // Arrange
        var target = new MethodTarget();
        var action = new CallMethodAction
        {
            TargetObject = target,
            MethodName = nameof(MethodTarget.InvokeObject),
            Converter = new NullConverter()
        };

        // Act
        action.Execute(new TestBindable(), "eventArgument");

        // Assert
        Assert.True(target.ObjectCalled);
        Assert.Null(target.ObjectReceived);
    }

    [Fact]
    public void PassesNullWhenMethodParameterIsSetToNull()
    {
        // Arrange
        var target = new MethodTarget();
        var action = new CallMethodAction
        {
            TargetObject = target,
            MethodName = nameof(MethodTarget.InvokeObject),
            MethodParameter = null
        };

        // Act
        action.Execute(new TestBindable(), "eventArgument");

        // Assert
        Assert.True(target.ObjectCalled);
        Assert.Null(target.ObjectReceived);
    }

    [Fact]
    public void CallsParameterlessMethod()
    {
        // Arrange
        var target = new MethodTarget();
        var action = new CallMethodAction
        {
            TargetObject = target,
            MethodName = nameof(MethodTarget.InvokeParameterless)
        };

        // Act
        action.Execute(new TestBindable(), "eventArgument");

        // Assert
        Assert.True(target.ParameterlessCalled);
    }

    [Fact]
    public void ConvertsWideningArgumentToMethodParameterType()
    {
        // Arrange
        var target = new MethodTarget();
        var action = new CallMethodAction
        {
            TargetObject = target,
            MethodName = nameof(MethodTarget.InvokeLong)
        };

        // Act
        action.Execute(new TestBindable(), 42);

        // Assert
        Assert.Equal(42L, target.LongReceived);
    }

    [Fact]
    public void ConvertsEnumArgumentFromUnderlyingValue()
    {
        // Arrange
        var target = new MethodTarget();
        var action = new CallMethodAction
        {
            TargetObject = target,
            MethodName = nameof(MethodTarget.InvokeDay)
        };

        // Act
        action.Execute(new TestBindable(), 3);

        // Assert
        Assert.Equal(DayOfWeek.Wednesday, target.DayReceived);
    }

    [Fact]
    public void ResolvesAgainWhenMethodNameChanged()
    {
        // Arrange
        var target = new MethodTarget();
        var action = new CallMethodAction
        {
            TargetObject = target,
            MethodName = nameof(MethodTarget.InvokeObject)
        };
        action.Execute(new TestBindable(), "eventArgument");

        // Act
        action.MethodName = nameof(MethodTarget.InvokeParameterless);
        action.Execute(new TestBindable(), "eventArgument");

        // Assert
        Assert.True(target.ObjectCalled);
        Assert.True(target.ParameterlessCalled);
    }

    [Fact]
    public void ResolvesAgainWhenTargetTypeChanged()
    {
        // Arrange
        var first = new MethodTarget();
        var second = new OtherTarget();
        var action = new CallMethodAction
        {
            TargetObject = first,
            MethodName = nameof(MethodTarget.InvokeObject)
        };
        action.Execute(new TestBindable(), "first");

        // Act
        action.TargetObject = second;
        action.Execute(new TestBindable(), "second");

        // Assert
        Assert.Equal("first", first.ObjectReceived);
        Assert.Equal("second", second.ObjectReceived);
    }

    [Fact]
    public void ResolvesAgainWhenMethodParameterTypeChanged()
    {
        // Arrange
        var target = new OverloadTarget();
        var argument = new Argument();
        var action = new CallMethodAction
        {
            TargetObject = target,
            MethodName = nameof(OverloadTarget.Select),
            MethodParameter = argument
        };
        action.Execute(new TestBindable(), null);
        var first = target.Received;

        // Act
        action.MethodParameter = "text";
        action.Execute(new TestBindable(), null);

        // Assert
        Assert.Same(argument, first);
        Assert.Equal("text", target.Received);
    }

    [Fact]
    public void ThrowsOriginalExceptionWhenMethodThrows()
    {
        // Arrange
        var action = new CallMethodAction
        {
            TargetObject = new MethodTarget(),
            MethodName = nameof(MethodTarget.Fail)
        };

        // Act
        var exception = Assert.Throws<InvalidOperationException>(() => action.Execute(new TestBindable(), "eventArgument"));

        // Assert
        Assert.Equal("failed", exception.Message);
    }

    [Fact]
    public void ThrowsOriginalExceptionWhenMethodWithArgumentThrows()
    {
        // Arrange
        var action = new CallMethodAction
        {
            TargetObject = new MethodTarget(),
            MethodName = nameof(MethodTarget.FailWith)
        };

        // Act
        var exception = Assert.Throws<InvalidOperationException>(() => action.Execute(new TestBindable(), "eventArgument"));

        // Assert
        Assert.Equal("failed eventArgument", exception.Message);
    }
}
