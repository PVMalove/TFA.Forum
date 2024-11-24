namespace TFA.Forum.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public required string Login { get; set; }
    public byte[] Salt { get; set; }
    public byte[] PasswordHash { get; set; }
    public ICollection<Topic> Topics { get; set; }
    public ICollection<Comment> Comments { get; set; }
}