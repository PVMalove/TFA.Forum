using TFA.Forum.Domain.Entities.Interfaces;
using TFA.Forum.Domain.EntityIds;

namespace TFA.Forum.Domain.Entities;

public class User : Entity<UserId>
{
    public string Login { get; init; } = null!;
    public byte[] Salt { get; set; }
    public byte[] PasswordHash { get; set; }
    public ICollection<Topic> Topics { get; init; } = null!;
    public ICollection<Comment> Comments { get; init; } = null!;
    public ICollection<Session> Sessions { get; init; } = null!;
    
    protected User(UserId id) : base(id) { }
    
    private User(UserId id, string login, byte[] salt, byte[] passwordHash) : base(id)
    {
        Login = login;
        Salt = salt;
        PasswordHash = passwordHash;
    }
    
    public static User Create(UserId id, string login, byte[] salt, byte[] passwordHash)
    {
        return new User(id, login, salt, passwordHash);
    }
}