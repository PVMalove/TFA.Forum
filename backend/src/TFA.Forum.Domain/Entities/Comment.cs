using TFA.Forum.Domain.Entities.Interfaces;

namespace TFA.Forum.Domain.Entities;

public class Comment : IAuditable
{
    public Guid Id { get; set; }
    
    public string Content { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset UpdatedAt { get; set; }
    
    public User Author { get; set; }
    
    public Guid AuthorId  { get; set; }
    
    public Topic Topic { get; set; }
    
    public Guid TopicId { get; set; }
}