namespace Smart.Maui.Validation;

public interface IValidator<in T>
{
    string ErrorMessage { get; }

    bool Validate(T value);
}
