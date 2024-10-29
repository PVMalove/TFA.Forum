using TFA.Forum.Domain.Interfaces;

namespace TFA.Forum.Application;

public class GuidFactory : IGuidFactory
{
    public Guid Create() => Guid.NewGuid();
}