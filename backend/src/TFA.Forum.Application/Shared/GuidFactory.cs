using TFA.Forum.Domain.Interfaces;

namespace TFA.Forum.Application.Shared;

public class GuidFactory : IGuidFactory
{
    public Guid Create() => Guid.NewGuid();
}