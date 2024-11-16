using TFA.Forum.Domain.Enum;

namespace TFA.Forum.Domain.Result;


public class CollectionResult<T>(string errorMessage = null, ErrorCodes? errorCode = null)
    : BaseResult<IEnumerable<T>>(errorMessage, errorCode)
{
    public int Count { get; }

    public CollectionResult(IEnumerable<T> data, int count)
        : this()
    {
        Data = data;
        Count = count;
    }
}