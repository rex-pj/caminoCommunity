using Camino.Infrastructure.AspNetCore.Models;

namespace Module.Navigation.Api.Models
{
    public class ShortcutFilterModel : BaseFilterModel
    {
        public int TypeId { get; set; }
    }
}
