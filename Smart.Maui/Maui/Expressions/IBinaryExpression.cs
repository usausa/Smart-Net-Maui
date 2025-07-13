namespace Smart.Maui.Expressions;

public interface IBinaryExpression
{
    object? Eval(object? left, object? right);
}
