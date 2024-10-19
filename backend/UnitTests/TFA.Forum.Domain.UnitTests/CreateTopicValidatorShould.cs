using FluentAssertions;
using TFA.Forum.Application.Queries.CreateTopic;

namespace TFA.Forum.Domain.UnitTests;

public class CreateTopicValidatorShould
{
    private readonly CreateTopicValidator sut = new();

    [Fact]
    public void ReturnSuccess_WhenQueryIsValid()
    {
        var forumId = Guid.Parse("09EB8B65-A5D4-4171-B4C5-B34905DAE1DB");
        
        var query = new CreateTopicQuery(forumId, "Some title", "Some content");
        var actual = sut.Validate(query);
        actual.IsValid.Should().BeTrue();
    }

    public static IEnumerable<object[]> GetInvalidQueries()
    {
        var forumId = Guid.Parse("09EB8B65-A5D4-4171-B4C5-B34905DAE1DB");
        var validQuery = new CreateTopicQuery(forumId, "Some title", "Some content");
        
        yield return [validQuery with{ForumId = Guid.Empty}];
        yield return [validQuery with{Title = ""}];
        yield return [validQuery with{Title = "   "}];
        yield return [validQuery with{Title = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet"}];
        yield return [validQuery with{Content = ""}];
        yield return [validQuery with{Content = "   "}];
    }
    
    [Theory]
    [MemberData(nameof(GetInvalidQueries))]
    public void ReturnFailure_WhenQueryIsValid(CreateTopicQuery query)
    {
        var actual = sut.Validate(query);
        actual.IsValid.Should().BeFalse();
    }
}