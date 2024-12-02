using TFA.Forum.Domain.Entities.Interfaces;
using TFA.Forum.Domain.EntityIds;

namespace TFA.Forum.Domain.Entities;

public class User : Entity<AuthorId>
{
    public string Login { get; init; } = null!;
    public byte[] Salt { get; set; }
    public byte[] PasswordHash { get; set; }
    public ICollection<Topic> Topics { get; init; } = null!;
    public ICollection<Comment> Comments { get; init; } = null!;
    
    protected User(AuthorId id) : base(id) { }
    
    private User(AuthorId id, string login, byte[] salt, byte[] passwordHash) : base(id)
    {
        Login = login;
        Salt = salt;
        PasswordHash = passwordHash;
    }
    
    public static User Create(AuthorId id, string login, byte[] salt, byte[] passwordHash)
    {
        return new User(id, login, salt, passwordHash);
    }
}