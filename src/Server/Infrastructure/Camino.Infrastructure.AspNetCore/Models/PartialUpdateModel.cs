namespace Camino.Infrastructure.AspNetCore.Models
{
    public class PartialUpdateModel
    {
        public string Key { get; set; }
        public IList<PartialUpdateItemModel> Updates { get; set; }
    }
}
