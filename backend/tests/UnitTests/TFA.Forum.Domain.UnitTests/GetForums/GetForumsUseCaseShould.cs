using CSharpFunctionalExtensions;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using TFA.Forum.Application.Queries.GetAllForums;
using TFA.Forum.Domain.DTO.Forum;
using TFA.Forum.Domain.Shared;
using TFA.Forum.Persistence.Storage.Forum;

namespace TFA.Forum.Domain.UnitTests.GetForums;

public class GetForumsUseCaseShould
{
    private readonly GetAllForumsUseCase sut;
    private readonly ISetup<IGetAllForumsStorage,Task<Result<IReadOnlyList<ForumGetDto>, Error>>> getForumsSetup;
    private readonly Mock<IGetAllForumsStorage> storage;

    public GetForumsUseCaseShould()
    {
        storage = new Mock<IGetAllForumsStorage>();
        getForumsSetup = storage.Setup(s => s.GetAllSortedForums( It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));

        sut = new GetAllForumsUseCase(storage.Object);
    }

    [Fact]
    public async Task ReturnForums_FromStorage()
    {
        var forums = new ForumGetDto[]
        {
            new(Guid.NewGuid(), "Some title 1", DateTimeOffset.UtcNow),
            new(Guid.NewGuid(), "Some title 2", DateTimeOffset.UtcNow)
        };
        getForumsSetup.ReturnsAsync(forums);

        var query = new GetAllSortedForumsQuery("created", "abc");
        
        var actual = await sut.Execute(query, CancellationToken.None);
        actual.Value.Should().BeEquivalentTo(forums);
        storage.Verify(s => s.GetAllSortedForums("created", "abc",CancellationToken.None), Times.Once);
        storage.VerifyNoOtherCalls();
    }
}