namespace TFA.Forum.Domain;

public class GuidFactory : IGuidFactory
{
    public Guid Create() => Guid.NewGuid();
}