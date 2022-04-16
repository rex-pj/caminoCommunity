using System;
using System.Collections.Generic;

namespace Camino.Core.Domain.Articles
{
    public class Article
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public long UpdatedById { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public long CreatedById { get; set; }
        public int ArticleCategoryId { get; set; }
        public int StatusId { get; set; }

        public virtual ArticleCategory ArticleCategory { get; set; }
    }
}
