using TFA.Forum.Domain.Entities.Interfaces;
using TFA.Forum.Domain.EntityIds;

namespace TFA.Forum.Domain.Entities;

public class Comment : IAuditable
{
    public Guid Id { get; set; }
    
    public string Content { get; set; }

    public DateTimeOffset CreatedAt { get; init; }
    
    public DateTimeOffset UpdatedAt { get; set; }
    
    public User Author { get; set; }
    
    public UserId UserId  { get; set; }
    
    public Topic Topic { get; set; }
    
    public TopicId TopicId { get; set; }
}