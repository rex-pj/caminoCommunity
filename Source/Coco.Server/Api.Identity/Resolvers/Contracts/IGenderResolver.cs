using Coco.Framework.Models;
using System.Collections.Generic;

namespace Api.Identity.Resolvers.Contracts
{
    public interface IGenderResolver
    {
        IEnumerable<ISelectOption> GetSelections();
    }
}
