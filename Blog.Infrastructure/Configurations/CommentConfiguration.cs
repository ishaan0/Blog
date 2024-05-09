using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Body)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(b => b.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(b => b.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Article)
            .WithMany(a => a.Comments)
            .HasForeignKey(b => b.ArticleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

    }
}
