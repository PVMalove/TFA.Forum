using TFA.Forum.Domain.Interfaces;

namespace TFA.Forum.Domain.EntityIds;

public class GuidFactory : IGuidFactory
{
    public Guid Create() => Guid.NewGuid();
}