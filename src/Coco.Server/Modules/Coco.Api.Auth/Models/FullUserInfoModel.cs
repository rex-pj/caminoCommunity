using Coco.Core.Dtos.General;
using Coco.Framework.Models;
using System.Collections.Generic;

namespace  Coco.Api.Auth.Models
{
    public class FullUserInfoModel : UserInfoModel
    {
        public IEnumerable<ISelectOption> GenderSelections { get; set; }
        public IEnumerable<ISelectOption> CountrySelections { get; set; }
    }
}
