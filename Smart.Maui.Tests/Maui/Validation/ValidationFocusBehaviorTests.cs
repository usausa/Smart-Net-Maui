namespace Smart.Maui.Validation;

using Microsoft.Maui.Controls;

using Smart.Maui.Interactivity;
using Smart.Mvvm.ViewModels;

using SmartBehavior = Smart.Maui.Behavior;

public sealed class ValidationFocusBehaviorTests
{
    private static Entry Field(string key)
    {
        var entry = new Entry();
        SmartBehavior.SetKey(entry, key);
        return entry;
    }

    private static ErrorInfo Errors(params string[] keys)
    {
        var errors = new ErrorInfo();
        foreach (var key in keys)
        {
            errors.AddError(key, "error");
        }
        return errors;
    }

    [Fact]
    public void FindsFieldInLayout()
    {
        // Arrange
        var expected = Field("A");
        var root = new ContentView { Content = new VerticalStackLayout { Field("B"), expected } };
        using var errors = Errors("A");

        // Act
        var target = ValidationFocusBehavior.FindTarget(root, errors);

        // Assert
        Assert.Same(expected, target);
    }

    [Fact]
    public void FindsFieldInsideBorder()
    {
        // Arrange
        var expected = Field("A");
        var root = new ContentView { Content = new VerticalStackLayout { new Border { Content = expected } } };
        using var errors = Errors("A");

        // Act
        var target = ValidationFocusBehavior.FindTarget(root, errors);

        // Assert
        Assert.Same(expected, target);
    }

    [Fact]
    public void FindsFieldThatIsContentOfContentView()
    {
        // Arrange
        var expected = Field("A");
        var root = new ContentView { Content = new VerticalStackLayout { new ContentView { Content = expected } } };
        using var errors = Errors("A");

        // Act
        var target = ValidationFocusBehavior.FindTarget(root, errors);

        // Assert
        Assert.Same(expected, target);
    }

    [Fact]
    public void FindsFieldInsideBorderInScrollView()
    {
        // Arrange
        var expected = Field("A");
        var root = new ContentView { Content = new ScrollView { Content = new VerticalStackLayout { new Border { Content = expected } } } };
        using var errors = Errors("A");

        // Act
        var target = ValidationFocusBehavior.FindTarget(root, errors);

        // Assert
        Assert.Same(expected, target);
    }

    [Fact]
    public void FindsFieldInsideTemplatedContentView()
    {
        // Arrange
        TestDispatcher.Install();
        var expected = Field("A");
        var card = new ContentView
        {
            ControlTemplate = new ControlTemplate(static () => new Border { Content = new ContentPresenter() }),
            Content = new VerticalStackLayout { new Border { Content = expected } }
        };
        var root = new ContentView { Content = new VerticalStackLayout { card } };
        using var errors = Errors("A");

        // Act
        var target = ValidationFocusBehavior.FindTarget(root, errors);

        // Assert
        Assert.Same(expected, target);
    }

    [Fact]
    public void ReturnsFirstFieldWithError()
    {
        // Arrange
        var expected = Field("B");
        var root = new ContentView
        {
            Content = new VerticalStackLayout
            {
                new Border { Content = Field("A") },
                new Border { Content = expected },
                new Border { Content = Field("C") }
            }
        };
        using var errors = Errors("C", "B");

        // Act
        var target = ValidationFocusBehavior.FindTarget(root, errors);

        // Assert
        Assert.Same(expected, target);
    }

    [Fact]
    public void SkipsHiddenAndDisabledFields()
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
        using var errors = Errors("A");

        // Act
        var target = ValidationFocusBehavior.FindTarget(root, errors);

        // Assert
        Assert.Same(expected, target);
    }

    [Fact]
    public void SkipsFieldsInsideHiddenContainer()
    {
        // Arrange
        var expected = Field("A");
        var root = new ContentView
        {
            Content = new VerticalStackLayout
            {
                new Border { IsVisible = false, Content = Field("A") },
                new Border { Content = expected }
            }
        };
        using var errors = Errors("A");

        // Act
        var target = ValidationFocusBehavior.FindTarget(root, errors);

        // Assert
        Assert.Same(expected, target);
    }

    [Fact]
    public void ReturnsNullWithoutError()
    {
        // Arrange
        var root = new ContentView { Content = new VerticalStackLayout { new Border { Content = Field("A") } } };
        using var errors = Errors("B");

        // Act
        var target = ValidationFocusBehavior.FindTarget(root, errors);

        // Assert
        Assert.Null(target);
    }
}
