using TFA.Forum.Application.Authentication;

namespace TFA.Forum.Application.Commands.SignIn;

public record SignInResponse(IIdentity Identity, string Token);