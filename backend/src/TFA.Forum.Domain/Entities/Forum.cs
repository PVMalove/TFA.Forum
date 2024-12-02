using TFA.Forum.Domain.Entities.Interfaces;
using TFA.Forum.Domain.EntityIds;
using TFA.Forum.Domain.ValueObjects;

namespace TFA.Forum.Domain.Entities;

public class Forum : Entity<ForumId>, IAuditable
{
    public Title Title  { get; init; } = null!;
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; set; }
    
    public IReadOnlyList<Topic> Topics { get; private set;  } = null!;
    
    protected Forum(ForumId id) : base(id) { }
    
    private Forum(ForumId id, Title title, DateTimeOffset createdAt) : base(id)
    {
        Title = title;
        CreatedAt = createdAt;
    }
    
    public static Forum Create(ForumId id, Title title, DateTimeOffset createdAt)
    {
        return new Forum(id, title, createdAt);
    }
}