using FluentAssertions;
using TFA.Forum.Application.Commands.CreateTopic;

namespace TFA.Forum.Domain.UnitTests;

public class CreateTopicValidatorShould
{
    private readonly CreateTopicValidator sut = new();

    [Fact]
    public void ReturnSuccess_WhenCommandIsValid()
    {
        var forumId = Guid.Parse("09EB8B65-A5D4-4171-B4C5-B34905DAE1DB");
        
        var command = new CreateTopicCommand(forumId, "Some title", "Some content");
        var actual = sut.Validate(command);
        actual.IsValid.Should().BeTrue();
    }

    public static IEnumerable<object[]> GetInvalidCommand()
    {
        var forumId = Guid.Parse("09EB8B65-A5D4-4171-B4C5-B34905DAE1DB");
        var validCommand = new CreateTopicCommand(forumId, "Some title", "Some content");
        
        yield return [validCommand with{ForumId = Guid.Empty}];
        yield return [validCommand with{Title = ""}];
        yield return [validCommand with{Title = "   "}];
        yield return [validCommand with{Title = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet"}];
        yield return [validCommand with{Content = ""}];
        yield return [validCommand with{Content = "   "}];
    }
    
    [Theory]
    [MemberData(nameof(GetInvalidCommand))]
    public void ReturnFailure_WhenCommandIsValid(CreateTopicCommand command)
    {
        var actual = sut.Validate(command);
        actual.IsValid.Should().BeFalse();
    }
}