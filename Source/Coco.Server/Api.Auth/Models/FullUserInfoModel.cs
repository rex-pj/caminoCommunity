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
            //GenderSelections = new List<SelectOption>();
        }

        public FullUserInfoModel(UserInfoModel userInfo, IEnumerable<SelectOption> genderOptions,
            IEnumerable<SelectOption> countrySelections) : base(userInfo)
        {
            //GenderSelections = genderOptions;
            //CountrySelections = countrySelections;
        }

        //public string AvatarUrl { get; set; }
        public string CoverPhotoUrl { get; set; }
        //public bool CanEdit { get; set; }
        //public IEnumerable<ISelectOption> GenderSelections { get; set; }
        //public IEnumerable<ISelectOption> CountrySelections { get; set; }
    }
}
