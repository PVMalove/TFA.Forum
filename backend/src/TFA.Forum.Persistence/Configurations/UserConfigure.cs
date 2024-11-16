using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TFA.Forum.Domain.Entities;
using TFA.Forum.Persistence.Shared;

namespace TFA.Forum.Persistence.Configurations;

public class UserConfigure : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.FirstName).IsRequired().HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH_50);
        builder.Property(u => u.LastName).IsRequired().HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH_50);
        builder.HasMany(u => u.Topics)
            .WithOne(t => t.Author)
            .HasForeignKey(t => t.AuthorId);
        builder.HasMany(u => u.Comments)
            .WithOne(c => c.Author)
            .HasForeignKey(c => c.AuthorId);
    }
}