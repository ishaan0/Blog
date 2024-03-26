﻿using Blog.Domain.Enums;
using Blog.Domain.IdentityEntities;

namespace Blog.Domain.Entities
{
    public class Article
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public string? CoverImage { get; set; }
        public ArticleStatus Status { get; set; }
        public Guid? AuthorId { get; set; }
        public User? Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}
