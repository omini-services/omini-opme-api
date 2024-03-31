using FluentValidation.Results;

public record ValidationFailed(IEnumerable<ValidationFailure> errors)
{
    public ValidationFailed(ValidationFailure error) : this(new[] { error })
    {

    }
}