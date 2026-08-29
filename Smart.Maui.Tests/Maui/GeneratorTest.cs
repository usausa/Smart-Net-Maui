namespace Smart.Maui;

using Microsoft.CodeAnalysis;

public sealed class GeneratorTest
{
    private const string Source =
        """
        using Smart.Maui;
        using Microsoft.Maui.Controls;

        namespace Test;

        public partial class TestElement : BindableObject
        {
            [BindableProperty]
            public partial string? Text { get; set; }
        }
        """;

    // ------------------------------------------------------------
    // Basic
    // ------------------------------------------------------------

    [Fact]
    public void PropertyGeneratesFieldAndAccessor()
    {
        // Arrange & Act
        var generated = GeneratorTestHelper.GetGeneratedSource(Source);

        // Assert
        Assert.Contains("public static readonly global::Microsoft.Maui.Controls.BindableProperty TextProperty = global::Microsoft.Maui.Controls.BindableProperty.Create(", generated, StringComparison.Ordinal);
        Assert.Contains("nameof(Text)", generated, StringComparison.Ordinal);
        Assert.Contains("typeof(string)", generated, StringComparison.Ordinal);
        Assert.Contains("typeof(TestElement))", generated, StringComparison.Ordinal);
        Assert.Contains("public partial string? Text", generated, StringComparison.Ordinal);
        Assert.Contains("get => (string?)GetValue(TextProperty);", generated, StringComparison.Ordinal);
        Assert.Contains("set => SetValue(TextProperty, value);", generated, StringComparison.Ordinal);
    }

    [Fact]
    public void PropertyProducesNoCompilationError()
    {
        // Arrange & Act
        var diagnostics = GeneratorTestHelper.GetDiagnosticsAll(Source);

        // Assert
        Assert.DoesNotContain(diagnostics, static x => x.Severity == DiagnosticSeverity.Error);
    }

    [Fact]
    public void MultiplePropertiesGenerateInOneClass()
    {
        // Arrange
        const string source =
            """
            using Smart.Maui;
            using Microsoft.Maui.Controls;

            namespace Test;

            public partial class TestElement : BindableObject
            {
                [BindableProperty]
                public partial string? Text { get; set; }

                [BindableProperty]
                public partial int Number { get; set; }
            }
            """;

        // Act
        var generated = GeneratorTestHelper.GetGeneratedSource(source);

        // Assert
        Assert.Contains("TextProperty", generated, StringComparison.Ordinal);
        Assert.Contains("NumberProperty", generated, StringComparison.Ordinal);
        Assert.Contains("get => (int)GetValue(NumberProperty);", generated, StringComparison.Ordinal);
    }

    [Fact]
    public void ObjectPropertyOmitsCast()
    {
        // Arrange
        const string source =
            """
            using Smart.Maui;
            using Microsoft.Maui.Controls;

            namespace Test;

            public partial class TestElement : BindableObject
            {
                [BindableProperty]
                public partial object? Value { get; set; }
            }
            """;

        // Act
        var generated = GeneratorTestHelper.GetGeneratedSource(source);

        // Assert
        Assert.Contains("get => GetValue(ValueProperty);", generated, StringComparison.Ordinal);
    }

    [Fact]
    public void NestedClassGeneratesContainingTypes()
    {
        // Arrange
        const string source =
            """
            using Smart.Maui;
            using Microsoft.Maui.Controls;

            namespace Test;

            public partial class Outer
            {
                public partial class TestElement : BindableObject
                {
                    [BindableProperty]
                    public partial string? Text { get; set; }
                }
            }
            """;

        // Act
        var generated = GeneratorTestHelper.GetGeneratedSource(source);

        // Assert
        Assert.Contains("partial class Outer", generated, StringComparison.Ordinal);
        Assert.Contains("partial class TestElement", generated, StringComparison.Ordinal);
    }

    // ------------------------------------------------------------
    // Default value
    // ------------------------------------------------------------

    [Fact]
    public void DefaultValueIsApplied()
    {
        // Arrange
        const string source =
            """
            using Smart.Maui;
            using Microsoft.Maui.Controls;

            namespace Test;

            public partial class TestElement : BindableObject
            {
                [BindableProperty(DefaultValue = "abc")]
                public partial string? Text { get; set; }
            }
            """;

        // Act
        var generated = GeneratorTestHelper.GetGeneratedSource(source);

        // Assert
        Assert.Contains("defaultValue: \"abc\"", generated, StringComparison.Ordinal);
    }

    [Fact]
    public void DefaultValueIsCastToPropertyType()
    {
        // Arrange
        const string source =
            """
            using Smart.Maui;
            using Microsoft.Maui.Controls;

            namespace Test;

            public partial class TestElement : BindableObject
            {
                [BindableProperty(DefaultValue = 1)]
                public partial double Scale { get; set; }
            }
            """;

        // Act
        var generated = GeneratorTestHelper.GetGeneratedSource(source);

        // Assert
        Assert.Contains("defaultValue: (double)1", generated, StringComparison.Ordinal);
    }

    [Fact]
    public void DefaultValueExpressionIsApplied()
    {
        // Arrange
        const string source =
            """
            using Smart.Maui;
            using Microsoft.Maui.Controls;

            namespace Test;

            public partial class TestElement : BindableObject
            {
                [BindableProperty(DefaultValueExpression = "global::Test.TestElement.CreateDefault()")]
                public partial string? Text { get; set; }

                public static string CreateDefault() => "abc";
            }
            """;

        // Act
        var generated = GeneratorTestHelper.GetGeneratedSource(source);

        // Assert
        Assert.Contains("defaultValue: global::Test.TestElement.CreateDefault()", generated, StringComparison.Ordinal);
    }

