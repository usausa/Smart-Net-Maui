namespace Smart.Maui.Generator;

using Microsoft.CodeAnalysis;

internal static class Diagnostics
{
    public static DiagnosticDescriptor InvalidPropertyDefinition { get; } = new(
        id: "SMU0001",
        title: "Invalid property definition",
        messageFormat: "[BindableProperty] property must be partial. property=[{0}]",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static DiagnosticDescriptor StaticPropertyNotSupported { get; } = new(
        id: "SMU0002",
        title: "Static property not supported",
        messageFormat: "[BindableProperty] static property is not supported. property=[{0}]",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static DiagnosticDescriptor InvalidPropertyAccessor { get; } = new(
        id: "SMU0003",
        title: "Invalid property accessor",
        messageFormat: "[BindableProperty] property must have get/set without modifiers. property=[{0}]",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static DiagnosticDescriptor ContainingTypeNotPartial { get; } = new(
        id: "SMU0004",
        title: "Containing type not partial",
        messageFormat: "[BindableProperty] containing type must be partial. property=[{0}]",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static DiagnosticDescriptor InvalidContainingType { get; } = new(
        id: "SMU0005",
        title: "Invalid containing type",
        messageFormat: "[BindableProperty] containing type is not BindableObject. property=[{0}]",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static DiagnosticDescriptor GenericTypeNotSupported { get; } = new(
        id: "SMU0006",
        title: "Generic type not supported",
        messageFormat: "[BindableProperty] generic containing type is not supported. property=[{0}]",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static DiagnosticDescriptor DefaultValueConflict { get; } = new(
        id: "SMU0007",
        title: "DefaultValue conflict",
        messageFormat: "[BindableProperty] DefaultValue and DefaultValueExpression conflict. property=[{0}]",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static DiagnosticDescriptor CallbackMethodNotFound { get; } = new(
        id: "SMU0008",
        title: "Callback method not found",
        messageFormat: "[BindableProperty] callback method is not found. method=[{0}]",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static DiagnosticDescriptor InvalidCallbackMethod { get; } = new(
        id: "SMU0009",
        title: "Invalid callback method",
        messageFormat: "[BindableProperty] callback method signature is invalid. method=[{0}]",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);

    public static DiagnosticDescriptor InvalidDefaultValue { get; } = new(
        id: "SMU0010",
        title: "Invalid default value",
        messageFormat: "[BindableProperty] DefaultValue is not a supported constant. property=[{0}]",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
}
