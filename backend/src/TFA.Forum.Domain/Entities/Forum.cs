using TFA.Forum.Domain.Entities.Interfaces;
using TFA.Forum.Domain.ValueObjects;

namespace TFA.Forum.Domain.Entities;

public class Forum : IAuditable
{
    public Guid Id { get; init; }
    public Title Title { get; init; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    
    public ICollection<Topic> Topics { get; init; }
    
    /// CtorEF
    private Forum() { }
    
    public Forum(Guid id, Title title, DateTimeOffset createdAt)
    {
        Id = id;
        Title = title;
        CreatedAt = createdAt;
    }
}