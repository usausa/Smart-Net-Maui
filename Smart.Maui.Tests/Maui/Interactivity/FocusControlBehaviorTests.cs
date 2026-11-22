namespace Smart.Maui.Interactivity;

using Microsoft.Maui.Controls;

using SmartBehavior = Smart.Maui.Behavior;

public sealed class FocusControlBehaviorTests
{
    private static Entry Field(string key, bool focused = false)
    {
        var entry = new Entry();
        SmartBehavior.SetKey(entry, key);
        ((IView)entry).IsFocused = focused;
        return entry;
    }

    [Fact]
    public void FindsNamedFieldInLayout()
    {
        // Arrange
        var expected = Field("A");
        var root = new ContentView { Content = new VerticalStackLayout { Field("B"), expected } };

        // Act
        var target = FocusControlBehavior.FindTargetByName(root, "A");

        // Assert
        Assert.Same(expected, target);
    }

    [Fact]
    public void FindsNamedFieldInsideBorder()
    {
        // Arrange
        var expected = Field("A");
        var root = new ContentView { Content = new VerticalStackLayout { new Border { Content = expected } } };

        // Act
        var target = FocusControlBehavior.FindTargetByName(root, "A");

        // Assert
        Assert.Same(expected, target);
    }

    [Fact]
    public void FindsNamedFieldThatIsContentOfContentView()
    {
        // Arrange
        var expected = Field("A");
        var root = new ContentView { Content = new VerticalStackLayout { new ContentView { Content = expected } } };

        // Act
        var target = FocusControlBehavior.FindTargetByName(root, "A");

        // Assert
        Assert.Same(expected, target);
    }

    [Fact]
    public void SkipsHiddenAndDisabledNamedFields()
    {
        // Arrange
        var hidden = Field("A");
        hidden.IsVisible = false;
        var disabled = Field("A");
        disabled.IsEnabled = false;
        var expected = Field("A");
        var root = new ContentView
        {
            Content = new VerticalStackLayout
            {
                new Border { Content = hidden },
                new Border { Content = disabled },
                new Border { Content = expected }
            }
        };

        // Act
        var target = FocusControlBehavior.FindTargetByName(root, "A");

        // Assert
        Assert.Same(expected, target);
    }

    [Fact]
    public void ReturnsNullWhenNameIsMissing()
    {
        // Arrange
        var root = new ContentView { Content = new VerticalStackLayout { new Border { Content = Field("A") } } };

        // Act
        var target = FocusControlBehavior.FindTargetByName(root, "B");

        // Assert
        Assert.Null(target);
    }

    [Fact]
    public void FindsFocusedFieldInLayout()
    {
        // Arrange
        var root = new ContentView { Content = new VerticalStackLayout { Field("A"), Field("B", true) } };

        // Act
        var name = FocusControlBehavior.FindFocused(root);

        // Assert
        Assert.Equal("B", name);
    }

    [Fact]
    public void FindsFocusedFieldInsideBorder()
    {
        // Arrange
        var root = new ContentView
        {
            Content = new VerticalStackLayout
            {
                new Border { Content = Field("A") },
                new Border { Content = Field("B", true) }
            }
        };

        // Act
        var name = FocusControlBehavior.FindFocused(root);

        // Assert
        Assert.Equal("B", name);
    }

    [Fact]
    public void ReturnsNullWhenNothingIsFocused()
    {
        // Arrange
        var root = new ContentView { Content = new VerticalStackLayout { new Border { Content = Field("A") } } };

        // Act
        var name = FocusControlBehavior.FindFocused(root);

        // Assert
        Assert.Null(name);
    }
}
