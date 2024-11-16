using TFA.Forum.Domain.Enum;

namespace TFA.Forum.Domain.Result;


public class BaseResult(string errorMessage = null, ErrorCodes? errorCode = null)
{
    public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
    public string ErrorMessage { get; init; } = errorMessage;
    public ErrorCodes? ErrorCode { get; init; } = errorCode;

    public BaseResult() : this(null, null) { }
}

public class BaseResult<T>(string errorMessage = null, ErrorCodes? errorCode = null)
    : BaseResult(errorMessage, errorCode)
{
    public T Data { get; init; }

    public BaseResult(T data)
        : this()
    {
        Data = data;
    }
}