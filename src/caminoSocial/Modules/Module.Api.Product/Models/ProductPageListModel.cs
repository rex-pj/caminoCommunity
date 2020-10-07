using Camino.Framework.Models;
using System.Collections.Generic;

namespace Module.Api.Product.Models
{
    public class ProductPageListModel: PageListModel<ProductModel>
    {
        public ProductPageListModel(IEnumerable<ProductModel> collections) : base(collections)
        {
        }
    }
}
