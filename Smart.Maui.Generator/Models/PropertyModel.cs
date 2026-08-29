namespace Smart.Maui.Generator.Models;

using Microsoft.CodeAnalysis;

using SourceGenerateHelper;

internal sealed record PropertyModel(
    // Containing type
    string Namespace,
    string ClassName,
    EquatableArray<ContainingTypeModel> ContainingTypes,
    // Property signature
    Accessibility PropertyAccessibility,
    string PropertyName,
    string PropertyType,
    string TypeofType,
    bool RequireCast,
    // Metadata
    string? DefaultValue,
    string? DefaultBindingMode,
    // Callback
    PropertyChangedModel? PropertyChanged,
    PropertyChangedModel? PropertyChanging,
    CoerceModel? Coerce,
    ValidateModel? Validate);
