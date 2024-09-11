using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasOne(t => t.Article)
            .WithMany(a => a.Tags)
            .HasForeignKey(t => t.ArticleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
