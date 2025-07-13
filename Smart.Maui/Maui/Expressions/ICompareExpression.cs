namespace Smart.Mvvm.Expressions;

public interface ICompareExpression
{
    bool Eval(object? left, object? right);
}
