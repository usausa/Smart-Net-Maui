namespace Smart.Maui.Expressions;

public sealed class BinaryExpressionsTest
{
    [Fact]
    public void MaxReturnsLarger()
    {
        // Act & Assert
        Assert.Equal(5, BinaryExpressions.Max.Eval(5, 3));
    }

    [Fact]
    public void MaxReturnsRightWhenLarger()
    {
        // Act & Assert
        Assert.Equal(7, BinaryExpressions.Max.Eval(3, 7));
    }

    [Fact]
    public void MinReturnsSmaller()
    {
        // Act & Assert
        Assert.Equal(3, BinaryExpressions.Min.Eval(5, 3));
    }

    [Fact]
    public void MinReturnsLeftWhenSmaller()
    {
        // Act & Assert
        Assert.Equal(2, BinaryExpressions.Min.Eval(2, 9));
    }

    [Fact]
    public void AddIntegers()
    {
        // Act & Assert
        Assert.Equal(7, BinaryExpressions.Add.Eval(3, 4));
    }

    [Fact]
    public void AddDoubles()
    {
        // Act & Assert
        Assert.Equal(2.5d, BinaryExpressions.Add.Eval(1.0d, 1.5d));
    }

    [Fact]
    public void SubtractIntegers()
    {
        // Act & Assert
        Assert.Equal(2, BinaryExpressions.Sub.Eval(5, 3));
    }

    [Fact]
    public void SubtractLongs()
    {
        // Act & Assert
        Assert.Equal(10L, BinaryExpressions.Sub.Eval(15L, 5L));
    }

    [Fact]
    public void NullLeftReturnsNull()
    {
        // Act & Assert
        Assert.Null(BinaryExpressions.Add.Eval(null, 1));
    }

    [Fact]
    public void NullRightReturnsNull()
    {
        // Act & Assert
        Assert.Null(BinaryExpressions.Add.Eval(1, null));
    }

    [Fact]
    public void MaxWithNullRightReturnsLeft()
    {
        // Act & Assert
        Assert.Equal(5, BinaryExpressions.Max.Eval(5, null));
    }
}
