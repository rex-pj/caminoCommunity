using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using Camino.Shared.Enums;
using System.Collections.Generic;
using Camino.Core.Utils;
using Camino.Shared.General;

namespace Module.Api.Auth.GraphQL.Resolvers
{
    public class GenderResolver : IGenderResolver
    {
        public IEnumerable<SelectOption> GetSelections()
        {
            return EnumUtil.ToSelectOptions<GenderType>();
        }
    }
}
