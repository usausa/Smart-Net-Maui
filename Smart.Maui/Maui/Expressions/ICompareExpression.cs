namespace Smart.Maui.Expressions;

public interface ICompareExpression
{
    bool Eval(object? left, object? right);
}
