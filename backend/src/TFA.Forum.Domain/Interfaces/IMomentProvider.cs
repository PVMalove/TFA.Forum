namespace TFA.Forum.Domain;

public interface IMomentProvider
{
    DateTimeOffset Now { get; }
}