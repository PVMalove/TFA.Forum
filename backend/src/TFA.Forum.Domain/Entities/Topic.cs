using TFA.Forum.Domain.Entities.Interfaces;

namespace TFA.Forum.Domain.Entities;

public class Topic : IAuditable
{
    public Guid Id { get; set; }
    
    public string Title { get; set; }

    public string Content { get; set; }
    
    public User Author { get; set; }
    
    public Guid AuthorId { get; set; }
    
    public ICollection<Comment> Comments { get; set; }
    
    public DateTime CreateAt { get; set; }
    
    public DateTime? UpdateAt { get; set; }
}