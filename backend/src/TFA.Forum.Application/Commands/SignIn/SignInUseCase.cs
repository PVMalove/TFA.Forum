using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Options;
using TFA.Forum.Application.Abstractions;
using TFA.Forum.Application.Authentication;
using TFA.Forum.Application.Extensions;
using TFA.Forum.Domain.Shared;
using TFA.Forum.Persistence.Storage.User;

namespace TFA.Forum.Application.Commands.SignIn;

public class SignInUseCase : ICommandHandler<SignInResultDto, SignInCommand>
{
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

    public async Task<Result<SignInResultDto, ErrorList>> Execute(
        SignInCommand command, CancellationToken cancellationToken)
    {
        var validateResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validateResult.IsValid)
            return validateResult.ToList();

        var recognisedUser = await storage.FindUser(command.Login, cancellationToken);
        if (recognisedUser is null)
        {
            throw new Exception();
        }

        var passwordMatches = passwordManager.ComparePasswords(
            command.Password, recognisedUser.Salt, recognisedUser.PasswordHash);
        if (!passwordMatches)
        {
            throw new Exception();
        }

        var token = await encryptor.Encrypt(recognisedUser.UserId.ToString(), configuration.Key, cancellationToken);
        var result = new SignInResultDto(new User(recognisedUser.UserId), token);
        return result;
    }
}