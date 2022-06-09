namespace Camino.Application.Contracts.AppServices.Products.Dtos
{
    public class ProductAttributeFilter : BaseFilter
    {
        public ProductAttributeFilter()
        {
            ExcludedIds = new List<int>();
        }

        public int? StatusId { get; set; }
        public int? Id { get; set; }
        public bool CanGetInactived { get; set; }
        public IEnumerable<int> ExcludedIds { get; set; }
    }
}
