namespace Smart.Maui.Generator;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using Smart.Maui.Generator.Models;

using SourceGenerateHelper;

[Generator]
public sealed class BindablePropertyGenerator : IIncrementalGenerator
{
    private const string AttributeName = "Smart.Maui.BindablePropertyAttribute";

    private const string BindableObjectTypeName = "Microsoft.Maui.Controls.BindableObject";

    private const string BindablePropertyTypeName = "global::Microsoft.Maui.Controls.BindableProperty";

    private static readonly SymbolDisplayFormat TypeDisplayFormat = SymbolDisplayFormat.FullyQualifiedFormat
        .WithMiscellaneousOptions(SymbolDisplayFormat.FullyQualifiedFormat.MiscellaneousOptions | SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier);

    // ------------------------------------------------------------
    // Initialize
    // ------------------------------------------------------------

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var propertyProvider = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                AttributeName,
                static (syntax, _) => IsPropertySyntax(syntax),
                static (context, _) => GetPropertyModel(context))
            .Collect();

        context.RegisterSourceOutput(
            propertyProvider,
            static (context, properties) => ReportDiagnostics(context, properties));

        var typeProvider = propertyProvider.SelectMany(static (properties, _) => SelectTypeModel(properties));

        context.RegisterImplementationSourceOutput(
            typeProvider,
            static (context, type) => Execute(context, type));
    }

    private static ImmutableArray<TypeModel> SelectTypeModel(ImmutableArray<Result<PropertyModel>> properties) =>
        [.. properties
            .SelectValue()
            .GroupBy(static x => new { x.Namespace, x.ClassName, x.ContainingTypes })
            .Select(static x => new TypeModel(
                x.Key.Namespace,
                x.Key.ClassName,
                x.Key.ContainingTypes,
                new EquatableArray<PropertyModel>(x)))];

    // ------------------------------------------------------------
    // Parser
    // ------------------------------------------------------------

    private static bool IsPropertySyntax(SyntaxNode syntax) =>
        syntax is PropertyDeclarationSyntax;

    private static Result<PropertyModel> GetPropertyModel(GeneratorAttributeSyntaxContext context)
    {
        var syntax = (PropertyDeclarationSyntax)context.TargetNode;
        if (context.TargetSymbol is not IPropertySymbol symbol)
        {
            return Results.Errors<PropertyModel>();
        }

        var location = syntax.GetLocation();

        // Validate property definition
        if (symbol.IsStatic)
        {
            return Results.Error<PropertyModel>(new DiagnosticInfo(Diagnostics.StaticPropertyNotSupported, location, symbol.Name));
        }

        if (!symbol.IsPartialDefinition || !syntax.Modifiers.Any(static x => x.IsKind(SyntaxKind.PartialKeyword)))
        {
            return Results.Error<PropertyModel>(new DiagnosticInfo(Diagnostics.InvalidPropertyDefinition, location, symbol.Name));
        }

        if ((symbol.GetMethod is null) || (symbol.SetMethod is null) ||
            symbol.SetMethod.IsInitOnly ||
            (symbol.GetMethod.DeclaredAccessibility != symbol.DeclaredAccessibility) ||
            (symbol.SetMethod.DeclaredAccessibility != symbol.DeclaredAccessibility))
        {
            return Results.Error<PropertyModel>(new DiagnosticInfo(Diagnostics.InvalidPropertyAccessor, location, symbol.Name));
        }

        // Validate containing type
        for (var typeSyntax = syntax.Parent as TypeDeclarationSyntax; typeSyntax is not null; typeSyntax = typeSyntax.Parent as TypeDeclarationSyntax)
        {
            if (!typeSyntax.Modifiers.Any(static x => x.IsKind(SyntaxKind.PartialKeyword)))
            {
                return Results.Error<PropertyModel>(new DiagnosticInfo(Diagnostics.ContainingTypeNotPartial, location, symbol.Name));
            }
        }

        var containingType = symbol.ContainingType;
        for (var type = containingType; type is not null; type = type.ContainingType)
        {
            if (type.IsGenericType)
            {
                return Results.Error<PropertyModel>(new DiagnosticInfo(Diagnostics.GenericTypeNotSupported, location, symbol.Name));
            }
        }

        var isBindableObject = false;
        for (var baseType = containingType.BaseType; baseType is not null; baseType = baseType.BaseType)
        {
            if (baseType.ToDisplayString() == BindableObjectTypeName)
            {
                isBindableObject = true;
                break;
            }
        }

        if (!isBindableObject)
        {
            return Results.Error<PropertyModel>(new DiagnosticInfo(Diagnostics.InvalidContainingType, location, symbol.Name));
        }

        // Parse attribute
        var defaultValue = default(TypedConstant?);
        var defaultValueExpression = default(string);
        var defaultBindingMode = default(string);
        var propertyChangedName = default(string);
        var propertyChangingName = default(string);
        var coerceName = default(string);
        var validateName = default(string);
        foreach (var argument in context.Attributes[0].NamedArguments)
        {
            switch (argument.Key)
            {
                case "DefaultValue":
                    defaultValue = argument.Value;
                    break;
                case "DefaultValueExpression":
                    defaultValueExpression = argument.Value.Value as string;
                    break;
                case "DefaultBindingMode":
                    defaultBindingMode = argument.Value.ToCSharpExpression();
                    break;
                case "PropertyChanged":
                    propertyChangedName = argument.Value.Value as string;
                    break;
                case "PropertyChanging":
                    propertyChangingName = argument.Value.Value as string;
                    break;
                case "Coerce":
                    coerceName = argument.Value.Value as string;
                    break;
                case "Validate":
                    validateName = argument.Value.Value as string;
                    break;
            }
        }

        // Default value
        if (defaultValue.HasValue && !String.IsNullOrEmpty(defaultValueExpression))
        {
            return Results.Error<PropertyModel>(new DiagnosticInfo(Diagnostics.DefaultValueConflict, location, symbol.Name));
        }

        var defaultValueLiteral = defaultValueExpression;
        if (defaultValue.HasValue)
        {
            defaultValueLiteral = defaultValue.Value.ToCSharpExpression(symbol.Type);
            if (defaultValueLiteral is null)
            {
                return Results.Error<PropertyModel>(new DiagnosticInfo(Diagnostics.InvalidDefaultValue, location, symbol.Name));
            }
        }

        // Callback
        var propertyChanged = default(PropertyChangedModel);
        if (!String.IsNullOrEmpty(propertyChangedName))
        {
            var (model, error) = ResolvePropertyChanged(context.SemanticModel.Compilation, containingType, propertyChangedName!, symbol.Type, location);
            if (error is not null)
            {
                return Results.Error<PropertyModel>(error);
            }

            propertyChanged = model;
        }

        var propertyChanging = default(PropertyChangedModel);
        if (!String.IsNullOrEmpty(propertyChangingName))
        {
            var (model, error) = ResolvePropertyChanged(context.SemanticModel.Compilation, containingType, propertyChangingName!, symbol.Type, location);
            if (error is not null)
            {
                return Results.Error<PropertyModel>(error);
            }

            propertyChanging = model;
        }

        var coerce = default(CoerceModel);
        if (!String.IsNullOrEmpty(coerceName))
        {
            var (model, error) = ResolveCoerce(context.SemanticModel.Compilation, containingType, coerceName!, symbol.Type, location);
            if (error is not null)
            {
                return Results.Error<PropertyModel>(error);
            }

            coerce = model;
        }

        var validate = default(ValidateModel);
        if (!String.IsNullOrEmpty(validateName))
        {
            var (model, error) = ResolveValidate(context.SemanticModel.Compilation, containingType, validateName!, symbol.Type, location);
            if (error is not null)
            {
                return Results.Error<PropertyModel>(error);
            }

            validate = model;
        }

        // Model
        var ns = String.IsNullOrEmpty(containingType.ContainingNamespace.Name)
            ? string.Empty
            : containingType.ContainingNamespace.ToDisplayString();

        var containingTypes = default(List<ContainingTypeModel>?);
        var containingSymbol = containingType.ContainingType;
        while (containingSymbol is not null)
        {
            containingTypes ??= [];
            containingTypes.Add(new ContainingTypeModel(containingSymbol.GetClassName(), containingSymbol.IsValueType));
            containingSymbol = containingSymbol.ContainingType;
        }

        containingTypes?.Reverse();

        return Results.Success(new PropertyModel(
            ns,
            containingType.GetClassName(),
            new EquatableArray<ContainingTypeModel>(containingTypes ?? []),
            symbol.DeclaredAccessibility,
            symbol.Name,
            symbol.Type.ToDisplayString(TypeDisplayFormat),
            symbol.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            symbol.Type.SpecialType != SpecialType.System_Object,
            defaultValueLiteral,
            defaultBindingMode,
            propertyChanged,
            propertyChanging,
            coerce,
            validate));
    }

    private static (PropertyChangedModel? Model, DiagnosticInfo? Error) ResolvePropertyChanged(Compilation compilation, INamedTypeSymbol containingType, string methodName, ITypeSymbol propertyType, Location location)
    {
        var found = false;
        var candidates = new List<PropertyChangedModel>();
        foreach (var method in EnumerateCallbackMethods(compilation, containingType, methodName))
        {
            found = true;

            if (method.IsStatic || method.IsGenericMethod || !method.ReturnsVoid)
            {
                continue;
            }

            if (method.Parameters.Length == 0)
            {
                candidates.Add(new PropertyChangedModel(methodName, false, string.Empty, string.Empty));
            }
            else if ((method.Parameters.Length == 2) &&
                     SymbolEqualityComparer.Default.Equals(method.Parameters[0].Type, propertyType) &&
                     SymbolEqualityComparer.Default.Equals(method.Parameters[1].Type, propertyType))
            {
                candidates.Add(new PropertyChangedModel(
                    methodName,
                    true,
                    method.Parameters[0].Type.ToDisplayString(TypeDisplayFormat),
                    method.Parameters[1].Type.ToDisplayString(TypeDisplayFormat)));
            }
        }

        if (candidates.Count == 1)
        {
            return (candidates[0], null);
        }

        return found
            ? (null, new DiagnosticInfo(Diagnostics.InvalidCallbackMethod, location, methodName))
            : (null, new DiagnosticInfo(Diagnostics.CallbackMethodNotFound, location, methodName));
    }

    private static (CoerceModel? Model, DiagnosticInfo? Error) ResolveCoerce(Compilation compilation, INamedTypeSymbol containingType, string methodName, ITypeSymbol propertyType, Location location)
    {
        var found = false;
        var candidates = new List<CoerceModel>();
        foreach (var method in EnumerateCallbackMethods(compilation, containingType, methodName))
        {
            found = true;

            if (method.IsGenericMethod ||
                (method.Parameters.Length != 1) ||
                !SymbolEqualityComparer.Default.Equals(method.Parameters[0].Type, propertyType) ||
                !SymbolEqualityComparer.Default.Equals(method.ReturnType, propertyType))
            {
                continue;
            }

            candidates.Add(new CoerceModel(methodName, method.IsStatic, method.Parameters[0].Type.ToDisplayString(TypeDisplayFormat)));
        }

        if (candidates.Count == 1)
        {
            return (candidates[0], null);
        }

        return found
            ? (null, new DiagnosticInfo(Diagnostics.InvalidCallbackMethod, location, methodName))
            : (null, new DiagnosticInfo(Diagnostics.CallbackMethodNotFound, location, methodName));
    }

    private static (ValidateModel? Model, DiagnosticInfo? Error) ResolveValidate(Compilation compilation, INamedTypeSymbol containingType, string methodName, ITypeSymbol propertyType, Location location)
    {
        var found = false;
        var candidates = new List<ValidateModel>();
        foreach (var method in EnumerateCallbackMethods(compilation, containingType, methodName))
        {
            found = true;

            if (method.IsGenericMethod ||
                (method.Parameters.Length != 1) ||
                !SymbolEqualityComparer.Default.Equals(method.Parameters[0].Type, propertyType) ||
                (method.ReturnType.SpecialType != SpecialType.System_Boolean))
            {
                continue;
            }

            candidates.Add(new ValidateModel(methodName, method.IsStatic, method.Parameters[0].Type.ToDisplayString(TypeDisplayFormat)));
        }

        if (candidates.Count == 1)
        {
            return (candidates[0], null);
        }

        return found
            ? (null, new DiagnosticInfo(Diagnostics.InvalidCallbackMethod, location, methodName))
            : (null, new DiagnosticInfo(Diagnostics.CallbackMethodNotFound, location, methodName));
    }

    private static IEnumerable<IMethodSymbol> EnumerateCallbackMethods(Compilation compilation, INamedTypeSymbol containingType, string methodName)
    {
        for (var type = containingType; type is not null; type = type.BaseType)
        {
            var declared = false;
            foreach (var method in type.GetMembers(methodName).OfType<IMethodSymbol>())
            {
                if (!compilation.IsSymbolAccessibleWithin(method, containingType))
                {
                    continue;
                }

                declared = true;
                yield return method;
            }

            if (declared)
            {
                yield break;
            }
        }
    }

    // ------------------------------------------------------------
    // Generator
    // ------------------------------------------------------------

    private static void ReportDiagnostics(SourceProductionContext context, ImmutableArray<Result<PropertyModel>> properties)
    {
        foreach (var info in properties.SelectError())
        {
            context.ReportDiagnostic(info);
        }
    }

    private static void Execute(SourceProductionContext context, TypeModel type)
    {
        context.CancellationToken.ThrowIfCancellationRequested();

        var builder = new SourceBuilder();
        BuildSource(builder, type);

        context.AddSource(
            HintNameBuilder.Build(type.Namespace, [.. type.ContainingTypes.Select(static x => x.ClassName), type.ClassName]),
            builder);
    }

    private static void BuildSource(SourceBuilder builder, TypeModel type)
    {
        var ns = type.Namespace;
        var containingTypes = type.ContainingTypes;
        var className = type.ClassName;

        builder.AutoGenerated();
        builder.EnableNullable();
        builder.NewLine();

        // namespace
        if (!String.IsNullOrEmpty(ns))
        {
            builder.Namespace(ns);
            builder.NewLine();
        }

        // containing types
        foreach (var containingType in containingTypes)
        {
            builder
                .Indent()
                .Append("partial ")
                .Append(containingType.IsValueType ? "struct " : "class ")
                .Append(containingType.ClassName)
                .NewLine();
            builder.BeginScope();
        }

        // class
        builder
            .Indent()
            .Append("partial class ")
            .Append(className)
            .NewLine();
        builder.BeginScope();

        var first = true;
        foreach (var property in type.Properties)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                builder.NewLine();
            }

            BuildProperty(builder, className, property);
        }

        builder.EndScope();

        // end containing types
        for (var i = 0; i < containingTypes.Count; i++)
        {
            builder.EndScope();
        }
    }

    private static void BuildProperty(SourceBuilder builder, string className, PropertyModel property)
    {
        var accessibility = property.PropertyAccessibility.ToText();

        // field
        builder
            .Indent()
            .Append(accessibility)
            .Append(" static readonly ")
            .Append(BindablePropertyTypeName)
            .Append(" ")
            .Append(property.PropertyName)
            .Append("Property = ")
            .Append(BindablePropertyTypeName)
            .Append(".Create(")
            .NewLine();
        builder.Indent().Append("    nameof(").Append(property.PropertyName).Append("),").NewLine();
        builder.Indent().Append("    typeof(").Append(property.TypeofType).Append("),").NewLine();
        builder.Indent().Append("    typeof(").Append(className).Append(")");

        foreach (var argument in MakeOptionArguments(className, property))
        {
            builder.Append(",").NewLine();
            builder.Indent().Append("    ").Append(argument);
        }

        builder.Append(");").NewLine();
        builder.NewLine();

        // property
        builder
            .Indent()
            .Append(accessibility)
            .Append(" partial ")
            .Append(property.PropertyType)
            .Append(" ")
            .Append(property.PropertyName)
            .NewLine();
        builder.BeginScope();
        builder.Indent().Append("get => ");
        if (property.RequireCast)
        {
            builder.Append("(").Append(property.PropertyType).Append(")");
        }

        builder.Append("GetValue(").Append(property.PropertyName).Append("Property);").NewLine();
        builder.Indent().Append("set => SetValue(").Append(property.PropertyName).Append("Property, value);").NewLine();
        builder.EndScope();
    }

    private static List<string> MakeOptionArguments(string className, PropertyModel property)
    {
        var arguments = new List<string>();

        if (property.DefaultValue is not null)
        {
            arguments.Add($"defaultValue: {property.DefaultValue}");
        }

        if (property.DefaultBindingMode is not null)
        {
            arguments.Add($"defaultBindingMode: {property.DefaultBindingMode}");
        }

        if (property.Validate is not null)
        {
            arguments.Add($"validateValue: {MakeValidateCallback(className, property.Validate)}");
        }

        if (property.PropertyChanged is not null)
        {
            arguments.Add($"propertyChanged: {MakeChangedCallback(className, property.PropertyChanged)}");
        }

        if (property.PropertyChanging is not null)
        {
            arguments.Add($"propertyChanging: {MakeChangedCallback(className, property.PropertyChanging)}");
        }

        if (property.Coerce is not null)
        {
            arguments.Add($"coerceValue: {MakeCoerceCallback(className, property.Coerce)}");
        }

        return arguments;
    }

    private static string MakeChangedCallback(string className, PropertyChangedModel changed) =>
        changed.HasParameters
            ? $"static (bindable, oldValue, newValue) => (({className})bindable).{changed.MethodName}(({changed.OldParameterType})oldValue, ({changed.NewParameterType})newValue)"
            : $"static (bindable, oldValue, newValue) => (({className})bindable).{changed.MethodName}()";

    private static string MakeCoerceCallback(string className, CoerceModel coerce) =>
        coerce.IsStatic
            ? $"static (bindable, value) => {coerce.MethodName}(({coerce.ParameterType})value)"
            : $"static (bindable, value) => (({className})bindable).{coerce.MethodName}(({coerce.ParameterType})value)";

    private static string MakeValidateCallback(string className, ValidateModel validate) =>
        validate.IsStatic
            ? $"static (bindable, value) => {validate.MethodName}(({validate.ParameterType})value)"
            : $"static (bindable, value) => (({className})bindable).{validate.MethodName}(({validate.ParameterType})value)";
}
