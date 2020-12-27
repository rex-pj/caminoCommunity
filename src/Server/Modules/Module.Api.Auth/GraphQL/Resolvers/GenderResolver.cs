using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using Camino.Data.Enums;
using System.Collections.Generic;
using Camino.Core.Utils;
using Camino.Core.Models;

namespace  Module.Api.Auth.GraphQL.Resolvers
{
    public class GenderResolver : IGenderResolver
    {
        public IEnumerable<SelectOption> GetSelections()
        {
            return EnumUtil.EnumToSelectList<GenderType>();
        }
    }
}
