using Camino.Infrastructure.AspNetCore.Models;

namespace Module.Api.Farm.Models
{
    public class FarmSelectFilterModel : BaseSelectFilterModel
    {
        public long[] CurrentIds { get; set; }
    }
}
