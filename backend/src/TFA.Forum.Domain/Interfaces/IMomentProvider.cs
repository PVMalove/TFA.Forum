namespace TFA.Forum.Domain.Interfaces;

public interface IMomentProvider
{
    DateTimeOffset Now { get; }
}