using Camino.Infrastructure.AspNetCore.Models;

namespace Module.Auth.Api.Models
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
