namespace TFA.Forum.Application.Authentication;

public class IdentityProvider : IIdentityProvider
{
    public IIdentity Current => new User(Guid.Parse("33bf86a8-8cb0-4829-88da-4766f8bf4a62"));
}