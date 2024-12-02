using CSharpFunctionalExtensions;
using TFA.Forum.Domain.Shared;

namespace TFA.Forum.Domain.ValueObjects;

public record Content
{
    public string Value { get; }

    private Content(string value) => Value = value;

    public static Result<Content, Error> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsInvalid("contrent");

        if (value.Length > Constants.MAX_HIGH_TEXT_LENGTH_2000)
            return Errors.General.ValueIsRequired("contrent", value.Length);

        return new Content(value);
    }
};