using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TFA.Forum.Domain.Entities;
using TFA.Forum.Domain.EntityIds;
using TFA.Forum.Domain.Shared;

namespace TFA.Forum.Persistence.Configurations;

public class TopicConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .HasConversion(
                id => id.Id,
                result => TopicId.Create(result)
            );
        
        builder.ComplexProperty(f => f.Title, fb =>
        {
            fb.Property(f => f.Value)
                .HasColumnName("title")
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH_50);
        });
        
        builder.ComplexProperty(f => f.Content, fb =>
        {
            fb.Property(f => f.Value)
                .HasColumnName("content")
                .IsRequired()
                .HasMaxLength(Constants.MAX_HIGH_TEXT_LENGTH_2000);
        });

        builder.HasMany(t => t.Comments)
            .WithOne(c => c.Topic)
            .HasForeignKey(c => c.TopicId);
    }
}