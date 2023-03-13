namespace Hippocampus.Domain.Services.ApplicationValues;

public class ServiceResult<TResponse>
{
    public TResponse? Result { get; }
    public string Message { get; } = string.Empty;
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;


    private ServiceResult(TResponse result)
    {
        IsSuccess = true;
        Result = result;
    }

    private ServiceResult(string message)
    {
        IsSuccess = false;
        Message = message;
    }

    public static ServiceResult<TResponse> Success(TResponse result) => new(result);
    public static ServiceResult<TResponse> Error(string message) => new(message);
}