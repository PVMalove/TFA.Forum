using TFA.Forum.Domain.Entities.Interfaces;

namespace TFA.Forum.Domain.Entities;

public class Topic : IAuditable
{
    public Guid Id { get; set; }
    
    public string? Title { get; set; }

    public string? Content { get; set; }
    
    public User Author { get; set; }
    
    public Guid AuthorId { get; set; }

    public Forum Forum { get; set; }
    
    public Guid ForumId { get; set; }

    public ICollection<Comment> Comments { get; set; }
    
    public DateTimeOffset CreateAt { get; set; }
    
    public DateTimeOffset? UpdateAt { get; set; }
}