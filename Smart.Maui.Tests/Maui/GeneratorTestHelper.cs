namespace Smart.Maui;

using System.Collections.Generic;

using Microsoft.CodeAnalysis;

using Smart.Maui.Generator;

using SourceGenerateHelper.Testing;

internal static class GeneratorTestHelper
{
    private static GeneratorTestRunner Runner => GeneratorTestRunner
        .For<BindablePropertyGenerator>()
        .WithReference(typeof(BindablePropertyAttribute).Assembly)
        .WithReference(typeof(BindableObject).Assembly)
        .WithReference(typeof(BindingMode).Assembly)
        .WithDiagnosticPrefix("SMU")
        .VerifyCompiles();

    public static IReadOnlyList<Diagnostic> GetDiagnostics(string source) => Runner.GetDiagnostics(source);

    public static IReadOnlyList<Diagnostic> GetDiagnosticsAll(string source) => Runner.GetDiagnosticsAll(source);

    // Used when the generated code can not compile by design, such as a type with no known base type
    public static IReadOnlyList<Diagnostic> GetDiagnosticsWithoutVerify(string source) =>
        Runner.VerifyCompiles(false).GetDiagnostics(source);

    public static string GetGeneratedSource(string source) => Runner.GetGeneratedSource(source);

    public static IncrementalRunResult RunIncremental(string source, string addedSource) =>
        Runner.WithTracking().RunIncremental(source, addedSource);
}
