using Camino.Framework.Models;

namespace Module.Api.Content.Models
{
    public class ArticleFilterModel : BaseFilterModel
    {
        public ArticleFilterModel() : base()
        {

        }

        public string UserIdentityId { get; set; }
    }
}
