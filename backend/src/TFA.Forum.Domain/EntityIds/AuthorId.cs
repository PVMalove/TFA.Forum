using TFA.Forum.Domain.Interfaces;

namespace TFA.Forum.Domain.EntityIds;

public record AuthorId
{
    public Guid Id { get; }
    
    private AuthorId(Guid id) => Id = id;

    public static AuthorId NewId(IGuidFactory guidFactory) => new(guidFactory.Create());
    public static AuthorId Create(Guid id) => new(id);
    public static implicit operator Guid(AuthorId authorId) => authorId.Id;
}