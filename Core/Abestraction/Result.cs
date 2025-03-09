namespace Core.Abestraction;
public class Result
{
    public Result(bool isSuccess, Error error)
    {
        if ((isSuccess && error != Error.None) || (!isSuccess && error == Error.None))
            new InvalidOperationException();
        IsSuccess = isSuccess;
        Error = error;

    }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; set; } = default!;

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
};
public class Result<TValue>(TValue tValue, bool IsSuccess, Error error) : Result(IsSuccess, error)
{

    public TValue Value { get; set; } = tValue;
    public static Result<TValue> Success(TValue tValue) => new(tValue, true, Error.None);
    public static Result<TValue> Failure(Error error) => new(default!, false, error);

}



