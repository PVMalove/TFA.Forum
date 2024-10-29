using TFA.Forum.Domain.Interfaces;

namespace TFA.Forum.Application;

public class MomentProvider : IMomentProvider
{
    public DateTimeOffset Now => DateTimeOffset.UtcNow;
}