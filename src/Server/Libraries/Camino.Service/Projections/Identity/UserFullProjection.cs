using Camino.Service.Projections.Identity;

namespace Camino.Service.Projections.Request
{
    public class UserFullProjection : UserProjection
    {
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string GenderLabel { get; set; }
        public string StatusLabel { get; set; }
    }
}
