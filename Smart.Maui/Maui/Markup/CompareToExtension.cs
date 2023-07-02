namespace Smart.Maui.Markup;

using Smart.Maui.Data;
using Smart.Maui.Expressions;

public sealed class CompareToBoolExtension : IMarkupExtension<CompareToBoolConverter>
{
    public ICompareExpression? Expression { get; set; }

    public CompareToBoolConverter ProvideValue(IServiceProvider serviceProvider) =>
        new() { Expression = Expression ?? CompareExpressions.Equal, TrueValue = true, FalseValue = false };

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}

public sealed class CompareToTextExtension : IMarkupExtension<CompareToTextConverter>
{
    public ICompareExpression? Expression { get; set; }

    public string? True { get; set; }

    public string? False { get; set; }

    public CompareToTextConverter ProvideValue(IServiceProvider serviceProvider) =>
        new() { Expression = Expression ?? CompareExpressions.Equal, TrueValue = True, FalseValue = False };

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}

public sealed class CompareToColorExtension : IMarkupExtension<CompareToColorConverter>
{
    public ICompareExpression? Expression { get; set; }

    public Color True { get; set; } = Colors.Transparent;

    public Color False { get; set; } = Colors.Transparent;

    public CompareToColorConverter ProvideValue(IServiceProvider serviceProvider) =>
        new() { Expression = Expression ?? CompareExpressions.Equal, TrueValue = True, FalseValue = False };

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}
