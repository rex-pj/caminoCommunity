using Camino.Infrastructure.AspNetCore.Models;
using System.Collections.Generic;

namespace Module.Api.Auth.Models
{
    public class UserPageListModel : PageListModel<UserInfoModel>
    {
        public UserPageListModel(IEnumerable<UserInfoModel> collections) : base(collections)
        {
        }
    }
}
