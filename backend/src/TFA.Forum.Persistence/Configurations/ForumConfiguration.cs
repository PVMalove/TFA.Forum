using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TFA.Forum.Domain.EntityIds;
using TFA.Forum.Domain.Shared;

namespace TFA.Forum.Persistence.Configurations;

public class ForumConfiguration : IEntityTypeConfiguration<Forum.Domain.Entities.Forum>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Forum> builder)
    {
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id)
            .HasConversion(
                id => id.Id,
                result => ForumId.Create(result)
            );
        
        builder.ComplexProperty(f => f.Title, fb =>
        {
            fb.Property(f => f.Value)
                .HasColumnName("title")
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH_50);
        });
        
        builder
            .HasMany(f => f.Topics)
            .WithOne(t => t.Forum)
            .HasForeignKey(t => t.ForumId);
    }
}