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

    [Fact]
    public void FindParentWithPredicateSkipsNotMatching()
    {
        // Arrange
        _ = TestDispatcher.Install();
        var (outer, _, label) = Build();

        // Act & Assert
        Assert.Same(outer, label.FindParent<ContentView>(static x => x.StyleId == "outer"));
        Assert.Null(label.FindParent<ContentView>(static _ => false));
    }

    [Fact]
    public void FindParentWithStatePassesState()
    {
        // Arrange
        _ = TestDispatcher.Install();
        var (outer, _, label) = Build();

        // Act & Assert
        Assert.Same(outer, label.FindParent<ContentView, string>("outer", static (x, s) => x.StyleId == s));
    }

    [Fact]
    public void FindParentOrSelfReturnsSelfFirst()
    {
        // Arrange
        _ = TestDispatcher.Install();
        var (_, inner, label) = Build();

        // Act & Assert
        Assert.Same(inner, inner.FindParentOrSelf<ContentView>());
        Assert.Same(inner, label.FindParentOrSelf<ContentView>());
    }

    [Fact]
    public void FindParentOrSelfWithPredicateChecksSelf()
    {
        // Arrange
        _ = TestDispatcher.Install();
        var (outer, inner, _) = Build();

        // Act & Assert
        Assert.Same(inner, inner.FindParentOrSelf<ContentView>(static x => x.StyleId == "inner"));
        Assert.Same(outer, inner.FindParentOrSelf<ContentView>(static x => x.StyleId == "outer"));
        Assert.Same(outer, inner.FindParentOrSelf<ContentView, string>("outer", static (x, s) => x.StyleId == s));
    }

    private static (ContentView Outer, ContentView Inner, Label Label) Build()
    {
        var label = new Label();
        var inner = new ContentView { StyleId = "inner", Content = label };
        var outer = new ContentView { StyleId = "outer", Content = inner };
        _ = new ContentPage { Content = outer };
        return (outer, inner, label);
    }
}
