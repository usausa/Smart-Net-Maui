namespace Smart.Maui.Expressions;

public sealed class CompareExpressionsTest
{
    [Fact]
    public void EqualTrueWhenEqual()
    {
        // Act & Assert
        Assert.True(CompareExpressions.Equal.Eval(1, 1));
    }

    [Fact]
    public void EqualFalseWhenNotEqual()
    {
        // Act & Assert
        Assert.False(CompareExpressions.Equal.Eval(1, 2));
    }

    [Fact]
    public void EqualBothNullReturnsTrue()
    {
        // Act & Assert
        Assert.True(CompareExpressions.Equal.Eval(null, null));
    }

    [Fact]
    public void NotEqualTrueWhenDifferent()
    {
        // Act & Assert
        Assert.True(CompareExpressions.NotEqual.Eval(1, 2));
    }

    [Fact]
    public void NotEqualFalseWhenSame()
    {
        // Act & Assert
        Assert.False(CompareExpressions.NotEqual.Eval(1, 1));
    }

    [Fact]
    public void LessThanTrueWhenLess()
    {
        // Act & Assert
        Assert.True(CompareExpressions.LessThan.Eval(1, 2));
    }

    [Fact]
    public void LessThanFalseWhenEqual()
    {
        // Act & Assert
        Assert.False(CompareExpressions.LessThan.Eval(2, 2));
    }

    [Fact]
    public void LessThanFalseWhenGreater()
    {
        // Act & Assert
        Assert.False(CompareExpressions.LessThan.Eval(3, 2));
    }

    [Fact]
    public void LessThanOrEqualTrueWhenEqual()
    {
        // Act & Assert
        Assert.True(CompareExpressions.LessThanOrEqual.Eval(2, 2));
    }

    [Fact]
    public void LessThanOrEqualTrueWhenLess()
    {
        // Act & Assert
        Assert.True(CompareExpressions.LessThanOrEqual.Eval(1, 2));
    }

    [Fact]
    public void GreaterThanTrueWhenGreater()
    {
        // Act & Assert
        Assert.True(CompareExpressions.GreaterThan.Eval(3, 2));
    }

    [Fact]
    public void GreaterThanFalseWhenLess()
    {
        // Act & Assert
        Assert.False(CompareExpressions.GreaterThan.Eval(1, 2));
    }

    [Fact]
    public void GreaterThanOrEqualTrueWhenEqual()
    {
        // Act & Assert
        Assert.True(CompareExpressions.GreaterThanOrEqual.Eval(2, 2));
    }

    [Fact]
    public void GreaterThanOrEqualTrueWhenGreater()
    {
        // Act & Assert
        Assert.True(CompareExpressions.GreaterThanOrEqual.Eval(3, 2));
    }

    [Fact]
    public void EqualWithStringConversion()
    {
        // Act & Assert
        // left=int 5, right="5" — ConvertHelper converts "5" to int, then Equal
        Assert.True(CompareExpressions.Equal.Eval(5, "5"));
    }

    [Fact]
    public void LeftNullReturnsFalseForComparison()
    {
        // Act & Assert
        Assert.False(CompareExpressions.LessThan.Eval(null, 1));
    }
}
