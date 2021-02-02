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

        public long Id { get; set; }
        public string ExclusiveCreatedIdentityId { get; set; }
        public string UserIdentityId { get; set; }
    }
}
