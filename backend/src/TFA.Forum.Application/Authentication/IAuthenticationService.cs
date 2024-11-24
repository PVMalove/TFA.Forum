namespace TFA.Forum.Application.Authentication;

public interface IAuthenticationService
{
    Task<IIdentity> Authenticate(string authToken, CancellationToken cancellationToken);
}