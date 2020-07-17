using Camino.Business.Dtos.General;
using Camino.Core.Models;
using System.Collections.Generic;

namespace  Module.Api.Auth.Resolvers.Contracts
{
    public interface IGenderResolver
    {
        IEnumerable<ISelectOption> GetSelections();
    }
}
