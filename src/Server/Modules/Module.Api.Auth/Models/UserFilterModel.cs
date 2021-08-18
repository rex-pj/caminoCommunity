using Camino.Framework.Models;

namespace Module.Api.Auth.Models
{
    public class UserFilterModel : BaseFilterModel
    {
        public UserFilterModel() : base()
        {
        }

        public string ExclusiveUserIdentityId { get; set; }
        public string UserIdentityId { get; set; }
    }
}
