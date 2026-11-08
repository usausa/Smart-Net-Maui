namespace Smart.Maui;

using Microsoft.Maui.Controls;

using Smart.Maui.Interactivity;

public sealed class ElementExtensionsTests
{
    [Fact]
    public void FindParentDoesNotReturnSelf()
    {
        // Arrange
        _ = TestDispatcher.Install();
        var inner = new ContentView();
        var outer = new ContentView { Content = inner };
        _ = new ContentPage { Content = outer };

        // Act
        var parent = inner.FindParent<ContentView>();

        // Assert
        Assert.Same(outer, parent);
    }

    [Fact]
    public void FindParentReturnsRoot()
    {
        // Arrange
        _ = TestDispatcher.Install();
        var label = new Label();
        var page = new ContentPage { Content = label };

        // Act
        var parent = label.FindParent<ContentPage>();

        // Assert
        Assert.Same(page, parent);
    }

    [Fact]
    public void FindParentReturnsNullWhenNotFound()
    {
        // Arrange
        _ = TestDispatcher.Install();
        var label = new Label();
        _ = new ContentPage { Content = label };

        // Act
        var parent = label.FindParent<ScrollView>();

        // Assert
        Assert.Null(parent);
    }
}
