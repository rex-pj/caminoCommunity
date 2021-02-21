using Camino.Framework.Models;
using HotChocolate;
using HotChocolate.Types;
using System;

namespace Module.Api.Article.Models
{
    public class ArticleModel
    {
        [GraphQLType(typeof(LongType))]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public PictureRequestModel Picture { get; set; }
        [GraphQLType(typeof(DateTimeType))]
        public DateTimeOffset CreatedDate { get; set; }
        [GraphQLType(typeof(LongType))]
        public long CreatedById { get; set; }
        [GraphQLType(typeof(DateTimeType))]
        public DateTimeOffset UpdatedDate { get; set; }
        [GraphQLType(typeof(LongType))]
        public long UpdatedById { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByPhotoCode { get; set; }
        public string CreatedByIdentityId { get; set; }
        public int ArticleCategoryId { get; set; }
        public string ArticleCategoryName { get; set; }
    }
}
