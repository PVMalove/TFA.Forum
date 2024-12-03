namespace TFA.Forum.Application.Authentication;

public interface IIdentityProvider
{
    IIdentity Current { get; set; }
}