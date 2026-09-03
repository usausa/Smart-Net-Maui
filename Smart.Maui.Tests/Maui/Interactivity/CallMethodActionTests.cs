namespace Smart.Maui.Interactivity;

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
}
