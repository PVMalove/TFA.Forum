using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TFA.Forum.Domain.Entities;
using TFA.Forum.Domain.EntityIds;
using TFA.Forum.Domain.Shared;

namespace TFA.Forum.Persistence.Configurations;

public class UserConfigure : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .HasConversion(
                id => id.Id,
                result => UserId.Create(result)
            );
        
        builder.Property(u => u.Login).IsRequired().HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH_25);
        builder.Property(u => u.Salt).HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH_50);
        builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(Constants.MAX_LOW_PASSWORD_HASH_LENGTH_32);
        
        builder.HasMany(u => u.Topics)
            .WithOne(t => t.Author)
            .HasForeignKey(t => t.UserId);
        
        builder.HasMany(u => u.Comments)
            .WithOne(c => c.Author)
            .HasForeignKey(c => c.UserId);
        
        builder.HasMany(u => u.Sessions)
            .WithOne(s => s.User)
            .HasForeignKey(c => c.UserId);
    }
}