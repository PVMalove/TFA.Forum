using TFA.Forum.Domain.Interfaces;

namespace TFA.Forum.Persistence.Shared;

public class GuidFactory : IGuidFactory
{
    public Guid Create() => Guid.NewGuid();
}