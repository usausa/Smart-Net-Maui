namespace Smart.Maui.Validation;

using System.Collections.ObjectModel;

using Smart.ComponentModel;

#pragma warning disable IDE0032
public sealed class ValidationValue<T> : NotificationValue<T>, IValidatable, IValidationResult
{
    private bool hasError;

#pragma warning disable CA1002
    public List<IValidator<T>> Validators { get; } = [];
#pragma warning restore CA1002

    public ObservableCollection<string> Errors { get; } = [];

    public bool HasError
    {
        get => hasError;
        set
        {
            Errors.Clear();
            SetProperty(ref hasError, value);
        }
    }

    public ValidationValue()
    {
    }

    public ValidationValue(T value)
        : base(value)
    {
    }

    public bool Validate()
    {
        Errors.Clear();

        foreach (var message in Validators.Where(x => !x.Validate(Value)).Select(static x => x.ErrorMessage))
        {
            Errors.Add(message);
        }

        HasError = Errors.Count > 0;

        return !HasError;
    }
}
