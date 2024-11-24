using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TFA.Forum.Domain.Shared;

namespace TFA.Forum.Persistence.Configurations;

public class ForumConfiguration : IEntityTypeConfiguration<Forum.Domain.Entities.Forum>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Forum> builder)
    {
        builder.HasKey(f => f.Id);
        
        builder.ComplexProperty(f => f.Title, fb =>
        {
            fb.Property(f => f.Value)
                .HasColumnName("title")
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_SOLT_LENGTH_50);
        });
        
        builder
            .HasMany(f => f.Topics);
    }
}