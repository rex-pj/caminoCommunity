using Camino.Service.Data.Identity;

namespace Camino.Service.Data.Request
{
    public class UserFullProjection : UserProjection
    {
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string GenderLabel { get; set; }
        public string StatusLabel { get; set; }
    }
}
