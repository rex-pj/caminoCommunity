using Camino.Infrastructure.AspNetCore.Models;
using System.Collections.Generic;

namespace Module.Product.Api.Models
{
    public class ProductPageListModel: PageListModel<ProductModel>
    {
        public ProductPageListModel(IEnumerable<ProductModel> collections) : base(collections)
        {
        }
    }
}
