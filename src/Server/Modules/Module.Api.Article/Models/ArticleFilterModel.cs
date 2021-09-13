﻿using Camino.Framework.Models;
using HotChocolate;
using HotChocolate.Types;

namespace Module.Api.Article.Models
{
    public class ArticleFilterModel : BaseFilterModel
    {
        public ArticleFilterModel() : base()
        {
        }

        public long? Id { get; set; }
        public string UserIdentityId { get; set; }
    }
}
