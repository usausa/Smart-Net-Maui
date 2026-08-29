namespace Smart.Maui;

using System;

using Microsoft.Maui.Controls;

[AttributeUsage(AttributeTargets.Property)]
public sealed class BindablePropertyAttribute : Attribute
{
    public object? DefaultValue { get; set; }

    public string? DefaultValueExpression { get; set; }

    public BindingMode DefaultBindingMode { get; set; }

    public string? PropertyChanged { get; set; }

    public string? PropertyChanging { get; set; }

    public string? Coerce { get; set; }

    public string? Validate { get; set; }
}
