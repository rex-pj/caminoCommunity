using Camino.Infrastructure.AspNetCore.Models;

namespace Module.Farm.Api.Models
{
    public class FarmSelectFilterModel : BaseSelectFilterModel
    {
        public long[] CurrentIds { get; set; }
    }
}
