﻿using Camino.Framework.Models;
using System;

namespace Module.Web.ArticleManagement.Models
{
    public class ArticleFilterModel : BaseFilterModel
    {
        public string Content { get; set; }
        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
        public long? CategoryId { get; set; }
        public int? StatusId { get; set; }
    }
}
