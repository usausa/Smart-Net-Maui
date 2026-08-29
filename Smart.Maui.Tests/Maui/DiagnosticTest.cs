namespace Smart.Maui;

public sealed class DiagnosticTest
{
    // ------------------------------------------------------------
    // Property definition
    // ------------------------------------------------------------

    [Fact]
    public void Smu0001NotPartialEmitsDiagnostic()
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
                public string? Text { get; set; }
            }
            """;

        // Act
        var diagnostics = GeneratorTestHelper.GetDiagnostics(source);

        // Assert
        Assert.Contains(diagnostics, static x => x.Id == "SMU0001");
    }

    [Fact]
    public void Smu0002StaticPropertyEmitsDiagnostic()
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
                public static partial string? Text { get; set; }
            }
            """;

        // Act
        var diagnostics = GeneratorTestHelper.GetDiagnostics(source);

        // Assert
        Assert.Contains(diagnostics, static x => x.Id == "SMU0002");
    }

    [Fact]
    public void Smu0003AccessorModifierEmitsDiagnostic()
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
                public partial string? Text { get; private set; }
            }
            """;

        // Act
        var diagnostics = GeneratorTestHelper.GetDiagnostics(source);

        // Assert
        Assert.Contains(diagnostics, static x => x.Id == "SMU0003");
    }

    // ------------------------------------------------------------
    // Containing type
    // ------------------------------------------------------------

    [Fact]
    public void Smu0004ContainingTypeNotPartialEmitsDiagnostic()
    {
        // Arrange
        const string source =
            """
            using Smart.Maui;
            using Microsoft.Maui.Controls;

            namespace Test;

            public class TestElement : BindableObject
            {
                [BindableProperty]
                public partial string? Text { get; set; }
            }
            """;

        // Act
        var diagnostics = GeneratorTestHelper.GetDiagnostics(source);

        // Assert
        Assert.Contains(diagnostics, static x => x.Id == "SMU0004");
    }

    [Fact]
    public void Smu0005NotBindableObjectEmitsDiagnostic()
    {
        // Arrange
        const string source =
            """
            using Smart.Maui;

            namespace Test;

            public partial class TestElement
            {
                [BindableProperty]
                public partial string? Text { get; set; }
            }
            """;

        // Act
        var diagnostics = GeneratorTestHelper.GetDiagnostics(source);

        // Assert
        Assert.Contains(diagnostics, static x => x.Id == "SMU0005");
    }

    [Fact]
    public void Smu0006GenericTypeEmitsDiagnostic()
    {
        // Arrange
        const string source =
            """
            using Smart.Maui;
            using Microsoft.Maui.Controls;

            namespace Test;

            public partial class TestElement<T> : BindableObject
            {
                [BindableProperty]
                public partial string? Text { get; set; }
            }
            """;

        // Act
        var diagnostics = GeneratorTestHelper.GetDiagnostics(source);

        // Assert
        Assert.Contains(diagnostics, static x => x.Id == "SMU0006");
    }

    // ------------------------------------------------------------
    // Attribute argument
    // ------------------------------------------------------------

    [Fact]
    public void Smu0007DefaultValueConflictEmitsDiagnostic()
    {
        // Arrange
        const string source =
            """
            using Smart.Maui;
            using Microsoft.Maui.Controls;

            namespace Test;

            public partial class TestElement : BindableObject
            {
                [BindableProperty(DefaultValue = "abc", DefaultValueExpression = "\"abc\"")]
                public partial string? Text { get; set; }
            }
            """;

        // Act
        var diagnostics = GeneratorTestHelper.GetDiagnostics(source);

        // Assert
        Assert.Contains(diagnostics, static x => x.Id == "SMU0007");
    }

    [Fact]
    public void Smu0008CallbackNotFoundEmitsDiagnostic()
    {
        // Arrange
        const string source =
            """
            using Smart.Maui;
            using Microsoft.Maui.Controls;

            namespace Test;

            public partial class TestElement : BindableObject
            {
                [BindableProperty(PropertyChanged = "OnTextChanged")]
                public partial string? Text { get; set; }
            }
            """;

        // Act
        var diagnostics = GeneratorTestHelper.GetDiagnostics(source);

        // Assert
        Assert.Contains(diagnostics, static x => x.Id == "SMU0008");
    }

    [Fact]
    public void Smu0009InvalidCallbackSignatureEmitsDiagnostic()
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

                private void OnTextChanged(int value)
                {
                }
            }
            """;

        // Act
        var diagnostics = GeneratorTestHelper.GetDiagnostics(source);

        // Assert
        Assert.Contains(diagnostics, static x => x.Id == "SMU0009");
    }

    [Fact]
    public void Smu0009InvalidCoerceSignatureEmitsDiagnostic()
    {
        // Arrange
        const string source =
            """
            using Smart.Maui;
            using Microsoft.Maui.Controls;

            namespace Test;

            public partial class TestElement : BindableObject
            {
                [BindableProperty(Coerce = nameof(CoerceText))]
                public partial string? Text { get; set; }

                private void CoerceText(string? value)
                {
                }
            }
            """;

        // Act
        var diagnostics = GeneratorTestHelper.GetDiagnostics(source);

        // Assert
        Assert.Contains(diagnostics, static x => x.Id == "SMU0009");
    }

    [Fact]
    public void Smu0010InvalidDefaultValueEmitsDiagnostic()
    {
        // Arrange
        const string source =
            """
            using Smart.Maui;
            using Microsoft.Maui.Controls;

            namespace Test;

            public partial class TestElement : BindableObject
            {
                [BindableProperty(DefaultValue = new int[] { 1, 2 })]
                public partial int[]? Values { get; set; }
            }
            """;

        // Act
        var diagnostics = GeneratorTestHelper.GetDiagnostics(source);

        // Assert
        Assert.Contains(diagnostics, static x => x.Id == "SMU0010");
    }

    // ------------------------------------------------------------
    // Valid
    // ------------------------------------------------------------

    [Fact]
    public void ValidDefinitionEmitsNoDiagnostic()
    {
        // Arrange
        const string source =
            """
            using Smart.Maui;
            using Microsoft.Maui.Controls;

            namespace Test;

            public partial class TestElement : BindableObject
            {
                [BindableProperty(DefaultValue = 0d, DefaultBindingMode = BindingMode.TwoWay, PropertyChanged = nameof(OnScaleChanged), Coerce = nameof(CoerceScale), Validate = nameof(ValidateScale))]
                public partial double Scale { get; set; }

                private void OnScaleChanged(double oldValue, double newValue)
                {
                }

                private double CoerceScale(double value) => value;

                private bool ValidateScale(double value) => true;
            }
            """;

        // Act
        var diagnostics = GeneratorTestHelper.GetDiagnostics(source);

        // Assert
        Assert.Empty(diagnostics);
    }
}
