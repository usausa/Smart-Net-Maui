namespace Smart.Maui.Generator.Models;

internal sealed record CoerceModel(
    string MethodName,
    bool IsStatic,
    string ParameterType);
