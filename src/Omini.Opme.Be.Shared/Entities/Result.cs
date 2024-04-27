namespace Omini.Opme.Be.Shared.Entities;

public readonly struct Result<TValue, TError>
{
    private readonly TValue? _value;
    private readonly TError? _error;

    public TValue Response { get { return _value! ;} }

    private Result(TValue value)
    {
        IsError = false;
        _value = value;
        _error = default;
    }

    private Result(TError error)
    {
        IsError = true;
        _value = default;
        _error = error;
    }

    public bool IsError { get; }

    public bool IsSuccess => !IsError;

    public static implicit operator Result<TValue, TError>(TValue value) => new(value);
    public static implicit operator Result<TValue, TError>(TError value) => new(value);

    public TResult Match<TResult>(
        Func<TValue, TResult> success,
        Func<TError, TResult> failure) =>
            !IsError ? success(_value!) : failure(_error!);
}

public readonly struct Result<TValue>
{
    private readonly TValue? _value;

    public TValue Response { get { return _value! ;} }

    private Result(TValue value)
    {
        IsError = false;
        _value = value;
    }

    public bool IsError { get; }

    public bool IsSuccess => !IsError;

    public static implicit operator Result<TValue>(TValue value) => new(value);
}