using Camino.Core.Models;
using Camino.Framework.Models;
using System.Collections.Generic;

namespace  Module.Api.Auth.Models
{
    public class FullUserInfoModel : UserInfoModel
    {
        public IEnumerable<ISelectOption> GenderSelections { get; set; }
        public IEnumerable<ISelectOption> CountrySelections { get; set; }
    }
}
