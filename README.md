# Smart.Maui .NET - MVVM helper library for MAUI

[![NuGet](https://img.shields.io/nuget/v/Usa.Smart.Maui.svg)](https://www.nuget.org/packages/Usa.Smart.Maui/)

## Features

* Basic converters.
* Observable commands.
* Actions, Behaviors and Triggers.
* Markup extensions.
* Messenger.
* Resolver(DI Container) integration.
* Base class for ViewModel.
* BindableProperty source generator.

## BindableProperty generator

Add `[BindableProperty]` to a partial property, and the `BindableProperty` field and the property implementation are generated.

```csharp
public partial class GaugeView : View
{
    [BindableProperty(DefaultValue = 0d, PropertyChanged = nameof(OnLevelChanged), Coerce = nameof(CoerceLevel))]
    public partial double Level { get; set; }

    [BindableProperty(DefaultBindingMode = BindingMode.TwoWay)]
    public partial string? Label { get; set; }

    private void OnLevelChanged(double oldValue, double newValue) { }

    private double CoerceLevel(double value) => Math.Clamp(value, 0d, 100d);
}
```

| Option | Note |
|-|-|
| `DefaultValue` | Default value of the property |
| `DefaultValueExpression` | Default value as an expression, for values that can not be written as a constant |
| `DefaultBindingMode` | `BindingMode` |
| `PropertyChanged` | Name of a `void` method with no parameters, or with `(T oldValue, T newValue)` |
| `PropertyChanging` | Name of a `void` method with no parameters, or with `(T oldValue, T newValue)` |
| `Coerce` | Name of a `T` method with `(T value)` |
| `Validate` | Name of a `bool` method with `(T value)` |

Requires C# 13 or later, because partial properties are used.

## NuGet

| Package | Note  |
|-|-|
| [![NuGet](https://img.shields.io/nuget/v/Usa.Smart.Maui.svg)](https://www.nuget.org/packages/Usa.Smart.Maui/) | Core libyrary |
| [![NuGet](https://img.shields.io/nuget/v/Usa.Smart.Maui.Extensions.svg)](https://www.nuget.org/packages/Usa.Smart.Maui.Extensions/) | Extension helpers |

## Link

* [Smart.Mvvm](https://github.com/usausa/Smart-Net-Mvvm)
* [Smart.Resolver](https://github.com/usausa/Smart-Net-Resolver)
* [Smart.Navigation](https://github.com/usausa/Smart-Net-Navigation)
