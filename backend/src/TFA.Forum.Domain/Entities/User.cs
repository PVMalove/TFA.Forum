namespace TFA.Forum.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }
    
    public ICollection<Topic> Topics { get; set; }
    
    public ICollection<Comment> Comments { get; set; }
}