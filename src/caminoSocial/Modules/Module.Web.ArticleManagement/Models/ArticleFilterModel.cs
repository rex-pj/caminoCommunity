using Camino.Framework.Models;
using System;

namespace Module.Web.ArticleManagement.Models
{
    public class ArticleFilterModel : BaseFilterModel
    {
        public string Content { get; set; }
        public DateTimeOffset? CreatedDateFrom { get; set; }
        public DateTimeOffset? CreatedDateTo { get; set; }
        public long? CreatedById { get; set; }
        public long? UpdatedById { get; set; }
    }
}
