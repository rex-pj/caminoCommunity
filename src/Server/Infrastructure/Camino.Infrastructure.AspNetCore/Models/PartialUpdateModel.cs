using System.ComponentModel.DataAnnotations;

namespace Camino.Infrastructure.AspNetCore.Models
{
    public class PartialUpdateModel
    {
        [Required]
        public string Key { get; set; }
        public IList<PartialUpdateItemModel> Updates { get; set; }
    }
}
