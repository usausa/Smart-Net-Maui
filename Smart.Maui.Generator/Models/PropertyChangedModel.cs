namespace Smart.Maui.Generator.Models;

internal sealed record PropertyChangedModel(
    string MethodName,
    bool HasParameters,
    string OldParameterType,
    string NewParameterType);
