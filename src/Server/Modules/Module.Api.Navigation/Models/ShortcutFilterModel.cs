using Camino.Infrastructure.AspNetCore.Models;

namespace Module.Api.Navigation.Models
{
    public class ShortcutFilterModel : BaseFilterModel
    {
        public int TypeId { get; set; }
    }
}
