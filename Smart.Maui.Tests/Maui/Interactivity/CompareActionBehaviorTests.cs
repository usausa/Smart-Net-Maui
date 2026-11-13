namespace Smart.Maui.Interactivity;

using Microsoft.Maui.Controls;

using Smart.Maui.Expressions;

public sealed class CompareActionBehaviorTests
{
    private sealed class RecordAction : IAction
    {
        public List<(BindableObject AssociatedObject, object? Parameter)> Calls { get; } = [];

        public void Execute(BindableObject associatedObject, object? parameter) => Calls.Add((associatedObject, parameter));
    }

    private static (Border Element, CompareActionBehavior Behavior, RecordAction Action) Create()
    {
        var element = new Border();
        var behavior = new CompareActionBehavior();
        var action = new RecordAction();
        behavior.Actions.Add(action);
        element.Behaviors.Add(behavior);
        return (element, behavior, action);
    }

    [Fact]
    public void ExecutesActionsWhenBindingEqualsParameter()
    {
        // Arrange
        var (element, behavior, action) = Create();
        behavior.Parameter = 5;

        // Act
        behavior.Binding = 5;

        // Assert
        var (associatedObject, parameter) = Assert.Single(action.Calls);
        Assert.Same(element, associatedObject);
        Assert.Null(parameter);
    }

    [Fact]
    public void DoesNotExecuteActionsWhenBindingDiffersFromParameter()
    {
        // Arrange
        var (_, behavior, action) = Create();
        behavior.Parameter = 5;

        // Act
        behavior.Binding = 6;

        // Assert
        Assert.Empty(action.Calls);
    }

    [Fact]
    public void UsesExpression()
    {
        // Arrange
        var (_, behavior, action) = Create();
        behavior.Expression = CompareExpressions.GreaterThan;
        behavior.Parameter = 5;

        // Act
        behavior.Binding = 6;

        // Assert
        Assert.Single(action.Calls);
    }

    [Fact]
    public void ReevaluatesWhenExpressionChanges()
    {
        // Arrange
        var (_, behavior, action) = Create();
        behavior.Parameter = 5;
        behavior.Binding = 6;

        // Act
        behavior.Expression = CompareExpressions.GreaterThan;

        // Assert
        Assert.Single(action.Calls);
    }

    [Fact]
    public void DoesNotExecuteActionsWhenDetached()
    {
        // Arrange
        var (element, behavior, action) = Create();
        element.Behaviors.Remove(behavior);
        behavior.Parameter = 5;

        // Act
        behavior.Binding = 5;

        // Assert
        Assert.Empty(action.Calls);
    }

    [Fact]
    public void ExpressionIsEqualByDefault()
    {
        // Arrange & Act
        var behavior = new CompareActionBehavior();

        // Assert
        Assert.Same(CompareExpressions.Equal, behavior.Expression);
    }
}
