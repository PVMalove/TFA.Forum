using CSharpFunctionalExtensions;
using TFA.Forum.Domain.Shared;

namespace TFA.Forum.Domain.ValueObjects;

public record Title
{
    public string Value { get; }

    private Title(string value) => Value = value;

    public static Result<Title, Error> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsInvalid("title");
       
        if (value.Length > Constants.MAX_LOW_TEXT_LENGTH_50)
            return Errors.General.ValueIsRequired("title", value.Length);

        return new Title(value);
    }
}