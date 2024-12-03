using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Options;
using TFA.Forum.Application.Abstractions;
using TFA.Forum.Application.Authentication;
using TFA.Forum.Application.Extensions;
using TFA.Forum.Domain.Shared;
using TFA.Forum.Persistence.Storage.User;

namespace TFA.Forum.Application.Commands.SignIn;

public class SignInUseCase : ICommandHandler<SignInResponse, SignInCommand>
{
    private readonly TimeSpan defaultSessionLifetime = TimeSpan.FromHours(1);
    private readonly TimeSpan rememberMeSessionLifetime = TimeSpan.FromDays(30);
    
    private readonly IValidator<SignInCommand> validator;
    private readonly ISignInStorage storage;
    private readonly IPasswordManager passwordManager;
    private readonly ISymmetricEncryptor encryptor;
    private readonly AuthenticationConfiguration configuration;

    public SignInUseCase(
        IValidator<SignInCommand> validator,
        ISignInStorage storage,
        IPasswordManager passwordManager,
        ISymmetricEncryptor encryptor,
        IOptions<AuthenticationConfiguration> options)
    {
        this.validator = validator;
        this.storage = storage;
        this.passwordManager = passwordManager;
        this.encryptor = encryptor;
        configuration = options.Value;
    }

    public async Task<Result<SignInResponse, ErrorList>> Execute(
        SignInCommand command, CancellationToken cancellationToken)
    {
        var validateResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validateResult.IsValid)
            return validateResult.ToList();

        var existsUser = await storage.FindUserByLogin(command.Login, cancellationToken);
        if (existsUser is null)
            return Errors.User.InvalidCredentials().ToErrorList();
        
        var passwordCorrect = passwordManager.ComparePasswords(
            command.Password, existsUser.Salt, existsUser.PasswordHash);
        if (!passwordCorrect)
            return Errors.User.InvalidCredentials().ToErrorList();
        
        var sessionExpiry = DateTimeOffset.UtcNow + (command.RememberMe ? rememberMeSessionLifetime : defaultSessionLifetime);
        
        var sessionId = await storage.CreateSession(existsUser.UserId, sessionExpiry, cancellationToken);
        
        var token = await encryptor.Encrypt(sessionId.ToString(), configuration.Key, cancellationToken);
        var identity = new User(existsUser.UserId, sessionId);
        var result = new SignInResponse(identity, token);
        return result;
    }
}