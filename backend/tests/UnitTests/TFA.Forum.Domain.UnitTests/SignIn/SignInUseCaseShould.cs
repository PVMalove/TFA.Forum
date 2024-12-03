using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Language.Flow;
using TFA.Forum.Application.Authentication;
using TFA.Forum.Application.Commands.SignIn;
using TFA.Forum.Domain.DTO.User;
using TFA.Forum.Domain.Shared;
using TFA.Forum.Persistence.Storage.User;

namespace TFA.Forum.Domain.UnitTests.SignIn;

public class SignInUseCaseShould
{
    private readonly SignInUseCase sut;
    private readonly ISetup<ISignInStorage, Task<ExistsUserDto?>> findUserSetup;
    private readonly ISetup<IPasswordManager, bool> comparePasswordsSetup;
    private readonly ISetup<ISymmetricEncryptor, Task<string>> encryptSetup;
    private readonly ISetup<ISignInStorage,Task<Guid>> createSessionSetup;
    private readonly Mock<ISignInStorage> storage;
    private readonly Mock<ISymmetricEncryptor> encryptor;

    public SignInUseCaseShould()
    {
        var validator = new Mock<IValidator<SignInCommand>>();
        validator
            .Setup(v => v.ValidateAsync(It.IsAny<SignInCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        storage = new Mock<ISignInStorage>();
        findUserSetup = storage.Setup(s => s.FindUserByLogin(It.IsAny<string>(), It.IsAny<CancellationToken>()));
        createSessionSetup = storage.Setup(s => s.CreateSession(It.IsAny<Guid>(), It.IsAny<DateTimeOffset>(), It.IsAny<CancellationToken>()));

        var passwordManager = new Mock<IPasswordManager>();
        comparePasswordsSetup = passwordManager.Setup(m => m.ComparePasswords(
            It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()));

        encryptor = new Mock<ISymmetricEncryptor>();
        encryptSetup = encryptor.Setup(e =>
            e.Encrypt(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()));

        var configuration = new Mock<IOptions<AuthenticationConfiguration>>();
        configuration
            .Setup(c => c.Value)
            .Returns(new AuthenticationConfiguration
            {
                Base64Key = "fOpY0gOMaErVcls0knIXrp6vrCR60I3zlJOLjIoOCmQ="
            });

        sut = new SignInUseCase(
            validator.Object,
            storage.Object,
            passwordManager.Object,
            encryptor.Object,
            configuration.Object);
    }

    [Fact]
    public async Task InvalidCredentials_WhenUserNotFound()
    {
        var command = new SignInCommand("Test", "qwerty", false);
        var cancellationToken = CancellationToken.None;

        findUserSetup.ReturnsAsync(() => null);

        var result = await sut.Execute(command, cancellationToken);

        result.Error.Should().NotBeNull();
        result.Error.Should().ContainSingle(e => e.Message == Errors.User.InvalidCredentials().Message);
    }

    [Fact]
    public async Task InvalidCredentials_WhenPasswordDoesntMatch()
    {
        var userId = Guid.Parse("38C95601-DA60-43DA-811B-2E34D0555230");
        findUserSetup.ReturnsAsync(new ExistsUserDto(userId, [1], [2]));
        comparePasswordsSetup.Returns(false);

        var result = await sut.Execute(new SignInCommand("Test", "qwerty", false), CancellationToken.None);

        result.Error.Should().NotBeNull();
        result.Error.Should().ContainSingle(e => e.Message == Errors.User.InvalidCredentials().Message);
    }

    [Fact]
    public async Task CreateSession_WhenPasswordMatches()
    {
        var userId = Guid.Parse("38C95601-DA60-43DA-811B-2E34D0555230");
        var sessionId = Guid.Parse("8D8C2920-CE80-47FC-A589-562FE777F7BA");
        findUserSetup.ReturnsAsync(new ExistsUserDto(userId, It.IsAny<byte[]>(), It.IsAny<byte[]>()));
        comparePasswordsSetup.Returns(true);
        createSessionSetup.ReturnsAsync(sessionId);

        await sut.Execute(new SignInCommand("Test", "qwerty", false), CancellationToken.None);
        storage.Verify(s => s.CreateSession(userId, It.IsAny<DateTimeOffset>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ReturnTokenAndIdentity()
    {
        var userId = Guid.Parse("38C95601-DA60-43DA-811B-2E34D0555230");
        var sessionId = Guid.Parse("8D8C2920-CE80-47FC-A589-562FE777F7BA");
        findUserSetup.ReturnsAsync(new ExistsUserDto(userId, It.IsAny<byte[]>(), It.IsAny<byte[]>()));
        comparePasswordsSetup.Returns(true);
        createSessionSetup.ReturnsAsync(sessionId);
        encryptSetup.ReturnsAsync("token");
    
        var result = await sut.Execute(new SignInCommand("Test", "qwerty", false), CancellationToken.None);
        result.Value.Token.Should().NotBeEmpty();
        result.Value.Identity.UserId.Should().Be(userId);
        result.Value.Identity.SessionId.Should().Be(sessionId);
        result.Value.Token.Should().Be("token");
    }

    [Fact]
    public async Task EncryptSessionIdIntoToken()
    {
        var userId = Guid.Parse("38C95601-DA60-43DA-811B-2E34D0555230");
        var sessionId = Guid.Parse("8D8C2920-CE80-47FC-A589-562FE777F7BA");
        findUserSetup.ReturnsAsync(new ExistsUserDto(userId, It.IsAny<byte[]>(), It.IsAny<byte[]>()));
        comparePasswordsSetup.Returns(true);
        createSessionSetup.ReturnsAsync(sessionId);

        await sut.Execute(new SignInCommand("Test", "qwerty", false), CancellationToken.None);

        encryptor.Verify(s => s
            .Encrypt(sessionId.ToString(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()));
    }
}