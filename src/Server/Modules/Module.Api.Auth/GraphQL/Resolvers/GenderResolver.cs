using Module.Api.Auth.GraphQL.Resolvers.Contracts;
using System.Collections.Generic;
using Camino.Application.Contracts;
using Camino.Application.Contracts.Utils;
using Camino.Shared.Enums;

namespace Module.Api.Auth.GraphQL.Resolvers
{
    public class GenderResolver : IGenderResolver
    {
        public IEnumerable<SelectOption> GetSelections()
        {
            return SelectOptionUtils.ToSelectOptions<GenderTypes>();
        }
    }
}
