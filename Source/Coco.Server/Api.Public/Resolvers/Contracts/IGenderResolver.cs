using Coco.Api.Framework.Models;
using System.Collections.Generic;

namespace Api.Public.Resolvers.Contracts
{
    public interface IGenderResolver
    {
        IEnumerable<ISelectOption> GetSelections();
    }
}
