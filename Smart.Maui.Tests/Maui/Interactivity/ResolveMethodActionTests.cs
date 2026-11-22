namespace Smart.Maui.Interactivity;

using Microsoft.Maui.Controls;

using Smart.Mvvm.Messaging;

public sealed class ResolveMethodActionTests
{
    private sealed class TestBindable : BindableObject;

    private sealed class MethodTarget
    {
        private readonly string text = "described";

        private readonly string summary = "summarized";

        public string Describe() => text;

        public string Format(string value) => text + value;

        public string Summarize() => summary;

        public string Fail() => throw new InvalidOperationException(text);
    }

    private sealed class OtherTarget
    {
        private readonly string text = "other";

        public string Describe() => text;
    }

    private sealed class MethodHost : BindableObject
    {
        private readonly string text = "host";

        public string Describe() => text;
    }

    [Fact]
    public void ResolvesMethodOfTargetObject()
    {
        // Arrange
        var action = new ResolveMethodAction { TargetObject = new MethodTarget(), MethodName = nameof(MethodTarget.Describe) };
        var args = new ResolveEventArgs();

        // Act
        action.Execute(new TestBindable(), args);

        // Assert
        Assert.Equal("described", args.Result);
    }

    [Fact]
    public void ResolvesMethodOfAssociatedObjectWhenTargetObjectIsNotSet()
    {
        // Arrange
        var action = new ResolveMethodAction { MethodName = nameof(MethodHost.Describe) };
        var args = new ResolveEventArgs();

        // Act
        action.Execute(new MethodHost(), args);

        // Assert
        Assert.Equal("host", args.Result);
    }

    [Fact]
    public void IgnoresOtherParameter()
    {
        // Arrange
        var action = new ResolveMethodAction { TargetObject = new MethodTarget(), MethodName = nameof(MethodTarget.Describe) };

        // Act
        var exception = Record.Exception(() => action.Execute(new TestBindable(), EventArgs.Empty));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void IgnoresMethodWithParameters()
    {
        // Arrange
        var action = new ResolveMethodAction { TargetObject = new MethodTarget(), MethodName = nameof(MethodTarget.Format) };
        var args = new ResolveEventArgs();

        // Act
        action.Execute(new TestBindable(), args);

        // Assert
        Assert.Null(args.Result);
    }

    [Fact]
    public void ResolvesAgainWhenMethodNameChanged()
    {
        // Arrange
        var action = new ResolveMethodAction { TargetObject = new MethodTarget(), MethodName = nameof(MethodTarget.Describe) };
        var first = new ResolveEventArgs();
        var second = new ResolveEventArgs();
        action.Execute(new TestBindable(), first);

        // Act
        action.MethodName = nameof(MethodTarget.Summarize);
        action.Execute(new TestBindable(), second);

        // Assert
        Assert.Equal("described", first.Result);
        Assert.Equal("summarized", second.Result);
    }

    [Fact]
    public void ResolvesAgainWhenTargetTypeChanged()
    {
        // Arrange
        var action = new ResolveMethodAction { TargetObject = new MethodTarget(), MethodName = nameof(MethodTarget.Describe) };
        var first = new ResolveEventArgs();
        var second = new ResolveEventArgs();
        action.Execute(new TestBindable(), first);

        // Act
        action.TargetObject = new OtherTarget();
        action.Execute(new TestBindable(), second);

        // Assert
        Assert.Equal("described", first.Result);
        Assert.Equal("other", second.Result);
    }

    [Fact]
    public void ThrowsOriginalExceptionWhenMethodThrows()
    {
        // Arrange
        var action = new ResolveMethodAction { TargetObject = new MethodTarget(), MethodName = nameof(MethodTarget.Fail) };
        var args = new ResolveEventArgs();

        // Act
        var exception = Assert.Throws<InvalidOperationException>(() => action.Execute(new TestBindable(), args));

        // Assert
        Assert.Equal("described", exception.Message);
    }
}
