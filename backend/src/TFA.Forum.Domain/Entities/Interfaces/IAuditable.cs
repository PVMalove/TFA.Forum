namespace TFA.Forum.Domain.Entities.Interfaces;

public interface IAuditable
{
    DateTimeOffset CreateAt { get; set; }
    DateTimeOffset? UpdateAt { get; set; }
}