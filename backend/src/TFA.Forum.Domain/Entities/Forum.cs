namespace TFA.Forum.Domain.Entities;

public class Forum
{
    public Guid Id { get; set; }
    
    public string? Title { get; set; }
    
    public ICollection<Topic> Topics { get; set; }
}