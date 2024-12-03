namespace TFA.Forum.Application.Authentication;

public interface IIdentity
{
    Guid UserId { get; }
    Guid SessionId { get; }
}

public static class IdentityExtensions
{
    public static bool IsAuthenticated(this IIdentity identity) => identity.UserId != Guid.Empty;
}
