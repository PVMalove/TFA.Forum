namespace TFA.Forum.Application.Authentication;

public interface IIdentity
{
    Guid UserId { get; }
}

public class User(Guid userId) : IIdentity
{
    public Guid UserId { get; } = userId;
    public static User Guest => new(Guid.Empty);
}

public static class IdentityExtensions
{
    public static bool IsAuthenticated(this IIdentity identity) => identity.UserId != Guid.Empty;
}
