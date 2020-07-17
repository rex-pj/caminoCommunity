using Module.Api.Auth.Resolvers.Contracts;
using Camino.Data.Enums;
using System.Collections.Generic;
using Camino.Core.Utils;
using Camino.Core.Models;

namespace  Module.Api.Auth.Resolvers
{
    public class GenderResolver : IGenderResolver
    {
        public IEnumerable<ISelectOption> GetSelections()
        {
            return EnumUtil.EnumToSelectList<GenderType>();
        }
    }
}
