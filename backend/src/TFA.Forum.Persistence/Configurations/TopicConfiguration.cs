using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TFA.Forum.Domain.Entities;
using TFA.Forum.Persistence.Shared;

namespace TFA.Forum.Persistence.Configurations;

public class TopicConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Title).IsRequired().HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH_100);
        builder.Property(t => t.Content).IsRequired().HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH_2000);
        builder.HasMany(t => t.Comments)
            .WithOne(c => c.Topic)
            .HasForeignKey(c => c.TopicId);
    }
}