    [Fact]
    public void DefaultBindingModeIsApplied()
    {
        // Arrange
        const string source =
            """
            using Smart.Maui;
            using Microsoft.Maui.Controls;

            namespace Test;

            public partial class TestElement : BindableObject
            {
                [BindableProperty(DefaultBindingMode = BindingMode.TwoWay)]
                public partial string? Text { get; set; }
            }
            """;

        // Act
        var generated = GeneratorTestHelper.GetGeneratedSource(source);

        // Assert
        Assert.Contains("defaultBindingMode: global::Microsoft.Maui.Controls.BindingMode.TwoWay", generated, StringComparison.Ordinal);
    }

    // ------------------------------------------------------------
    // Callback
    // ------------------------------------------------------------

    [Fact]
    public void PropertyChangedCallbackIsApplied()
    {
        // Arrange
        const string source =
            """
            using Smart.Maui;
            using Microsoft.Maui.Controls;

            namespace Test;

            public partial class TestElement : BindableObject
            {
                [BindableProperty(PropertyChanged = nameof(OnTextChanged))]
                public partial string? Text { get; set; }

                private void OnTextChanged(string? oldValue, string? newValue)
                {
                }
            }
            """;

        // Act
        var generated = GeneratorTestHelper.GetGeneratedSource(source);

        // Assert
        Assert.Contains("propertyChanged: static (bindable, oldValue, newValue) => ((TestElement)bindable).OnTextChanged((string?)oldValue, (string?)newValue)", generated, StringComparison.Ordinal);
    }

    [Fact]
    public void PropertyChangingCallbackIsApplied()
    {
        // Arrange
        const string source =
            """
            using Smart.Maui;
            using Microsoft.Maui.Controls;

            namespace Test;

            public partial class TestElement : BindableObject
            {
                [BindableProperty(PropertyChanging = nameof(OnTextChanging))]
                public partial string? Text { get; set; }

                private void OnTextChanging(string? oldValue, string? newValue)
                {
                }
            }
            """;

        // Act
        var generated = GeneratorTestHelper.GetGeneratedSource(source);

        // Assert
        Assert.Contains("propertyChanging: static (bindable, oldValue, newValue) => ((TestElement)bindable).OnTextChanging((string?)oldValue, (string?)newValue)", generated, StringComparison.Ordinal);
    }

    [Fact]
    public void PropertyChangedNoArgumentCallbackIsApplied()
    {
        // Arrange
        const string source =
            """
            using Smart.Maui;
            using Microsoft.Maui.Controls;

            namespace Test;

            public partial class TestElement : BindableObject
            {
                [BindableProperty(PropertyChanged = nameof(OnTextChanged))]
                public partial string? Text { get; set; }

                private void OnTextChanged()
                {
                }
            }
            """;

        // Act
        var generated = GeneratorTestHelper.GetGeneratedSource(source);

        // Assert
        Assert.Contains("propertyChanged: static (bindable, oldValue, newValue) => ((TestElement)bindable).OnTextChanged()", generated, StringComparison.Ordinal);
    }

    [Fact]
    public void CoerceCallbackIsApplied()
    {
        // Arrange
        const string source =
            """
            using Smart.Maui;
            using Microsoft.Maui.Controls;

            namespace Test;

            public partial class TestElement : BindableObject
            {
                [BindableProperty(Coerce = nameof(CoerceScale))]
                public partial double Scale { get; set; }

                private double CoerceScale(double value) => value;
            }
            """;

        // Act
        var generated = GeneratorTestHelper.GetGeneratedSource(source);

        // Assert
        Assert.Contains("coerceValue: static (bindable, value) => ((TestElement)bindable).CoerceScale((double)value)", generated, StringComparison.Ordinal);
    }

    [Fact]
    public void ValidateCallbackIsApplied()
    {
        // Arrange
        const string source =
            """
            using Smart.Maui;
            using Microsoft.Maui.Controls;

            namespace Test;

            public partial class TestElement : BindableObject
            {
                [BindableProperty(Validate = nameof(ValidateScale))]
                public partial double Scale { get; set; }

                private static bool ValidateScale(double value) => true;
            }
            """;

        // Act
        var generated = GeneratorTestHelper.GetGeneratedSource(source);

        // Assert
        Assert.Contains("validateValue: static (bindable, value) => ValidateScale((double)value)", generated, StringComparison.Ordinal);
    }

    [Fact]
    public void AllCallbacksProduceNoCompilationError()
    {
        // Arrange
        const string source =
            """
            using Smart.Maui;
            using Microsoft.Maui.Controls;

            namespace Test;

            public partial class TestElement : BindableObject
            {
                [BindableProperty(DefaultValue = 0d, DefaultBindingMode = BindingMode.TwoWay, PropertyChanged = nameof(OnScaleChanged), PropertyChanging = nameof(OnScaleChanging), Coerce = nameof(CoerceScale), Validate = nameof(ValidateScale))]
                public partial double Scale { get; set; }

                private void OnScaleChanged(double oldValue, double newValue)
                {
                }

                private void OnScaleChanging(double oldValue, double newValue)
                {
                }

                private double CoerceScale(double value) => value;

                private bool ValidateScale(double value) => true;
            }
            """;

        // Act
        var diagnostics = GeneratorTestHelper.GetDiagnosticsAll(source);

        // Assert
        Assert.DoesNotContain(diagnostics, static x => x.Severity == DiagnosticSeverity.Error);
    }
}
