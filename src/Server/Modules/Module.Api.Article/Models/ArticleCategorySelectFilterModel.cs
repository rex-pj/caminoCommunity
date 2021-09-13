﻿using Camino.Framework.Models;

namespace Module.Api.Article.Models
{
    public class ArticleCategorySelectFilterModel : BaseSelectFilterModel
    {
        public int? CurrentId { get; set; }
        public bool? IsParentOnly { get; set; }
    }
}
