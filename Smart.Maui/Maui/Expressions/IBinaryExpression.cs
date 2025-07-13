namespace Smart.Mvvm.Expressions;

public interface IBinaryExpression
{
    object? Eval(object? left, object? right);
}
