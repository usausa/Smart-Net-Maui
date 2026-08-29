namespace Smart.Maui.Generator.Models;

internal sealed record ValidateModel(
    string MethodName,
    bool IsStatic,
    string ParameterType);
