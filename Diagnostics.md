# Diagnostics

| ID | Severity | Description | How to fix |
|---|---|---|---|
| SMU0001 | ❌ Error | `[BindableProperty]` property is not declared as a partial property | Declare the property as `public partial T Name { get; set; }` |
| SMU0002 | ❌ Error | `[BindableProperty]` property is static, and a static property can not be backed by an instance value | Remove `static` from the property, or create the `BindableProperty` by hand |
| SMU0003 | ❌ Error | `[BindableProperty]` property does not have both accessors, or an accessor has its own accessibility modifier such as `private set` | Declare the property as `{ get; set; }` without accessor modifiers |
| SMU0004 | ❌ Error | The type containing the `[BindableProperty]` property, or one of its outer types, is not partial | Add `partial` to the containing type and to every outer type |
| SMU0005 | ❌ Error | The type containing the `[BindableProperty]` property has an explicit base type that is not derived from `BindableObject`, so `GetValue` and `SetValue` are not available. A type with no explicit base type is not checked, because the base type can be declared in another partial declaration such as one generated from XAML | Derive the containing type from `BindableObject` |
| SMU0006 | ❌ Error | The type containing the `[BindableProperty]` property is generic, and a static `BindableProperty` field would be created per type argument | Move the property to a non generic type |
| SMU0007 | ❌ Error | `[BindableProperty]` specifies both `DefaultValue` and `DefaultValueExpression`, and only one default value can be used | Remove either `DefaultValue` or `DefaultValueExpression` |
| SMU0008 | ❌ Error | The method specified for `PropertyChanged`, `PropertyChanging`, `Coerce` or `Validate` of `[BindableProperty]` does not exist in the containing type | Specify the method with `nameof`, and define it in the same type |
| SMU0009 | ❌ Error | The signature of the callback method specified by `[BindableProperty]` does not match, or more than one overload is applicable | Match the signature: `PropertyChanged` and `PropertyChanging` are `void ()` or `void (T oldValue, T newValue)`, `Coerce` is `T (T value)`, `Validate` is `bool (T value)` |
| SMU0010 | ❌ Error | The value specified for `DefaultValue` of `[BindableProperty]` can not be written as a constant in the generated code | Use `DefaultValueExpression` to give the default value as an expression |
