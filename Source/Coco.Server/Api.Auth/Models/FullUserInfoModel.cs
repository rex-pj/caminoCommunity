using Coco.Entities.Models;
using Coco.Framework.Models;
using System.Collections.Generic;

namespace Api.Auth.Models
{
    public class FullUserInfoModel : UserInfoModel
    {
        public FullUserInfoModel()
        {

        }

        public FullUserInfoModel(UserInfoModel userInfo) : base(userInfo)
        {
        }

        public FullUserInfoModel(UserInfoModel userInfo, IEnumerable<SelectOption> genderOptions,
            IEnumerable<SelectOption> countrySelections) : base(userInfo)
        {
        }
    }
}
