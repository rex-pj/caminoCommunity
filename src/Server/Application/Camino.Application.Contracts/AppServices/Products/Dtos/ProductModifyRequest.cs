using Camino.Application.Contracts.AppServices.Media.Dtos;

namespace Camino.Application.Contracts.AppServices.Products.Dtos
{
    public class ProductModifyRequest
    {
        public ProductModifyRequest()
        {
            Pictures = new List<PictureRequest>();
            Categories = new List<ProductCategoryRequest>();
            Farms = new List<ProductFarmRequest>();
            ProductAttributes = new List<ProductAttributeRelationRequest>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long UpdatedById { get; set; }
        public long CreatedById { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public IEnumerable<ProductCategoryRequest> Categories { get; set; }
        public IEnumerable<ProductFarmRequest> Farms { get; set; }
        public IEnumerable<PictureRequest> Pictures { get; set; }
        public IEnumerable<ProductAttributeRelationRequest> ProductAttributes { get; set; }
    }
}
