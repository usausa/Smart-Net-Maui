namespace Smart.Maui.Markup;

using Smart.Maui.Data;
using Smart.Maui.Expressions;

public sealed class CompareToBoolExtension : IMarkupExtension<CompareConverter<bool>>
{
    public ICompareExpression? Expression { get; set; }

    public CompareConverter<bool> ProvideValue(IServiceProvider serviceProvider) =>
        new() { Expression = Expression ?? CompareExpressions.Equal, TrueValue = true, FalseValue = false };

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}

public sealed class CompareToTextExtension : IMarkupExtension<CompareConverter<string>>
{
    public ICompareExpression? Expression { get; set; }

    public string True { get; set; } = string.Empty;

    public string False { get; set; } = string.Empty;

    public CompareConverter<string> ProvideValue(IServiceProvider serviceProvider) =>
        new() { Expression = Expression ?? CompareExpressions.Equal, TrueValue = True, FalseValue = False };

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}

public sealed class CompareToColorExtension : IMarkupExtension<CompareConverter<Color>>
{
    public ICompareExpression? Expression { get; set; }

    public Color True { get; set; } = Colors.Transparent;

    public Color False { get; set; } = Colors.Transparent;

    public CompareConverter<Color> ProvideValue(IServiceProvider serviceProvider) =>
        new() { Expression = Expression ?? CompareExpressions.Equal, TrueValue = True, FalseValue = False };

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);
}
