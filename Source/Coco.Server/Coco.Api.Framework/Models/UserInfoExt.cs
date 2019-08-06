using System.Collections.Generic;

namespace Coco.Api.Framework.Models
{
    public class UserInfoExt : UserInfoModel
    {
        public UserInfoExt()
        {

        }

        public UserInfoExt(UserInfoModel userInfo) : base(userInfo)
        {
            GenderSelections = new List<SelectOption>();
        }

        public UserInfoExt(UserInfoModel userInfo, IEnumerable<SelectOption> genderOptions,
            IEnumerable<SelectOption> countrySelections) : base(userInfo)
        {
            GenderSelections = genderOptions;
            CountrySelections = countrySelections;
        }

        public IEnumerable<SelectOption> GenderSelections { get; set; }
        public IEnumerable<SelectOption> CountrySelections { get; set; }
    }
}
