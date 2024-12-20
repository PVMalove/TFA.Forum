﻿using FluentAssertions;
using TFA.Forum.Application.Queries.GetTopics;

namespace TFA.Forum.Domain.UnitTests.GetTopics;

public class GetTopicsQueryValidatorShould
{
    private readonly GetTopicsWithPaginationValidator sut = new();

    [Fact]
    public void ReturnSuccess_WhenQueryIsValid()
    {
        var query = new GetTopicsWithPaginationQuery(
            Guid.Parse("DA60E33E-7F32-4BFC-A4FF-E19F9BFE934B"),
            "created",
            "desc",
            10,
            5);
        sut.Validate(query).IsValid.Should().BeTrue();
    }

    public static IEnumerable<object[]> GetInvalidQuery()
    {
        var query = new GetTopicsWithPaginationQuery(Guid.Parse("51218A0B-4BEA-437A-84DD-461245D8CD86"), "created", "desc", 10, 5);
        yield return [query with { ForumId = Guid.Empty }];
        yield return [query with { Page = -40 }];
        yield return [query with { PageSize = -1 }];
    }

    [Theory]
    [MemberData(nameof(GetInvalidQuery))]
    public void ReturnFailure_WhenQueryIsInvalid(GetTopicsWithPaginationQuery query)
    {
        sut.Validate(query).IsValid.Should().BeFalse();
    }
}