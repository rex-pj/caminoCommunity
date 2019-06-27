using System.Collections.Generic;

namespace Coco.Api.Framework.Models
{
    public class UserInfoExt : UserInfo
    {
        public UserInfoExt(UserInfo userInfo) : base(userInfo)
        {

        }

        public UserInfoExt(UserInfo userInfo, IEnumerable<SelectOption> genderOptions) : base(userInfo)
        {
            GenderSelections = genderOptions;
        }

        public IEnumerable<SelectOption> GenderSelections { get; set; }
    }
}
