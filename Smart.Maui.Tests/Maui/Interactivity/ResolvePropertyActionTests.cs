namespace Smart.Maui.Interactivity;

using System.Reflection;

using Microsoft.Maui.Controls;

using Smart.Mvvm.Messaging;

public sealed class ResolvePropertyActionTests
{
    private sealed class TestBindable : BindableObject;

    private sealed class PropertyTarget
    {
        public string Name { get; } = "target";

        public string Broken => throw new InvalidOperationException(Name);
    }

    private sealed class PropertyHost : BindableObject
    {
        public string Name { get; } = "host";
    }

    [Fact]
    public void ResolvesPropertyOfTargetObject()
    {
        // Arrange
        var action = new ResolvePropertyAction { TargetObject = new PropertyTarget(), PropertyName = nameof(PropertyTarget.Name) };
        var args = new ResolveEventArgs();

        // Act
        action.Execute(new TestBindable(), args);

        // Assert
        Assert.Equal("target", args.Result);
    }

    [Fact]
    public void ResolvesPropertyOfAssociatedObjectWhenTargetObjectIsNotSet()
    {
        // Arrange
        var action = new ResolvePropertyAction { PropertyName = nameof(PropertyHost.Name) };
        var args = new ResolveEventArgs();

        // Act
        action.Execute(new PropertyHost(), args);

        // Assert
        Assert.Equal("host", args.Result);
    }

    [Fact]
    public void IgnoresOtherParameter()
    {
        // Arrange
        var action = new ResolvePropertyAction { TargetObject = new PropertyTarget(), PropertyName = nameof(PropertyTarget.Name) };

        // Act
        var exception = Record.Exception(() => action.Execute(new TestBindable(), EventArgs.Empty));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void IgnoresNullPropertyName()
    {
        // Arrange
        var action = new ResolvePropertyAction { TargetObject = new PropertyTarget(), PropertyName = null! };
        var args = new ResolveEventArgs();

        // Act
        var exception = Record.Exception(() => action.Execute(new TestBindable(), args));

        // Assert
        Assert.Null(exception);
        Assert.Null(args.Result);
    }

    [Fact]
    public void ThrowsWrappedExceptionWhenGetterThrows()
    {
        // Arrange
        var action = new ResolvePropertyAction { TargetObject = new PropertyTarget(), PropertyName = nameof(PropertyTarget.Broken) };
        var args = new ResolveEventArgs();

        // Act
        var exception = Assert.Throws<TargetInvocationException>(() => action.Execute(new TestBindable(), args));

        // Assert
        var inner = Assert.IsType<InvalidOperationException>(exception.InnerException);
        Assert.Equal("target", inner.Message);
    }
}
