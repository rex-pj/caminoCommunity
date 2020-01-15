using System.Collections.Generic;

namespace Coco.Api.Framework.Models
{
    public class UserInfoExtend : UserInfoModel
    {
        public UserInfoExtend()
        {

        }

        public UserInfoExtend(UserInfoModel userInfo) : base(userInfo)
        {
            GenderSelections = new List<SelectOption>();
        }

        public UserInfoExtend(UserInfoModel userInfo, IEnumerable<SelectOption> genderOptions,
            IEnumerable<SelectOption> countrySelections) : base(userInfo)
        {
            GenderSelections = genderOptions;
            CountrySelections = countrySelections;
        }

        public IEnumerable<SelectOption> GenderSelections { get; set; }
        public IEnumerable<SelectOption> CountrySelections { get; set; }
    }
}
