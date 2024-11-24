using TFA.Forum.Application.Authentication;

namespace TFA.Forum.Application.Commands.SignIn;

public record SignInResultDto(IIdentity Identity, string Token);