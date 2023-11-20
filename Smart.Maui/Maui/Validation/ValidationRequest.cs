namespace Smart.Maui.Validation;

public sealed class ValidationRequest
{
    public event EventHandler<EventArgs>? ValidationErrorRequested;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1030:UseEventsWhereAppropriate", Justification = "Ignore.")]
    public void RaiseValidationError()
    {
        ValidationErrorRequested?.Invoke(this, EventArgs.Empty);
    }
}
