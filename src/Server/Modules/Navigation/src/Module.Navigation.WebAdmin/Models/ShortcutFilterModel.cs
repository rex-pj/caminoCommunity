using Camino.Infrastructure.AspNetCore.Models;

namespace Module.Navigation.WebAdmin.Models
{
    public class ShortcutFilterModel : BaseFilterModel
    {
        public int? TypeId { get; set; }
        public int? StatusId { get; set; }
    }
}
