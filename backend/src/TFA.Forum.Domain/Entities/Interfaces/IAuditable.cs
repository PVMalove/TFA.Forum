namespace TFA.Forum.Domain.Entities.Interfaces;

public interface IAuditable
{
    DateTimeOffset CreatedAt { get; init; }
    DateTimeOffset UpdatedAt { get; set; }
}