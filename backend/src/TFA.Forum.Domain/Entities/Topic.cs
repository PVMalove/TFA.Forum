using TFA.Forum.Domain.Entities.Interfaces;
using TFA.Forum.Domain.EntityIds;
using TFA.Forum.Domain.ValueObjects;

namespace TFA.Forum.Domain.Entities;

public class Topic : Entity<TopicId>, IAuditable
{
    public Title Title { get; init; } = null!;
    public Content Content { get; init; } = null!;

    public User Author { get; private set; }  = null!;
    public AuthorId AuthorId { get; init; } = null!;
    public Forum Forum { get; private set; }  = null!;
    public ForumId ForumId { get; init; } = null!;
    public IReadOnlyList<Comment> Comments { get; private set; } = null!;

    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; set; }

    protected Topic(TopicId id) : base(id) { }

    private Topic(TopicId id, ForumId forumId, AuthorId authorId, Title title, Content content, DateTimeOffset createdAt) : base(id)
    {
        ForumId = forumId;
        AuthorId = authorId;
        Title = title;
        Content = content;
        CreatedAt = createdAt;
    }

    public static Topic Create(TopicId id, ForumId forumId, AuthorId authorId, Title title, Content content, DateTimeOffset createdAt )
    {
        return new Topic(id, forumId, authorId, title, content, createdAt);
    }
}
