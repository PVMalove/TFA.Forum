using CSharpFunctionalExtensions;
using TFA.Forum.Domain.Shared;

namespace TFA.Forum.Domain.ValueObjects;

public record Title
{
    public string Value { get; }

    private Title(string value) => Value = value;

    public static Result<Title, Error> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 50)
            return Errors.General.ValueIsInvalid("title");

        return new Title(value);
    }
}