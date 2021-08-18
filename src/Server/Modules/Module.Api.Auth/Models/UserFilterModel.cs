using Camino.Framework.Models;

namespace Module.Api.Auth.Models
{
    public class UserFilterModel : BaseFilterModel
    {
        public UserFilterModel() : base()
        {
            Page = 1;
            PageSize = 10;
        }

        public string ExclusiveUserIdentityId { get; set; }
        public string UserIdentityId { get; set; }
    }
}
