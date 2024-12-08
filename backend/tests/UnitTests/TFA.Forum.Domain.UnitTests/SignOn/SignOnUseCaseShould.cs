using Moq;
using Moq.Language.Flow;
using TFA.Forum.Application.Authentication;
using TFA.Forum.Application.Commands.SignOn;
using TFA.Forum.Persistence.Storage.User;

namespace TFA.Forum.Domain.UnitTests.SignOn;

public class SignOnUseCaseShould
{
    private readonly SignOnUseCase sut;
    private readonly ISetup<IPasswordManager,(byte[] Salt, byte[] Hash)> generatePasswordPartsSetup;
    private readonly ISetup<ISignOnStorage,Task<Guid>> createUserSetup;
    private readonly Mock<ISignOnStorage> storage;

    public SignOnUseCaseShould()
    {
        var passwordManager = new Mock<IPasswordManager>();
        generatePasswordPartsSetup = passwordManager.Setup(m => m.GeneratePasswordParts(It.IsAny<string>()));

        storage = new Mock<ISignOnStorage>();
        createUserSetup = storage.Setup(s =>
            s.CreateUser(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()));

        sut = new SignOnUseCase(passwordManager.Object, storage.Object);
    }

    [Fact]
    public async Task CreateUser_WithGeneratedPasswordParts()
    {
        var salt = new byte[] { 1 };
        var hash = new byte[] { 2 };
        generatePasswordPartsSetup.Returns((Salt: salt, Hash: hash));

        await sut.Handle(new SignOnCommand("Test", "qwerty"), CancellationToken.None);
        
        storage.Verify(s => s.CreateUser("Test", salt, hash, It.IsAny<CancellationToken>()), Times.Once);
        storage.VerifyNoOtherCalls();
    }
}