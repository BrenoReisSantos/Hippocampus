namespace Hippocampus.Domain.Services.ApplicationValues;

public class ServiceResult
{
    public string? Message { get; protected set; }

    public bool IsSuccess { get; protected set; }

    public bool IsFailure => !IsSuccess;

    protected ServiceResult(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }

    protected ServiceResult(string message, bool isSuccess)
    {
        Message = message;
        IsSuccess = isSuccess;
    }

    public static ServiceResult Success()
    {
        return new ServiceResult(true);
    }

    public static ServiceResult Error(string message)
    {
        return new ServiceResult(message, false);
    }
}

public class ServiceResult<TResult> : ServiceResult
{
    public TResult? Result { get; private set; }

    private ServiceResult(TResult result)
        : base(true)
    {
        Result = result;
    }

    private ServiceResult(string message, bool isSuccess)
        : base(message, isSuccess) { }

    public static ServiceResult<TResult> Success(TResult result) =>
        new ServiceResult<TResult>(result);

    public static new ServiceResult<TResult> Error(string message)
    {
        return new ServiceResult<TResult>(message, false);
    }
}
