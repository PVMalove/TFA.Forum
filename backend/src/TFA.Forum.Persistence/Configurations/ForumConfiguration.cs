using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TFA.Forum.Persistence.Shared;


namespace TFA.Forum.Persistence.Configurations;

public class ForumConfiguration : IEntityTypeConfiguration<Forum.Domain.Entities.Forum>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Forum> builder)
    {
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Title).IsRequired().HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH_100);
        builder
            .HasMany(f => f.Topics);
    }
}